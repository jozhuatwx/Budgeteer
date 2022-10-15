﻿using Microsoft.EntityFrameworkCore;

namespace Playground.Application.Contexts;

public class PlaygroundContext : DbContext
{
    public DbSet<User> Users { get; protected set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; protected set; } = null!;

    public PlaygroundContext(DbContextOptions options)
        : base(options)
    {
    }
}