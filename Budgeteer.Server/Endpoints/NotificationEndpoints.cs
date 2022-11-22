namespace Budgeteer.Server.Endpoints;

public static class NotificationEndpoints
{
    public static IEndpointRouteBuilder MapNotificationEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/notification");
        group.WithTags("Notification");

        group.MapGet("/all", GetNotificationsAsync)
            .RequireAuthorizationWithOpenApi()
            .Produces<ICollection<NotificationResponse>>();

        group.MapPut("/{id:int}", UpdateNotificationAsync)
            .RequireAuthorizationWithOpenApi()
            .Produces(StatusCodes.Status200OK);

        return builder;
    }

    private static async Task<IResult> GetNotificationsAsync(INotificationService service, HttpContext context, CancellationToken cancellationToken)
    {
        if (context.User.TryGetId(out var userId))
        {
            return Results.Ok(await service.GetNotificationsAsync(userId, cancellationToken));
        }
        return Results.Unauthorized();
    }

    private static async Task<IResult> UpdateNotificationAsync(INotificationService service, HttpContext context, int id, CancellationToken cancellationToken)
    {
        await service.ReadNotificationAsync(id, cancellationToken);
        return Results.Ok();
    }
}
