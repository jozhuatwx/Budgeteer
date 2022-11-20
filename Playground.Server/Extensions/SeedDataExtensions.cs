﻿using Microsoft.EntityFrameworkCore;

namespace Playground.Server.Extensions;

public static class SeedDataExtensions
{
    public static IApplicationBuilder SeedData(this IApplicationBuilder applicationBuilder)
    {
        using var scope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = scope.ServiceProvider
            .GetRequiredService<IDbContextFactory<PlaygroundContext>>()
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