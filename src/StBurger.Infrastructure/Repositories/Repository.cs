using StBurger.Application.Core.Abstractions.Repositories;
using StBurger.Domain.Core.Entities;
using StBurger.Infrastructure.Persistence;
namespace StBurger.Infrastructure.Repositories;
using Domain.Core.Exceptions;
using System.Linq.Expressions;

public class Repository<T, TId>(StBurgerDbContext dbContext) : IRepository<T, TId> where T : AuditableEntity<TId>, IAuditableEntity<TId>
    where TId : class, IEquatable<TId>
{
    public IQueryable<T> Entities => dbContext.Set<T>();

    public async Task<T> AddAsync(T entity)
    {
        await dbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        _ = await dbContext.Set<T>().Where(x => x.Id == entity.Id).ExecuteDeleteAsync<T>();
    }

    public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
    {
        _ = await dbContext.Set<T>().Where(predicate).ExecuteDeleteAsync<T>();
    }

    public async Task<IList<T>> GetAllAsync()
    {
        return await dbContext
            .Set<T>()
            .ToListAsync();
    }

    public async Task<T> GetByIdAsync(TId id)
    {
        return await dbContext.Set<T>().FindAsync(id) 
            ?? throw new EntityNotFoundException($"Entity not found", "NOT_FOUND");
    }

    public async Task<IList<T>> GetPagedResponseAsync(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0)
        {
            throw new ArgumentException("Page number must be greater than zero.", nameof(pageNumber));
        }
        if (pageSize <= 0)
        {
            throw new ArgumentException("Page size must be greater than zero.", nameof(pageSize));
        }

        var toSkip = (pageNumber - 1) * pageSize;

        if (dbContext.Set<T>().Count() <= toSkip)
        {
            throw new ArgumentException("Page number exceeds total pages.", nameof(pageNumber));
        }
        return await dbContext
            .Set<T>()
            .Skip(toSkip)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        var set = dbContext.Set<T>();
 
        var trackedEntity = dbContext
            .ChangeTracker
            .Entries<T>()
            .FirstOrDefault(e => e.Entity.Id.Equals(entity.Id));

        // if is already tracked
        if (trackedEntity is not null)
        {
            trackedEntity.CurrentValues.SetValues(entity);
            return;
        }

        // not tracked, then search
        var existingEntity = await set.FindAsync(entity.Id);

        if (existingEntity is not null)
        {
            // found. then update
            dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
        }
        else
        {
            throw new EntityNotFoundException("Entity not found", "NOT_FOUND");
        }
    }
}
