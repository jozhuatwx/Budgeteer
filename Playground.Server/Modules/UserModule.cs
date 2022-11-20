namespace Playground.Server.Modules;

internal static class UserModule
{
    internal static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/user");
        group.WithTags("User");

        group.MapGet("/all", GetUsersAsync)
            .Produces<ICollection<UserResponse>>();

        group.MapPost("/", CreateUserAsync)
            .Produces<UserResponse>(StatusCodes.Status201Created);

        group.MapGet("/", GetUserAsync)
            .RequireAuthorizationWithOpenApi()
            .Produces<UserResponse>();

        group.MapPut("/", UpdateUserAsync)
            .RequireAuthorizationWithOpenApi()
            .Produces<UserResponse>();

        group.MapDelete("/", DeleteUserAsync)
            .RequireAuthorizationWithOpenApi()
            .Produces<UserResponse>();

        group.MapPost("/login", LoginUserAsync)
            .Produces<UserSessionResponse>()
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPost("/refreshSession", RefreshUserSessionAsync)
            .RequireAuthorizationWithOpenApi()
            .Produces<UserSessionResponse>();

        return builder;
    }

    private static async Task<IResult> GetUsersAsync(
        IUserService service, CancellationToken cancellationToken)
    {
        return Results.Ok(await service.GetUsersAsync(cancellationToken));
    }

    private static async Task<IResult> CreateUserAsync(
        IUserService service, CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await service.CreateUserAsync(request, cancellationToken);
        return Results.Created($"/user/{user.Id}", user);
    }

    private static async Task<IResult> GetUserAsync(
        IUserService service, HttpContext context, CancellationToken cancellationToken)
    {
        if (context.User.TryGetId(out var id))
        {
            return Results.Ok(await service.GetUserAsync(id, cancellationToken));
        }
        return Results.Unauthorized();
    }

    private static async Task<IResult> UpdateUserAsync(
        IUserService service, HttpContext context, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        if (context.User.TryGetId(out var id))
        {
            return Results.Ok(await service.UpdateUserAsync(id, request, cancellationToken));
        }
        return Results.Unauthorized();
    }

    private static async Task<IResult> DeleteUserAsync(
        IUserService service, HttpContext context, CancellationToken cancellationToken)
    {
        if (context.User.TryGetId(out var id))
        {
            return Results.Ok(await service.DeleteUserAsync(id, cancellationToken));
        }
        return Results.Unauthorized();
    }

    private static async Task<IResult> LoginUserAsync(
        ISessionService service, LoginUserRequest request, CancellationToken cancellationToken)
    {
        var results = await service.LoginUserAsync(request, cancellationToken);

        if (results == null)
        {
            return Results.BadRequest();
        }

        return Results.Ok(results);
    }

    private static async Task<IResult> RefreshUserSessionAsync(
        ISessionService service, HttpContext context, RefreshUserSessionRequest request, CancellationToken cancellationToken)
    {
        if (context.User.TryGetId(out var id))
        {
            return Results.Ok(await service.RefreshUserSessionAsync(id, request, cancellationToken));
        }
        return Results.Unauthorized();
    }

    private static async Task<IResult> GetUserByIdAsync(
        IUserService service, int id, CancellationToken cancellationToken)
    {
        return Results.Ok(await service.GetUserAsync(id, cancellationToken));
    }
}
