using Microsoft.EntityFrameworkCore;

namespace PlaygroundApi.Data;

public class PlaygroundContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public PlaygroundContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
