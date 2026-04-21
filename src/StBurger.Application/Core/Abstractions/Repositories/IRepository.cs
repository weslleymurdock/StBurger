using StBurger.Domain.Core.Abstractions;

namespace StBurger.Application.Core.Abstractions.Repositories;

public interface IRepository<T, in TId> where T : class, IEntity<TId>
{
    IQueryable<T> Entities { get; }

    Task<T> GetByIdAsync(TId id);

    Task<IList<T>> GetAllAsync();

    Task<IList<T>> GetPagedResponseAsync(int pageNumber, int pageSize);

    Task<T> AddAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(T entity);
}
