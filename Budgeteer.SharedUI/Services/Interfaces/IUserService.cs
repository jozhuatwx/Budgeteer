using System.Net.Http.Json;
using Budgeteer.Shared.DTOs;

namespace Budgeteer.SharedUI.Services;

public interface IUserService
{
    Task<UserSessionResponse?> LoginUserAsync(LoginUserRequest request);
}
