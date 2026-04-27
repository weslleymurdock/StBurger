using StBurger.Application.Core.Abstractions.Repositories;
using StBurger.Domain.Core.Abstractions;
using System.Linq.Expressions;

namespace StBurger.UnitTests.Application.Menu.Services;

class FakeRepository<T> : IRepository<T, string> where T : class, IAuditableEntity<string>
{
    public Dictionary<string, T> Store { get; } = new();
    public List<T> Added { get; } = [];
    public List<string> Deleted { get; } = [];

    public IQueryable<T> Entities => Store.Values.AsQueryable();

    public Task<T> AddAsync(T entity)
    {
        Store[entity.Id] = entity;
        Added.Add(entity);
        return Task.FromResult(entity);
    }

    public Task DeleteAsync(T entity)
    {
        Store.Remove(entity.Id);
        Deleted.Add(entity.Id);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Expression<Func<T, bool>> predicate)
    {
        var toDelete = Store.Values.AsQueryable().Where(predicate).ToList();
        foreach (var entity in toDelete)
        {
            Store.Remove(entity.Id);
            Deleted.Add(entity.Id);
        }
        return Task.CompletedTask;
    }

    public Task<IList<T>> GetAllAsync()
        => Task.FromResult((IList<T>)Store.Values.ToList());

    public Task<T> GetByIdAsync(string id)
    {
        if (!Store.TryGetValue(id, out var entity))
            throw new KeyNotFoundException();

        return Task.FromResult(entity);
    }

    public Task<IList<T>> GetPagedResponseAsync(int pageNumber, int pageSize)
        => Task.FromResult<IList<T>>([]!);

    public Task UpdateAsync(T entity)
    {
        Store[entity.Id] = entity;
        return Task.CompletedTask;
    }
}
