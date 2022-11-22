using System.Net.Http.Json;

namespace Budgeteer.Web.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(
        HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UserResponse>?> GetUsersAsync()
    {
        var results = await _httpClient.GetAsync("/user/all");
        return await results.Content.ReadFromJsonAsync<List<UserResponse>>();
    }

    public async Task LoginUserAsync(LoginUserRequest request)
    {
        var results = await _httpClient.PostAsJsonAsync("/user/login", request);
        var response = await results.Content.ReadFromJsonAsync<UserSessionResponse?>();

        if (response != null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", response.Token);
        }
    }
}
