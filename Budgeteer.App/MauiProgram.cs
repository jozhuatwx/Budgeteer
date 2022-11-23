using Microsoft.Extensions.Logging;

namespace Budgeteer.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>();

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services
            .AddSingleton(sp => new HttpClient() { BaseAddress = new("https://localhost:7094") })
            .AddScoped<IUserService, UserService>();

        return builder.Build();
    }
}
