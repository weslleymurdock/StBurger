using StBurger.Domain.Menu.Entities;

namespace StBurger.Application.Order.Services;

using Order = Domain.Orders.Entities.Order;

public sealed class OrderService(IUnitOfWork<string> uow, IOrderReadOnlyRepository ro) : IOrderService
{
    public async Task<OrderResponse> AddItemAsync(string id, string itemId, CancellationToken cancellationToken)
    {
        var order = await ro.GetByIdWithItemsAsync(id, cancellationToken);

        MenuItem orderItem = uow
            .Repository<MenuItem>()
            .Entities
            .FirstOrDefault(m => m.Id == itemId) ??
            throw new KeyNotFoundException("Produto não encontrado");

        order.AddItem(orderItem);
        order.Validate();

        return OrderResponse.FromEntity(order);
    }

    public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest data, CancellationToken cancellationToken)
    {

        var order = CreateOrderRequest.ToEntity(data);

        var itemIds = data.Items.Select(x => x.Id).ToList();

        var menuItems = uow.Repository<MenuItem>()
            .Entities
            .Where(x => itemIds.Contains(x.Id))
            .ToList();

        if (menuItems.Count != itemIds.Count)
            throw new KeyNotFoundException("Um ou mais produtos não foram encontrados");

        foreach (var item in menuItems)
            order.AddItem(item);

        order.Validate();

        await uow.Repository<Order>().AddAsync(order);

        return CreateOrderResponse.ToResponse(order);

    }

    public async Task<Unit> DeleteItemAsync(string id, string itemId, CancellationToken cancellationToken)
    {

        Order order = await ro.GetByIdWithItemsAsync(id, cancellationToken) ??
            throw new KeyNotFoundException("Pedido não encontrado");

        if (!order.Items.Any(x => x.MenuItem.Id == itemId))
            throw new KeyNotFoundException("Produto não encontrado no pedido");

        order.RemoveItem(itemId);

        return Unit.Value;

    }

    public async Task<Unit> DeleteOrderAsync(string id, CancellationToken cancellationToken)
    {

        var order = uow.Repository<Order>()
            .Entities
            .FirstOrDefault<Order>(x => x.Id == id)
            ?? throw new KeyNotFoundException("Pedido não encontrado");
        await uow.Repository<Order>().DeleteAsync(order);

        return Unit.Value;

    }

    public async Task<IList<OrderResponse>> GetAsync(CancellationToken cancellationToken)
    {
        var orders = await ro.GetWithItemsAsync(cancellationToken);

        IList<OrderResponse> response = orders.Any()
            ? [.. orders.Select(e => OrderResponse.FromEntity(e))]
            : throw new KeyNotFoundException("Pedidos não encontrados");

        return response;
    }

    public async Task<OrderResponse?> GetAsync(string id, CancellationToken cancellationToken)
    {
        return OrderResponse.FromEntity(await ro.GetByIdWithItemsAsync(id, cancellationToken));
    }

    public async Task<UpdateOrderResponse> UpdateOrderAsync(UpdateOrderRequest data, CancellationToken cancellationToken)
    {
        var order = await ro.GetByIdWithItemsAsync(data.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Pedido não encontrado");

        var itemIds = data.Items.Select(x => x.Id).ToList();

        var menuItems = uow.Repository<MenuItem>()
            .Entities
            .Where(x => itemIds.Contains(x.Id))
            .ToList();

        if (menuItems.Count != itemIds.Count)
            throw new KeyNotFoundException("Um ou mais produtos do pedido não foram encontrados");

        var currentItems = order.Items;

        var currentIds = currentItems
            .Select(i => i.MenuItem.Id)
            .ToHashSet();

        var newIds = menuItems
            .Select(m => m.Id)
            .ToHashSet();

        var idsToRemove = currentIds
            .Where(id => !newIds.Contains(id))
            .ToList();

        if (idsToRemove.Count > 0)
        {
            await uow.Repository<OrderItem>()
                .DeleteAsync(x => x.OrderId == order.Id && idsToRemove.Contains(x.MenuItem.Id));

            order.Items.RemoveAll(i => idsToRemove.Contains(i.MenuItem.Id));
        }

        var toAdd = menuItems
            .Where(m => !currentIds.Contains(m.Id))
            .ToList();

        foreach (var menuItem in toAdd)
        {
            order.AddItem(menuItem);
        }

        order.Attendant = data.Attendant;
        order.Customer = data.CustomerName;

        order.Validate();

        return UpdateOrderResponse.ToResponse(order);
    }
}
