using Microsoft.Extensions.Logging;

namespace BudgeteerApp;

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
            .AddSingleton<ICategoriesService, CategoriesService>()
            .AddSingleton<ITransactionsService, TransactionsService>()
            .AddSingleton<IAccountsService, AccountsService>();

        return builder.Build();
    }
}

