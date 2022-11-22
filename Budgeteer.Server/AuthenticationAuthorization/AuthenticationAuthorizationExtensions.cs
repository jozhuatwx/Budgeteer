using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Budgeteer.Server.AuthenticationAuthorization;

public static class AuthenticationAuthorizationExtensions
{
    public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, JwtOptions jwtOptions)
    {
        services
            .AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    ClockSkew = TimeSpan.Zero
                };
            });

        services
            .AddAuthorization();

        services
            .AddCors();

        return services;
    }

    public static IApplicationBuilder UseAuthenticationAndAuthorization(this IApplicationBuilder builder)
    {
        builder
            .UseAuthentication()
            .UseAuthorization();

        return builder;
    }

    public static RouteHandlerBuilder RequireAuthorizationWithOpenApi(this RouteHandlerBuilder builder)
    {
        builder
            .RequireAuthorization()
            .Produces(StatusCodes.Status401Unauthorized)
            .AddOpenApiAuthorizationRequirement();

        return builder;
    }
}
