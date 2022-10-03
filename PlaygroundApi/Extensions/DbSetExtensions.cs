using Microsoft.EntityFrameworkCore;

namespace PlaygroundApi.Extensions;

public static class DbSetExtensions
{
    public static async Task<List<T>> GetAllAsync<T>(this DbSet<T> dbSet, bool track = false) where T : BaseEntity
    {
        if (track)
        {
            return await dbSet
                .Where(e => e.DeletedDateTime == null)
                .ToListAsync();
        }

        return await dbSet
            .AsNoTracking()
            .Where(e => e.DeletedDateTime == null)
            .ToListAsync();
    }

    public static async Task<T?> GetAsync<T>(this DbSet<T> dbSet, int id, bool track = false) where T : BaseEntity
    {
        if (track)
        {
            return await dbSet
                .FirstOrDefaultAsync(e => e.Id == id && e.DeletedDateTime == null);
        }

        return await dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id && e.DeletedDateTime == null);
    }

    public static async Task<bool> CreateAsync<T>(this DbSet<T> dbSet, T e) where T : BaseEntity
    {
        e.CreatedDateTime = DateTime.UtcNow;
        await dbSet.AddAsync(e);

        return true;
    }

    public static bool Update<T>(this DbSet<T> dbSet, T e) where T : BaseEntity
    {
        e.LastModifiedDateTime = DateTime.UtcNow;
        dbSet.Update(e);

        return true;
    }

    public static bool Delete<T>(this DbSet<T> dbSet, T e) where T : BaseEntity
    {
        e.DeletedDateTime = DateTime.UtcNow;
        dbSet.Update(e);

        return true;
    }
}
