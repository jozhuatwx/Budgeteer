namespace Budgeteer.Server.Services.Interfaces;

public interface IUserService
{
    Task<ICollection<UserResponse>> GetUsersAsync(CancellationToken cancellationToken = default);
    Task<UserResponse> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
    Task<UserResponse?> GetUserAsync(int id, CancellationToken cancellationToken = default);
    Task<UserResponse?> UpdateUserAsync(int id, UpdateUserRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteUserAsync(int id, CancellationToken cancellationToken = default);
}
