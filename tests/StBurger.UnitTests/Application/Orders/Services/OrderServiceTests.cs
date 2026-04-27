using FluentAssertions;
using Moq;
using StBurger.Application.Core.Abstractions.Repositories;
using StBurger.Application.Order.Requests;
using StBurger.Application.Order.Services;
using StBurger.Domain.Menu.Entities;
using StBurger.Domain.Orders.Entities;
using StBurger.UnitTests.Shared.Builders;

namespace StBurger.UnitTests.Application.Orders.Services;

public class OrderServiceTests
{
    private readonly Mock<IUnitOfWork<string>> _uow = new();
    private readonly Mock<IRepository<MenuItem, string>> _menuRepo = new();
    private readonly Mock<IRepository<Order, string>> _orderRepo = new();
    private readonly Mock<IRepository<OrderItem, string>> _orderItemRepo = new();
    private readonly Mock<IOrderReadOnlyRepository> _ro = new();

    private readonly OrderService _service;

    public OrderServiceTests()
    {
        _uow.Setup(x => x.Repository<MenuItem>()).Returns(_menuRepo.Object);
        _uow.Setup(x => x.Repository<Order>()).Returns(_orderRepo.Object);
        _uow.Setup(x => x.Repository<OrderItem>()).Returns(_orderItemRepo.Object);

        _service = new OrderService(_uow.Object, _ro.Object);
    }

    [Fact]
    public async Task AddItem_Should_Add()
    {
        var order = new Order("a", "c");
        var item = new Sandwich("n", "d", 10);

        using CancellationTokenSource c = new(); 

        _ro.Setup(x => x.GetByIdWithItemsAsync(order.Id, c.Token)).ReturnsAsync(order);
        _menuRepo.Setup(x => x.Entities).Returns(new List<MenuItem> { item }.AsQueryable());
        
        var result = await _service.AddItemAsync(order.Id, item.Id, c.Token);

        result.Items.Should().HaveCount(1);
    }

    [Fact]
    public async Task AddItem_Should_Throw_When_NotFound()
    {
        var order = new Order("a", "c");

        using CancellationTokenSource c = new(); 
        
        _ro.Setup(x => x.GetByIdWithItemsAsync(order.Id, c.Token)).ReturnsAsync(order);

        _menuRepo.Setup(x => x.Entities).Returns(new List<MenuItem>().AsQueryable());
        
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.AddItemAsync(order.Id, "x", c.Token));
    }

    [Fact]
    public async Task CreateOrder_Should_Create()
    {
        var item = new Sandwich("n", "d", 10);

        _menuRepo.Setup(x => x.Entities).Returns(new List<MenuItem> { item }.AsQueryable());

        var request = new CreateOrderRequest("a", "c", [new(item.Id)]);

        using CancellationTokenSource c = new(); 
        
        var result = await _service.CreateOrderAsync(request, c.Token);

        result.Items.Should().HaveCount(1);
        _orderRepo.Verify(x => x.AddAsync(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public async Task CreateOrder_Should_Throw_When_ItemMissing()
    {
        _menuRepo.Setup(x => x.Entities).Returns(new List<MenuItem>().AsQueryable());

        var request = new CreateOrderRequest("a", "c", [new("x")]);

        using CancellationTokenSource c = new(); 
        
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.CreateOrderAsync(request, c.Token));
    }

    [Fact]
    public async Task DeleteItem_Should_Remove()
    {
        var order = new Order("a", "c");
        var item = new Sandwich("n", "d", 10);
        order.AddItem(item);

        using CancellationTokenSource c = new(); 
        
        _ro.Setup(x => x.GetByIdWithItemsAsync(order.Id, c.Token)).ReturnsAsync(order);

        await _service.DeleteItemAsync(order.Id, item.Id, c.Token);

        order.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteItem_Should_Throw_When_NotExists()
    {
        var order = new Order("a", "c");

        using CancellationTokenSource c = new(); 
        
        _ro.Setup(x => x.GetByIdWithItemsAsync(order.Id, c.Token)).ReturnsAsync(order);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.DeleteItemAsync(order.Id, "x", c.Token));
    }

    [Fact]
    public async Task DeleteOrder_Should_Delete()
    {
        var order = new Order("a", "c");

        _orderRepo.Setup(x => x.Entities).Returns(new List<Order> { order }.AsQueryable());

        using CancellationTokenSource c = new(); 
        
        await _service.DeleteOrderAsync(order.Id, c.Token);

        _orderRepo.Verify(x => x.DeleteAsync(order), Times.Once);
    }

    [Fact]
    public async Task DeleteOrder_Should_Throw_When_NotFound()
    {
        _orderRepo.Setup(x => x.Entities).Returns(new List<Order>().AsQueryable());

        using CancellationTokenSource c = new();

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.DeleteOrderAsync("x", c.Token));
    }

    [Fact]
    public async Task Get_Should_Return_List()
    {
        var order = new Order("a", "c");

        using CancellationTokenSource c = new();

        _ro.Setup(x => x.GetWithItemsAsync(c.Token)).ReturnsAsync([order]);

        var result = await _service.GetAsync(c.Token);

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task Get_Should_Throw_When_Empty()
    {
        using CancellationTokenSource c = new();

        _ro.Setup(x => x.GetWithItemsAsync(c.Token)).ReturnsAsync([]);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.GetAsync(c.Token));
    }

    [Fact]
    public async Task UpdateOrder_Should_Update_Items()
    {
        var order = new Order("a", "c");
        var oldItem = new Sandwich("old", "d", 10);
        var newItem = new Sandwich("new", "d", 20);

        order.AddItem(oldItem);

        using CancellationTokenSource c = new();
        
        _ro.Setup(x => x.GetByIdWithItemsAsync(order.Id, c.Token)).ReturnsAsync(order);
        _menuRepo.Setup(x => x.Entities).Returns(new List<MenuItem> { newItem }.AsQueryable());

        var request = new UpdateOrderRequest(order.Id, "att", "cust", [new(newItem.Id)]);

        var result = await _service.UpdateOrderAsync(request, c.Token);

        result.Items.Should().Contain(x => x.Id == newItem.Id);
    }

    [Fact]
    public async Task UpdateOrder_Should_Throw_When_ItemMissing()
    {
        var order = new Order("a", "c");

        using CancellationTokenSource c = new();

        _ro.Setup(x => x.GetByIdWithItemsAsync(order.Id, c.Token)).ReturnsAsync(order);

        _menuRepo.Setup(x => x.Entities).Returns(new List<MenuItem>().AsQueryable());

        var request = new UpdateOrderRequest(order.Id, "a", "c", [new("x")]);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.UpdateOrderAsync(request, c.Token));
    }
}