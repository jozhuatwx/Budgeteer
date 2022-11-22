namespace Budgeteer.Server.Data;

public class UnitOfWork : IAsyncDisposable
{
    public BaseRepository<User> Users { get; init; }
    public BaseRepository<RefreshToken> RefreshTokens { get; init; }
    public BaseRepository<Notification> Notifications { get; init; }

    private readonly BudgeteerContext _context;

    public UnitOfWork(BudgeteerContext context)
    {
        _context = context;
        Users = new(_context);
        RefreshTokens = new(_context);
        Notifications = new(_context);
    }

    public Task<int> SaveAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return _context.DisposeAsync();
    }
}
