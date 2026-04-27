using StBurger.Domain.Core.Abstractions;
using StBurger.Domain.Core.Entities;

namespace StBurger.Application.Core.Abstractions.Repositories;

public interface IUnitOfWork<TId> : IDisposable where TId : class, IEquatable<TId> 
{
    IRepository<T, TId> Repository<T>() where T : class, IAuditableEntity<TId>;

    Task<int> Commit(CancellationToken cancellationToken);

    Task Rollback();
    Task BeginTransactionAsync();
}