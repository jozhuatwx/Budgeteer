using System.Net.Http.Json;
using Budgeteer.Shared.DTOs;

namespace Budgeteer.SharedUI.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(
        HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public virtual async Task<UserSessionResponse?> LoginUserAsync(LoginUserRequest request)
    {
        var results = await _httpClient.PostAsJsonAsync("/user/login", request);
        return await results.Content.ReadFromJsonAsync<UserSessionResponse?>();
    }
}
