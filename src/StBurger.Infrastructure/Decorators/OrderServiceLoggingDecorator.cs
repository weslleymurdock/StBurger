using StBurger.Application.Core.Abstractions.Services;
using StBurger.Application.Order.Requests;
using StBurger.Application.Order.Responses;

namespace StBurger.Infrastructure.Decorators;

public sealed class OrderServiceLoggingDecorator(
    IOrderService inner,
    ILogger<OrderServiceLoggingDecorator> logger)
    : IOrderService
{
    public async Task<OrderResponse> AddItemAsync(string id, string itemId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting OrderService.AddItemAsync");    
        var result = await inner.AddItemAsync(id, itemId, cancellationToken);
        logger.LogInformation("End OrderService.AddItemAsync");    
        return result;
    }

    public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest data, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting OrderService.CreateOrderAsync");
        var result = await inner.CreateOrderAsync(data, cancellationToken);
        logger.LogInformation("End OrderService.CreateOrderAsync");
        return result;
    }

    public async Task<Unit> DeleteItemAsync(string id, string itemId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting OrderService.DeleteItemAsync");
        var result = await inner.DeleteItemAsync(id, itemId, cancellationToken);
        logger.LogInformation("End OrderService.DeleteItemAsync");
        return result;
    }

    public async Task<Unit> DeleteOrderAsync(string id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting OrderService.");
        var result = await inner.DeleteOrderAsync(id, cancellationToken);
        logger.LogInformation("End OrderService.");
        return result;
    }

    public async Task<IList<OrderResponse>> GetAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting OrderService.");
        var result = await inner.GetAsync(cancellationToken);
        logger.LogInformation("End OrderService.");
        return result;
    }

    public async Task<OrderResponse?> GetAsync(string id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting OrderService.GetAsync (id)");
        var result = await inner.GetAsync(id, cancellationToken);
        logger.LogInformation("End OrderService.GetAsync (id)");
        return result;
    }

    public async Task<UpdateOrderResponse> UpdateOrderAsync(UpdateOrderRequest data, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting OrderService.UpdateOrderAsync");
        var result = await inner.UpdateOrderAsync(data, cancellationToken);
        logger.LogInformation("End OrderService.UpdateOrderAsync");
        return result;
    }
}
