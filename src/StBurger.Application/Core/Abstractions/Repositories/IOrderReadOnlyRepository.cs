namespace StBurger.Application.Core.Abstractions.Repositories;

public interface IOrderReadOnlyRepository
{
    Task<Domain.Orders.Entities.Order> GetByIdWithItemsAsync(string id, CancellationToken cancellationToken);
    Task<IList<Domain.Orders.Entities.Order>> GetWithItemsAsync(CancellationToken cancellationToken);
}
