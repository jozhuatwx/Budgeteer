using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Budgeteer.SharedUI.Services;

public class AuthStateProvider : IAuthStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public AuthStateProvider(
        HttpClient httpClient,
        ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var savedToken = await _localStorage.GetItemAsync<string>("authToken");

        if (string.IsNullOrWhiteSpace(savedToken))
        {
            return new AuthenticationState(new(new ClaimsIdentity()));
        }

        _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", savedToken);

        return new AuthenticationState(new(new ClaimsIdentity(ParseClaimsFromJwt(savedToken), "jwt")));
    }

    public override void MarkUserAsAuthenticated(string email)
    {
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "apiauth"));
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public override void MarkUserAsLoggedOut()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }
}
