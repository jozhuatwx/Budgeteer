namespace Budgeteer.Server.Services.Interfaces;

public interface ISessionService
{
    Task<UserSessionResponse?> LoginUserAsync(LoginUserRequest request, CancellationToken cancellationToken = default);
    Task<UserSessionResponse?> RefreshUserSessionAsync(int id, RefreshUserSessionRequest request, CancellationToken cancellationToken = default);
}
