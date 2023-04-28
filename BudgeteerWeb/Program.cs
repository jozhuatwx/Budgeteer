using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services
    .AddSingleton<ICategoriesService, CategoriesService>()
    .AddSingleton<ITransactionsService, TransactionsService>()
    .AddSingleton<IAccountsService, AccountsService>();

await builder.Build().RunAsync();

