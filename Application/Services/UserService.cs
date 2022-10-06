using AutoMapper;
using System.Security.Claims;

namespace Playground.Application.Services;

public class UserService
{
    private readonly PlaygroundContext _context;
    private readonly IMapper _mapper;

    public UserService(
        PlaygroundContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ICollection<UserResponse>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        return (await _context.Users.GetAllAsync(cancellationToken: cancellationToken))
            .Select(_mapper.Map<UserResponse>)
            .ToList();
    }

    public async Task<UserResponse> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = new User()
        {
            Name = request.Name,
            Email = request.Email.ToLower(),
            HashedPassword = await CryptographyUtility.HashPasswordAsync(request.Password, cancellationToken)
        };

        await _context.Users.CreateAsync(user, cancellationToken: cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse?> GetUserAsync(int id, CancellationToken cancellationToken = default)
    {
        return _mapper.Map<UserResponse>(await _context.Users.GetByIdAsync(id, cancellationToken: cancellationToken));
    }

    public Task<UserResponse?> GetUserAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            if (!claimsPrincipal.TryGetUserId(out var id)
                || !claimsPrincipal.TryGetUserName(out var name)
                || !claimsPrincipal.TryGetEmail(out var email))
            {
                return null;
            }
            return new UserResponse(id, name, email);
        }, cancellationToken);
    }

    public async Task<UserResponse?> UpdateUserAsync(int id, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.GetByIdAsync(id, track: true, cancellationToken: cancellationToken);

        if (user == null)
        {
            return null;
        }

        user.Name = request.Name;
        user.Email = request.Email.ToLower();

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse?> DeleteUserAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.GetByIdAsync(id, track: true, cancellationToken: cancellationToken);

        if (user == null)
        {
            return null;
        }

        _context.Users.Delete(user);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserResponse>(user);
    }
}
