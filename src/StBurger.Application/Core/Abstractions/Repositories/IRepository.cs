using StBurger.Domain.Core.Abstractions;
using System.Linq.Expressions;

namespace StBurger.Application.Core.Abstractions.Repositories;

public interface IRepository<T, in TId> where T : class, IEntity<TId>
{
    IQueryable<T> Entities { get; }

    Task<T> AddAsync(T entity);
    Task DeleteAsync(T entity);
    Task DeleteAsync(Expression<Func<T, bool>> predicate);
    Task<T> GetByIdAsync(TId id);
    Task<IList<T>> GetAllAsync();
    Task<IList<T>> GetPagedResponseAsync(int pageNumber, int pageSize);
    Task UpdateAsync(T entity);
}
