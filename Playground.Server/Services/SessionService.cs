using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Playground.Server.Services;

public class SessionService : ISessionService
{
    private readonly PlaygroundContext _context;
    private readonly JwtOptions _jwtOptions;

    public SessionService(
        IDbContextFactory<PlaygroundContext> contextFactory,
        IOptions<PlaygroundOptions> options)
    {
        _context = contextFactory.CreateDbContext();
        _jwtOptions = options.Value.Jwt;
    }

    public async Task<UserSessionResponse?> LoginUserAsync(
        LoginUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .Include((user) => user.RefreshTokens)
            .GetAsync((user) => user.Email == request.Email, cancellationToken: cancellationToken);

        if (user == null || !await CryptographyUtility.VerifyHashedPasswordAsync(user.HashedPassword, request.Password, cancellationToken))
        {
            return null;
        }

        return new(await GenerateJwtAsync(user), await GenerateRefreshTokenAsync(user, cancellationToken));
    }

    public async Task<UserSessionResponse?> RefreshUserSessionAsync(
        int id, RefreshUserSessionRequest request, CancellationToken cancellationToken = default)
    {
        var refreshToken = await _context.RefreshTokens.GetAsync(token => token.UserId == id && token.Token == request.RefreshToken, cancellationToken: cancellationToken);

        if (refreshToken == null || refreshToken.IsExpired)
        {
            return null;
        }

        var user = await _context.Users
            .Include((user) => user.RefreshTokens)
            .GetAsync((user) => user.Id == id, cancellationToken: cancellationToken);

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
            _context.RefreshTokens.DeleteRange(user.RefreshTokens);
        }

        var refreshToken = new RefreshToken()
        {
            Token = await CryptographyUtility.GenerateRandomCharactersAsync(12, cancellationToken),
            UserId = user.Id,
            ExpiryDateTime = DateTime.UtcNow.AddDays(7)
        };

        await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return refreshToken.Token;
    }
}
