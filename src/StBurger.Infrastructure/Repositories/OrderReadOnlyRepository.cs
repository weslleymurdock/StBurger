using StBurger.Application.Core.Abstractions.Repositories;

namespace StBurger.Infrastructure.Repositories;

public sealed class OrderReadOnlyRepository(IUnitOfWork<string> uow) : IOrderReadOnlyRepository
{
    public async Task<Order> GetByIdWithItemsAsync(string id, CancellationToken cancellationToken)
    {
        var order = await uow
            .Repository<Order>()
            .Entities
            .Where(x => x.Id == id)
            .Include(x => x.Items)
            .ThenInclude(x => x.MenuItem)
            .FirstOrDefaultAsync(cancellationToken) ??
            throw new KeyNotFoundException("Pedido não encontrado");
        return order;
    }

    public async Task<IList<Order>> GetWithItemsAsync(CancellationToken cancellationToken)
    {
        return await uow
            .Repository<Order>()
            .Entities
            .Include(x => x.Items)
            .ThenInclude(y => y.MenuItem)
            .ToListAsync(cancellationToken);
    }
}
