using Microsoft.EntityFrameworkCore;

namespace Budgeteer.Server.Data;

public class BudgeteerContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Notification> Notifications => Set<Notification>();

    public BudgeteerContext(DbContextOptions<BudgeteerContext> options)
        : base(options)
    {
    }
}
