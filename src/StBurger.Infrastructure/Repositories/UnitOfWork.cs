using StBurger.Application.Core.Abstractions.Repositories;
using StBurger.Domain.Core.Entities;
using StBurger.Infrastructure.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StBurger.Infrastructure.Repositories
{
    public class UnitOfWork<TId> : IUnitOfWork<TId> where TId : class, IEquatable<TId>
    {
        private readonly StBurgerDbContext _dbContext;
        private bool disposed;
        private Hashtable _repositories;

        public UnitOfWork(StBurgerDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _repositories ??= [];

        }

        public IRepository<TEntity, TId> Repository<TEntity>() where TEntity : AuditableEntity<TId>
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

        public async Task<int> Commit(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task Rollback()
        {
            _dbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            return Task.CompletedTask;
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
}
