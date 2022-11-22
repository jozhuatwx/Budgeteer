namespace Budgeteer.SharedUI.Services;

public interface IUserService
{
    Task<List<UserResponse>?> GetUsersAsync();
    Task LoginUserAsync(LoginUserRequest request);
}
