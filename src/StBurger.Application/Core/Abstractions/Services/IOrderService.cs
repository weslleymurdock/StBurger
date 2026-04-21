namespace StBurger.Application.Core.Abstractions.Services;

public interface IOrderService
{
    Task<OrderResponse> AddItemAsync(string id, string itemId, CancellationToken cancellationToken);
    Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest data, CancellationToken cancellationToken);
    Task<Unit> DeleteItemAsync(string id, string itemId, CancellationToken cancellationToken);
    Task<Unit> DeleteOrderAsync(string id, CancellationToken cancellationToken);
    Task<IList<OrderResponse>> GetAsync(CancellationToken cancellationToken);
    Task<OrderResponse?> GetAsync(string id, CancellationToken cancellationToken);
    Task<UpdateOrderResponse> UpdateOrderAsync(UpdateOrderRequest data, CancellationToken cancellationToken);
}
