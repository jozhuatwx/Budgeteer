using Microsoft.EntityFrameworkCore;

namespace Budgeteer.Server.Data;

public static class DataExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, DatabaseOptions databaseOptions)
    {
        services
            .AddPooledDbContextFactory<BudgeteerContext>(options => options.UseInMemoryDatabase(databaseOptions.Name))
            .AddScoped(provider => provider.GetRequiredService<IDbContextFactory<BudgeteerContext>>().CreateDbContext())
            .AddScoped<UnitOfWork>();

        return services;
    }

    public static async Task<IApplicationBuilder> UseSeedDataAsync(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();

        using var context = await scope.ServiceProvider
            .GetRequiredService<IDbContextFactory<BudgeteerContext>>()
            .CreateDbContextAsync();

        if (!context.Users.Any())
        {
            context.Users.AddRange(new User[]
            {
                new()
                {
                    Name = "Member",
                    Email = "member@mail.com",
                    CreatedDateTime = DateTime.UtcNow,
                    HashedPassword = await CryptographyUtility.HashPasswordAsync("123QWEasd")
                }
            });

            await context.SaveChangesAsync();
        }

        return builder;
    }
}
