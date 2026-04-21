using StBurger.Domain.Core.Entities;

namespace StBurger.Application.Core.Abstractions.Repositories;

public interface IUnitOfWork<TId> : IDisposable where TId : class, IEquatable<TId> 
{
    IRepository<T, TId> Repository<T>() where T : AuditableEntity<TId>;

    Task<int> Commit(CancellationToken cancellationToken);

    Task Rollback();
}