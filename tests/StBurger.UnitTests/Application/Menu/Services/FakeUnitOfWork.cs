using StBurger.Application.Core.Abstractions.Repositories;
using StBurger.Domain.Core.Abstractions;
using StBurger.Domain.Core.Entities;
using StBurger.Domain.Menu.Entities;

namespace StBurger.UnitTests.Application.Menu.Services;

class FakeUnitOfWork : IUnitOfWork<string>
{
    public FakeRepository<Sandwich> SandwichRepo { get; } = new();
    public FakeRepository<Drink> DrinkRepo { get; } = new();
    public FakeRepository<Side> SideRepo { get; } = new();
    public FakeRepository<MenuItem> MenuRepo { get; } = new();

    public IRepository<TEntity, string> Repository<TEntity>() 
        where TEntity : class, 
        IAuditableEntity<string>
    {
        if (typeof(TEntity) == typeof(Sandwich)) return (IRepository<TEntity, string>)SandwichRepo;
        if (typeof(TEntity) == typeof(Drink)) return (IRepository<TEntity, string>)DrinkRepo;
        if (typeof(TEntity) == typeof(Side)) return (IRepository<TEntity, string>)SideRepo;
        if (typeof(TEntity) == typeof(MenuItem)) return (IRepository<TEntity, string>)MenuRepo;

        throw new NotSupportedException();
    }

    public Task BeginTransactionAsync() => Task.CompletedTask;
    public Task<int> Commit(CancellationToken cancellationToken) => Task.FromResult(0);
    public Task Rollback() => Task.CompletedTask;
    public void Dispose() { }

}