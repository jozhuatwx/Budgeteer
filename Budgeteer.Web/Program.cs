using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddBlazoredLocalStorage()
    .AddAuthorizationCore()
    .AddSingleton(sp => new HttpClient() { BaseAddress = new("https://localhost:7094") })
    .AddScoped<IUserService, UserService>()
    .AddScoped<AuthenticationStateProvider, AuthStateProvider>();

await builder.Build().RunAsync();
