using FluentAssertions;
using MediatR;
using Moq;
using StBurger.Application.Core.Abstractions.Services;
using StBurger.Application.Order.Commands;
using StBurger.Application.Order.Handlers;
using StBurger.Application.Order.Queries;
using StBurger.Application.Order.Requests;
using StBurger.Application.Order.Responses;

namespace StBurger.UnitTests.Application.Orders.Handlers;

public class OrderHandlersTests
{
    private readonly Mock<IOrderService> _serviceMock = new();

    #region AddOrderItemCommandHandler

    [Fact]
    public async Task AddOrderItemHandler_Should_Call_Service()
    {
        var orderId = "order";
        var itemId = "item";

        var response = new OrderResponse(orderId, "", "", [], 0, 0, 0);

        _serviceMock.Setup(x => x.AddItemAsync(orderId, itemId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var handler = new AddOrderItemCommandHandler(_serviceMock.Object);
        using CancellationTokenSource c = new();
        var result = await handler.Handle(
            new AddOrderItemCommand(orderId, new NewOrderItemRequest(itemId)),
            c.Token);

        result.Should().Be(response);

        _serviceMock.Verify(x => x.AddItemAsync(orderId, itemId, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region CreateOrderCommandHandler

    [Fact]
    public async Task CreateOrderHandler_Should_Call_Service()
    {
        var request = new CreateOrderRequest("att", "cust", []);
        var response = new CreateOrderResponse("1", "", "", [], 0, 0, 0);

        _serviceMock.Setup(x => x.CreateOrderAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var handler = new CreateOrderCommandHandler(_serviceMock.Object);
        using CancellationTokenSource c = new();
        var result = await handler.Handle(new CreateOrderCommand(request), c.Token);

        result.Should().Be(response);

        _serviceMock.Verify(x => x.CreateOrderAsync(request, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region  DeleteOrderCommandHandler

    [Fact]
    public async Task DeleteOrderHandler_Should_Call_Service()
    {
        var id = "order";

        _serviceMock.Setup(x => x.DeleteOrderAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var handler = new DeleteOrderCommandHandler(_serviceMock.Object);
        using CancellationTokenSource c = new();
        var result = await handler.Handle(new DeleteOrderCommand(id), c.Token);

        result.Should().Be(Unit.Value);

        _serviceMock.Verify(x => x.DeleteOrderAsync(id, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region  DeleteOrderItemCommandHandler
    
    [Fact]
    public async Task DeleteOrderItemHandler_Should_Call_Service()
    {
        var orderId = "order";
        var itemId = "item";

        _serviceMock.Setup(x => x.DeleteItemAsync(orderId, itemId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var handler = new DeleteOrderItemCommandHandler(_serviceMock.Object);
        using CancellationTokenSource c = new();
        var result = await handler.Handle(
            new DeleteOrderItemCommand(orderId, itemId),
            c.Token);

        result.Should().Be(Unit.Value);

        _serviceMock.Verify(x => x.DeleteItemAsync(orderId, itemId, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region  GetOrderByIdQueryHandler
    
    [Fact]
    public async Task GetOrderByIdHandler_Should_Return_Order()
    {
        var id = "1";
        var response = new OrderResponse(id, "", "", [], 0, 0, 0);

        _serviceMock.Setup(x => x.GetAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var handler = new GetOrderByIdQueryHandler(_serviceMock.Object);
        using CancellationTokenSource c = new();
        var result = await handler.Handle(new GetOrderByIdQuery(id), c.Token);

        result.Should().Be(response);
    }

    [Fact]
    public async Task GetOrderByIdHandler_Should_Throw_When_Null()
    {
        var id = "1";

        _serviceMock.Setup(x => x.GetAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((OrderResponse?)null);

        var handler = new GetOrderByIdQueryHandler(_serviceMock.Object);
        using CancellationTokenSource c = new();
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(new GetOrderByIdQuery(id), c.Token));
    }

    #endregion

    #region  GetOrdersQueryHandler
    
    [Fact]
    public async Task GetOrdersHandler_Should_Return_List()
    {
        var list = new List<OrderResponse>
        {
            new("1", "", "", [], 0, 0, 0)
        };

        _serviceMock.Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(list);

        var handler = new GetOrdersQueryHandler(_serviceMock.Object);
        using CancellationTokenSource c = new();
        OrdersResponse result = await handler.Handle(new GetOrdersQuery(), c.Token);

        result.Should().NotBeNull();
        result.Orders.Should().HaveCount(1);
    }

    #endregion

    #region  UpdateOrderCommandHandler
    
    [Fact]
    public async Task UpdateOrderHandler_Should_Call_Service()
    {
        var request = new UpdateOrderRequest("1", "att", "cust", []);
        var response = new UpdateOrderResponse("1", "", "", [], 0);

        _serviceMock.Setup(x => x.UpdateOrderAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var handler = new UpdateOrderCommandHandler(_serviceMock.Object);
        using CancellationTokenSource c = new();
        UpdateOrderResponse result = await handler.Handle(new UpdateOrderCommand(request), c.Token);

        result.Should().Be(response);

        _serviceMock.Verify(x => x.UpdateOrderAsync(request, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region  Exception propagation (generic safety)
    
    [Fact]
    public async Task Handlers_Should_Propagate_Exception()
    {
        var request = new CreateOrderRequest("att", "cust", []);

        _serviceMock.Setup(x => x.CreateOrderAsync(request, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("fail"));

        var handler = new CreateOrderCommandHandler(_serviceMock.Object);
        using CancellationTokenSource c = new();
        await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(new CreateOrderCommand(request), c.Token));
    }
    
    #endregion
}
