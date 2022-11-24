namespace Budgeteer.SharedUI.Services.Interfaces;

public interface IUserService
{
    Task<List<UserResponse>?> GetUsersAsync(CancellationToken cancellationToken = default);
    Task<UserResponse?> GetUserAsync(CancellationToken cancellationToken = default);
    Task<UserResponse?> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToke = default);
    Task DeleteUserAsync(CancellationToken cancellationToken = default);
    Task<bool> LoginUserAsync(LoginUserRequest request, CancellationToken cancellationToken = default);
    Task<bool> RefreshUserSessionAsync(RefreshUserSessionRequest request, CancellationToken cancellationToken = default);
    Task LogoutUserAsync(CancellationToken cancellationToken = default);
}
