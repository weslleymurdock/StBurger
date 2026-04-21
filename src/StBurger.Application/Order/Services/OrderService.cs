namespace StBurger.Application.Order.Services;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using StBurger.Domain.Menu.Entities;
using Order = Domain.Orders.Entities.Order;

public sealed class OrderService(IRepository<Order, string> orderRepository, IRepository<OrderItem, string> orderItemRepository, IRepository<MenuItem, string> menuRepository, IUnitOfWork<string> uow, ILogger<OrderService> logger) : IOrderService
{
    public async Task<OrderResponse> AddItemAsync(string id, string itemId, CancellationToken cancellationToken)
    {
        try
        {
            Order order = await orderRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Order not found");
            MenuItem orderItem = await menuRepository.GetByIdAsync(itemId) ?? throw new KeyNotFoundException("Menu item not found");
            order.AddItem(orderItem);
            await orderRepository.UpdateAsync(order);
            await uow.Commit(cancellationToken);
            return OrderResponse.FromEntity(order);

        }
        catch (Exception e)
        {
            await uow.Rollback();
            logger.LogWarning(e, "Failed to add item with ID {ItemId} to order with ID {OrderId}. Error: {Message}", itemId, id, e.Message);
            throw;
        }
    }

    public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest data, CancellationToken cancellationToken)
    {
        try
        {
            Order order = CreateOrderRequest.ToEntity(data); 
            var result = await orderRepository.AddAsync(order);
            await uow.Commit(cancellationToken);
            return CreateOrderResponse.ToResponse(result);
        }
        catch (Exception e)
        {
            await uow.Rollback();
            logger.LogWarning(e, 
                "Failed to create order for customer {CustomerName} attended by {AttendantName}. Error: {Message}", 
                data.CustomerName, data.AttendantName, e.Message);
            throw;
        }
    }

    public async Task<Unit> DeleteItemAsync(string id, string itemId, CancellationToken cancellationToken)
    {
        try
        {
            var order = await orderRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Order not found");
            var orderItem = await orderItemRepository.GetByIdAsync(itemId) ?? throw new KeyNotFoundException("Order item not found");
            await orderItemRepository.DeleteAsync(orderItem);
            await uow.Commit(cancellationToken);
            return Unit.Value;
        }
        catch (Exception e)
        {
            await uow.Rollback();
            logger.LogWarning(e, "Failed to delete order item with id {ItemId} at order with id {OrderId}. Error: {Message}", itemId, id, e.Message);
            throw;
        }
    }

    public async Task<Unit> DeleteOrderAsync(string id, CancellationToken cancellationToken)
    {
        try
        {
            var order = await orderRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Order not found");
            await orderRepository.DeleteAsync(order);
            await uow.Commit(cancellationToken);
            return Unit.Value;
        }
        catch (Exception e)
        {
            await uow.Rollback();
            logger.LogWarning(e, "Failed to delete order with ID {OrderId}. Error: {Message}", id, e.Message);
            throw;
        }
    }

    public async Task<IList<OrderResponse>> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            return [.. (await orderRepository.GetAllAsync()).Select(e => OrderResponse.FromEntity(e))];
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "Failed to retrieve orders. Error: {Message}", e.Message);
            throw;
        }
    }

    public async Task<OrderResponse?> GetAsync(string id, CancellationToken cancellationToken)
    {
        try
        {
            return OrderResponse.FromEntity(await orderRepository.GetByIdAsync(id));
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "Failed to retrieve order by id '{Id}'. Error: {Message}", id, e.Message);
            throw;
        }
    }

    public async Task<UpdateOrderResponse> UpdateOrderAsync(UpdateOrderRequest data, CancellationToken cancellationToken)
    {
        try
        {
            Order order = await orderRepository.GetByIdAsync(data.Id) ?? throw new KeyNotFoundException("Order not found");
            List<MenuItem> items = [];
            foreach (var item in data.Items)
            {
                var menuItem = await menuRepository.GetByIdAsync(item.Id) ?? throw new KeyNotFoundException("Menu item not found");
                items.Add(menuItem);
            }

            order.Update(data.Attendant, data.CustomerName, items);
            await orderRepository.UpdateAsync(order);
            return UpdateOrderResponse.ToResponse(order);
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "Failed to update order with ID {OrderId}. Error: {Message}", data.Id, e.Message);
            throw;
        }
    }
}
