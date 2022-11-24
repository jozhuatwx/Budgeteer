using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Budgeteer.Server.Data;

public class BaseRepository<TEntity>
    where TEntity : BaseEntity
{
    public bool IsRelational { get; init; }

    private readonly DbSet<TEntity> _dbSet;

    public BaseRepository(BudgeteerContext context)
    {
        _dbSet = context.Set<TEntity>();
        IsRelational = context.GetService<IDatabaseCreator>() is RelationalDatabaseCreator;
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> func, bool withSoftDeleted = false, CancellationToken cancellationToken = default) =>
        await _dbSet
            .WithSoftDeleted(withSoftDeleted)
            .AnyAsync(func, cancellationToken: cancellationToken);

    public void Create(TEntity entity)
    {
        entity.CreatedDateTime = DateTime.UtcNow;
        _dbSet.Add(entity);
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> func, AutoMapper.IConfigurationProvider configurationProvider, bool track = false, bool withSoftDeleted = false, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[]? includes) =>
        await _dbSet
            .DefaultQueryOptions(track, withSoftDeleted, includes)
            .Where(func)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<TProjectedEntity?> GetAsync<TProjectedEntity>(Expression<Func<TEntity, bool>> func, AutoMapper.IConfigurationProvider configurationProvider, bool track = false, bool withSoftDeleted = false, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[]? includes) =>
        await _dbSet
            .DefaultQueryOptions(track, withSoftDeleted, includes)
            .Where(func)
            .ProjectTo<TProjectedEntity>(configurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<TEntity?> GetAsync(int id, AutoMapper.IConfigurationProvider configurationProvider, bool track = false, bool withSoftDeleted = false, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[]? includes) =>
        await GetAsync(entity => entity.Id == id, configurationProvider, track, withSoftDeleted, cancellationToken, includes);

    public async Task<TProjectedEntity?> GetAsync<TProjectedEntity>(int id, AutoMapper.IConfigurationProvider configurationProvider, bool track = false, bool withSoftDeleted = false, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[]? includes) =>
        await GetAsync<TProjectedEntity>(entity => entity.Id == id, configurationProvider, track, withSoftDeleted, cancellationToken, includes);

    public async Task<List<TEntity>> GetAllAsync(AutoMapper.IConfigurationProvider configurationProvider, bool track = false, bool withSoftDeleted = false, int? skip = null, int? take = null, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[]? includes) =>
        await _dbSet
            .DefaultListQueryOptions(track, withSoftDeleted, skip, take, includes)
            .ToListAsync(cancellationToken);

    public async Task<List<TProjectedEntity>> GetAllAsync<TProjectedEntity>(AutoMapper.IConfigurationProvider configurationProvider, bool track = false, bool withSoftDeleted = false, int? skip = null, int? take = null, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[]? includes) =>
        await _dbSet
            .DefaultListQueryOptions(track, withSoftDeleted, skip, take, includes)
            .ProjectTo<TProjectedEntity>(configurationProvider)
            .ToListAsync(cancellationToken);

    public void Update(TEntity entity)
    {
        entity.LastModifiedDateTime = DateTime.UtcNow;
        _dbSet.Update(entity);
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        Parallel.ForEach(entities, entity => entity.LastModifiedDateTime = DateTime.UtcNow);
        _dbSet.UpdateRange(entities);
    }

    public async Task DeleteAsync(TEntity entity, bool permanent = false)
    {
        if (permanent)
        {
            if (IsRelational)
                await ExecuteDeleteAsync(entity.Id);
            else
                _dbSet.Remove(entity);
        }
        else
        {
            entity.DeletedDateTime = DateTime.UtcNow;
            _dbSet.Update(entity);
        }
    }

    public void DeleteRange(IEnumerable<TEntity> entities, bool permanent = false)
    {
        if (permanent)
        {
            _dbSet.RemoveRange(entities);
        }
        else
        {
            Parallel.ForEach(entities, entity => entity.DeletedDateTime = DateTime.UtcNow);
            _dbSet.UpdateRange(entities);
        }
    }

    public async Task<int> ExecuteDeleteAsync(int id, CancellationToken cancellationToken = default) =>
        await ExecuteDeleteAsync(entity => entity.Id == id, cancellationToken);

    public async Task<int> ExecuteDeleteAsync(Expression<Func<TEntity, bool>> func, CancellationToken cancellationToken = default)
    {
        if (!IsRelational)
            throw new NotSupportedException();

        return await _dbSet
            .Where(func)
            .ExecuteDeleteAsync(default);
    }
}

file static class QueryExtensions
{
    public static IQueryable<TEntity> WithSoftDeleted<TEntity>(this IQueryable<TEntity> query, bool withSoftDeleted)
        where TEntity : BaseEntity
    {
        if (!withSoftDeleted)
            query.Where(entity => entity.DeletedDateTime == null);

        return query;
    }

    public static IQueryable<TEntity> DefaultQueryOptions<TEntity>(this IQueryable<TEntity> query, bool track, bool withSoftDeleted, params Expression<Func<TEntity, object>>[]? includes)
        where TEntity : BaseEntity
    {
        query.WithSoftDeleted(withSoftDeleted);

        if (includes != null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (!track)
            query.AsNoTracking();

        return query;
    }

    public static IQueryable<TEntity> DefaultListQueryOptions<TEntity>(this IQueryable<TEntity> query, bool track, bool withSoftDeleted, int? skip = null, int? take = null, params Expression<Func<TEntity, object>>[]? includes)
        where TEntity : BaseEntity
    {
        query = query.DefaultQueryOptions(track, withSoftDeleted, includes);

        if (skip != null)
            query.Skip((int)skip);

        if (take != null)
            query.Take((int)take);

        return query;
    }
}
