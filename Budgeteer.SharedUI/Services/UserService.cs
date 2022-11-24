using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Budgeteer.SharedUI.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authState;

    public UserService(
        HttpClient httpClient,
        ILocalStorageService localStorage,
        AuthenticationStateProvider authState)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _authState = authState;
    }

    public async Task<List<UserResponse>?> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        var results = await _httpClient.GetAsync("/user/all", cancellationToken);
        return await results.Content.ReadFromJsonAsync<List<UserResponse>>(cancellationToken: cancellationToken);
    }

    public Task<UserResponse?> GetUserAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<UserResponse?> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToke = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> LoginUserAsync(LoginUserRequest request, CancellationToken cancellationToken = default)
    {
        var results = await _httpClient.PostAsJsonAsync("/user/login", request, cancellationToken: cancellationToken);
        var response = await results.Content.ReadFromJsonAsync<UserSessionResponse?>(cancellationToken: cancellationToken);

        if (response == null)
            return false;

        await _localStorage.SetItemAsync("authToken", response.Token, cancellationToken);
        ((AuthStateProvider) _authState).MarkUserAsAuthenticated(request.Email);
        _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", response.Token);

        return true;
    }

    public Task<bool> RefreshUserSessionAsync(RefreshUserSessionRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task LogoutUserAsync(CancellationToken cancellationToken = default)
    {
        await _localStorage.RemoveItemAsync("authToken", cancellationToken);
        ((AuthStateProvider)_authState).MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}
