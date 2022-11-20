using Microsoft.EntityFrameworkCore;

namespace Playground.Server.Extensions;

public static class DbExtensions
{
    public static void Create<TEntity>(this DbSet<TEntity> dbSet,
        TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        entity.CreatedDateTime = DateTime.UtcNow;
        dbSet.Add(entity);
    }

    public static void Update<TEntity>(this DbSet<TEntity> dbSet,
        TEntity entity)
        where TEntity : BaseEntity
    {
        entity.LastModifiedDateTime = DateTime.UtcNow;
        dbSet.Update(entity);
    }

    public static void UpdateRange<TEntity>(this DbSet<TEntity> dbSet,
        IEnumerable<TEntity> entities)
        where TEntity : BaseEntity
    {
        Parallel.ForEach(entities, (entity) => dbSet.Update(entity));
    }

    public static void Delete<TEntity>(this DbSet<TEntity> dbSet,
        TEntity entity, bool permanent = false)
        where TEntity : BaseEntity
    {
        if (permanent)
        {
            dbSet.Remove(entity);
        }
        else
        {
            entity.DeletedDateTime = DateTime.UtcNow;
            dbSet.Update(entity);
        }
    }

    public static void DeleteRange<TEntity>(this DbSet<TEntity> dbSet,
        IEnumerable<TEntity> entities, bool permanent = false)
        where TEntity : BaseEntity
    {
        if (permanent)
        {
            dbSet.RemoveRange(entities);
        }
        else
        {
            Parallel.ForEach(entities, (entity) => dbSet.Delete(entity));
        }
    }
}
