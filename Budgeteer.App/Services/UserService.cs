using System.Net.Http.Json;

namespace Budgeteer.App.Services;

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
        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
        {
            var results = await _httpClient.GetAsync("/user/all");
            return await results.Content.ReadFromJsonAsync<List<UserResponse>>();
        }

        return null;
    }

    public async Task LoginUserAsync(LoginUserRequest request)
    {
        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
        {
            var results = await _httpClient.PostAsJsonAsync("/user/login", request);
            var response = await results.Content.ReadFromJsonAsync<UserSessionResponse?>();

            if (response != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", response.Token);
            }
        }
    }
}
