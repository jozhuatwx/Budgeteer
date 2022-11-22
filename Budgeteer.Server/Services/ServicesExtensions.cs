namespace Budgeteer.Server.Services;

public static class ServicesExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<IUserService, UserService>()
            .AddScoped<ISessionService, SessionService>()
            .AddScoped<INotificationService, NotificationService>()
            .AddTransient<IBackgroundQueueService, BackgroundQueueService>()
            .AddHostedService<BackgroundWorkerService>();

        return services;
    }
}
