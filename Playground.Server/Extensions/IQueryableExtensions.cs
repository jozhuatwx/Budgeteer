using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Playground.Server.Extensions;

public static class IQueryableExtensions
{
    public static async Task<List<TEntity>> GetAllAsync<TEntity>(this IQueryable<TEntity> dbSet,
        bool track = false, bool withSoftDeleted = false, int? skip = null, int? take = null, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        return await dbSet
            .DefaultQueryOptions(track, withSoftDeleted, skip, take)
            .ToListAsync(cancellationToken);
    }

    public static async Task<List<TProjectedEntity>> GetAllAsync<TEntity, TProjectedEntity>(this IQueryable<TEntity> dbSet,
        AutoMapper.IConfigurationProvider configurationProvider, bool track = false, bool withSoftDeleted = false, int? skip = null, int? take = null, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        return await dbSet
            .DefaultQueryOptions(track, withSoftDeleted, skip, take)
            .ProjectTo<TProjectedEntity>(configurationProvider)
            .ToListAsync(cancellationToken);
    }

    public static async Task<TEntity?> GetByIdAsync<TEntity>(this IQueryable<TEntity> dbSet,
        int id, bool track = false, bool withSoftDeleted = false, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        return await dbSet.GetAsync((entity) => entity.Id == id, track, withSoftDeleted, cancellationToken);
    }

    public static async Task<TProjectedEntity?> GetByIdAsync<TEntity, TProjectedEntity>(this IQueryable<TEntity> dbSet,
        int id, AutoMapper.IConfigurationProvider configurationProvider, bool track = false, bool withSoftDeleted = false, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        return await dbSet.GetAsync<TEntity, TProjectedEntity>((entity) => entity.Id == id, configurationProvider, track, withSoftDeleted, cancellationToken);
    }

    public static async Task<TEntity?> GetAsync<TEntity>(this IQueryable<TEntity> dbSet,
        Expression<Func<TEntity, bool>> func, bool track = false, bool withSoftDeleted = false, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        return await dbSet
            .DefaultQueryOptions(track, withSoftDeleted)
            .FirstOrDefaultAsync(func, cancellationToken);
    }

    public static async Task<TProjectedEntity?> GetAsync<TEntity, TProjectedEntity>(this IQueryable<TEntity> dbSet,
        Expression<Func<TEntity, bool>> func, AutoMapper.IConfigurationProvider configurationProvider, bool track = false, bool withSoftDeleted = false, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        return await dbSet
            .DefaultQueryOptions(track, withSoftDeleted)
            .Where(func)
            .ProjectTo<TProjectedEntity>(configurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public static IQueryable<TEntity> DefaultQueryOptions<TEntity>(this IQueryable<TEntity> dbSet,
        bool track = false, bool withSoftDeleted = false, int? skip = null, int? take = null)
        where TEntity : BaseEntity
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

        if (skip != null)
        {
            query.Skip((int)skip);
        }

        if (take != null)
        {
            query.Take((int)take);
        }

        return query;
    }

    private static IQueryable<TEntity> WithoutSoftDeleted<TEntity>(this IQueryable<TEntity> query)
        where TEntity : BaseEntity
    {
        return query.Where((entity) => entity.DeletedDateTime == null);
    }
}
