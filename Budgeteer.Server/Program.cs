var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Configure<BudgeteerOptions>(builder.Configuration);
var budgeteerOptions = builder.Configuration.Get<BudgeteerOptions>()!;

builder.Services
    .AddLogProviders()
    .AddAuthenticationAndAuthorization(budgeteerOptions.Jwt)
    .AddDatabase(budgeteerOptions.Database)
    .AddMappings()
    .AddServices()
    .AddOpenApi()
    .AddOpenTelemetry(builder.Environment.ApplicationName)
    .AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
    await app.UseSeedDataAsync();
}

app
    .UseExceptionLogging(app.Environment.IsDevelopment())
    .UseAuthenticationAndAuthorization()
    .UseHttpsRedirection();

app.MapEndpoints();

app.Run();
