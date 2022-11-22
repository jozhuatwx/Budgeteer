
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Budgeteer.Server.Services;

public class SessionService : ISessionService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly JwtOptions _jwtOptions;
    private readonly IMapper _mapper;

    public SessionService(
        UnitOfWork unitOfWork,
        IOptions<BudgeteerOptions> options,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _jwtOptions = options.Value.Jwt;
        _mapper = mapper;
    }

    public async Task<UserSessionResponse?> LoginUserAsync(
        LoginUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users
            .GetAsync(user => user.Email == request.Email, _mapper.ConfigurationProvider, cancellationToken: cancellationToken, includes: user => user.RefreshTokens);

        if (user == null || !await CryptographyUtility.VerifyHashedPasswordAsync(user.HashedPassword, request.Password, cancellationToken))
        {
            return null;
        }

        return new(await GenerateJwtAsync(user), await GenerateRefreshTokenAsync(user, cancellationToken));
    }

    public async Task<UserSessionResponse?> RefreshUserSessionAsync(
        int id, RefreshUserSessionRequest request, CancellationToken cancellationToken = default)
    {
        var refreshToken = await _unitOfWork.RefreshTokens
            .GetAsync(token => token.UserId == id && token.Token == request.RefreshToken, _mapper.ConfigurationProvider, cancellationToken: cancellationToken);

        if (refreshToken == null || refreshToken.IsExpired)
        {
            return null;
        }

        var user = await _unitOfWork.Users
            .GetAsync(user => user.Id == id, _mapper.ConfigurationProvider, cancellationToken: cancellationToken, includes: user => user.RefreshTokens);

        if (user == null)
        {
            return null;
        }

        return new(await GenerateJwtAsync(user), await GenerateRefreshTokenAsync(user, cancellationToken));
    }

    private async Task<string> GenerateJwtAsync(
        User user)
    {
        return await Task.Run(() =>
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new(new Claim[]
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Email, user.Email)
            }),
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        });
    }

    private async Task<string> GenerateRefreshTokenAsync(
        User user, CancellationToken cancellationToken = default)
    {
        if (user.RefreshTokens.Any())
        {
            _unitOfWork.RefreshTokens.DeleteRange(user.RefreshTokens);
        }

        var refreshToken = new RefreshToken()
        {
            Token = await CryptographyUtility.GenerateRandomCharactersAsync(12, cancellationToken),
            UserId = user.Id,
            ExpiryDateTime = DateTime.UtcNow.AddDays(7)
        };

        _unitOfWork.RefreshTokens.Create(refreshToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return refreshToken.Token;
    }
}

