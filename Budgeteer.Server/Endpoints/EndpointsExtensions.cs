namespace Budgeteer.Server.Endpoints;

public static class EndpointsExtensions
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        builder
            .MapUserEndpoints()
            .MapNotificationEndpoints()
            .MapHub<NotificationHub>("/notification");

        return builder;
    }
}
