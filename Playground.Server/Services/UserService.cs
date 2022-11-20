using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Playground.Server.Services;

public class UserService : IUserService
{
    private readonly PlaygroundContext _context;
    private readonly INotificationService _notificationService;
    private readonly IMapper _mapper;

    public UserService(
        IDbContextFactory<PlaygroundContext> contextFactory,
        INotificationService notificationService,
        IMapper mapper)
    {
        _context = contextFactory.CreateDbContext();
        _notificationService = notificationService;
        _mapper = mapper;
    }

    public async Task<ICollection<UserResponse>> GetUsersAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .GetAllAsync<User, UserResponse>(_mapper.ConfigurationProvider, cancellationToken: cancellationToken);
    }

    public async Task<UserResponse> CreateUserAsync(
        CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = new User()
        {
            Name = request.Name,
            Email = request.Email.ToLower(),
            HashedPassword = await CryptographyUtility.HashPasswordAsync(request.Password, cancellationToken)
        };

        _context.Users.Create(user, cancellationToken: cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse?> GetUserAsync(
        int id, CancellationToken cancellationToken = default)
    {
        return await _context.Users.GetByIdAsync<User, UserResponse>(id, _mapper.ConfigurationProvider, cancellationToken: cancellationToken);
    }

    public async Task<UserResponse?> UpdateUserAsync(
        int id, UpdateUserRequest request, CancellationToken cancellationToken = default)
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

        await _notificationService.SendIndividualAsync(id, "Updated user!", cancellationToken);

        return _mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse?> DeleteUserAsync(
        int id, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.GetByIdAsync(id, track: true, cancellationToken: cancellationToken);

        if (user == null)
        {
            return null;
        }

        _context.Users.Delete(user);
        await _context.SaveChangesAsync(cancellationToken);

        await _notificationService.SendIndividualAsync(id, "Deleted user!", cancellationToken);

        return _mapper.Map<UserResponse>(user);
    }
}
