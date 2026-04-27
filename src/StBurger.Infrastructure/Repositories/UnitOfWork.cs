using Microsoft.EntityFrameworkCore.Storage;
using StBurger.Application.Core.Abstractions.Repositories;
using StBurger.Domain.Core.Entities;
using StBurger.Infrastructure.Persistence;
using System.Collections;

namespace StBurger.Infrastructure.Repositories;

public class UnitOfWork<TId> : IUnitOfWork<TId> where TId : class, IEquatable<TId>
{
    private readonly StBurgerDbContext _dbContext;
    private bool disposed;
    private Hashtable _repositories;
    private IDbContextTransaction? _transaction;
    public UnitOfWork(StBurgerDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _repositories ??= [];

    }
    public async Task BeginTransactionAsync()
    {
        _transaction ??= await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task<int> Commit(CancellationToken cancellationToken)
    {
        var result = await _dbContext.SaveChangesAsync(cancellationToken);

        if (_transaction is not null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        return result;
    }

    public IRepository<TEntity, TId> Repository<TEntity>() where TEntity : class, IAuditableEntity<TId>
    {
        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(Repository<,>);

            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity), typeof(TId)), _dbContext);

            _repositories.Add(type, repositoryInstance);
        }

        return (IRepository<TEntity, TId>)_repositories[type]!;
    }
    public async Task Rollback()
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        _dbContext.ChangeTracker.Clear();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                //dispose managed resources
                _dbContext.Dispose();
            }
        }
        //dispose unmanaged resources
        disposed = true;
    }
}
