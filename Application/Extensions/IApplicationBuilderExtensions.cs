using Microsoft.EntityFrameworkCore;

namespace Playground.Application.Extensions;

public static class IApplicationBuilderExtensions
{
    public static IApplicationBuilder SeedData(this IApplicationBuilder applicationBuilder)
    {
        using var scope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = scope.ServiceProvider
            .GetService<IDbContextFactory<PlaygroundContext>>()!
            .CreateDbContext();

        if (!context.Users.Any())
        {
            context.Users.AddRange(new User[]
            {
                new()
                {
                    Name = "Member",
                    Email = "member@mail.com",
                    HashedPassword = CryptographyUtility.HashPasswordAsync("123QWEasd").Result
                }
            });
        }

        context.SaveChanges();
        
        return applicationBuilder;
    }
}
