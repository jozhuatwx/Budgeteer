using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Playground.Server.Extensions;

public static class OpenApiExtensions
{
    public static RouteHandlerBuilder RequireAuthorizationWithOpenApi(this RouteHandlerBuilder builder)
    {
        builder
            .RequireAuthorization()
            .Produces(StatusCodes.Status401Unauthorized)
            .AddOpenApiAuthorizationRequirement();

        return builder;
    }

    private static IEndpointConventionBuilder AddOpenApiAuthorizationRequirement(this IEndpointConventionBuilder builder)
    {
        var scheme = new OpenApiSecurityScheme()
        {
            Type = SecuritySchemeType.Http,
            Name = JwtBearerDefaults.AuthenticationScheme,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Reference = new()
            {
                Type = ReferenceType.SecurityScheme,
                Id = JwtBearerDefaults.AuthenticationScheme
            }
        };

        return builder.WithOpenApi((operation) => new(operation)
        {
            Security =
            {
                new()
                {
                    [scheme] = new List<string>()
                }
            }
        });
    }
}
