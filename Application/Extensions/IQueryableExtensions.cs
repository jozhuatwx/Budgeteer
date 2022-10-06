using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Playground.Application.Extensions;

public static class IQueryableExtensions
{
    public static async Task<List<TEntity>> GetAllAsync<TEntity>(this IQueryable<TEntity> dbSet,
        bool track = false, bool withSoftDeleted = false, CancellationToken cancellationToken = default) where TEntity : BaseEntity
    {
        return await dbSet
            .DefaultQueryOptions(track, withSoftDeleted)
            .ToListAsync(cancellationToken);
    }

    public static async Task<TEntity?> GetByIdAsync<TEntity>(this IQueryable<TEntity> dbSet,
        int id, bool track = false, bool withSoftDeleted = false, CancellationToken cancellationToken = default) where TEntity : BaseEntity
    {
        return await dbSet.GetAsync(entity => entity.Id == id, track, withSoftDeleted, cancellationToken);
    }

    public static async Task<TEntity?> GetAsync<TEntity>(this IQueryable<TEntity> dbSet,
        Expression<Func<TEntity, bool>> func, bool track = false, bool withSoftDeleted = false, CancellationToken cancellationToken = default) where TEntity : BaseEntity
    {
        return await dbSet
            .DefaultQueryOptions(track, withSoftDeleted)
            .FirstOrDefaultAsync(func, cancellationToken);
    }

    public static IQueryable<TEntity> DefaultQueryOptions<TEntity>(this IQueryable<TEntity> dbSet,
        bool track = false, bool withSoftDeleted = false) where TEntity : BaseEntity
    {
        var query = dbSet;

        if (!track)
        {
            query.AsNoTracking();
        }

        if (!withSoftDeleted)
        {
            query.WithoutSoftDeleted();
        }

        return query;
    }

    private static IQueryable<TEntity> WithoutSoftDeleted<TEntity>(this IQueryable<TEntity> query) where TEntity : BaseEntity
    {
        return query.Where(entity => entity.DeletedDateTime == null);
    }
}
