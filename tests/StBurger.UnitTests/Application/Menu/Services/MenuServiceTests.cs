using FluentAssertions;
using StBurger.Application.Core.Abstractions.Repositories;
using StBurger.Application.Menu.Requests;
using StBurger.Application.Menu.Services;
using StBurger.Domain.Menu.Entities;

namespace StBurger.UnitTests.Application.Menu.Services;

public class MenuServiceTests
{
    private readonly FakeUnitOfWork _uow = new();
    private readonly MenuService _service;

    public MenuServiceTests()
    {
        _service = new MenuService(_uow);
    }

    [Fact]
    public async Task CreateMenuItemAsync_Should_Create_Sandwich()
    {
        var request = new CreateMenuItemRequest("X-Burger", 10, "desc", "sandwich");

        using CancellationTokenSource c = new();

        var result = await _service.CreateMenuItemAsync(request, c.Token);

        result.Name.Should().Be("X-Burger");
        _uow.SandwichRepo.Added.Should().HaveCount(1);
    }

    [Fact]
    public async Task CreateMenuItemAsync_Should_Throw_Invalid_Type()
    {
        var request = new CreateMenuItemRequest("x", 10, "d", "invalid");

        using CancellationTokenSource c = new();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.CreateMenuItemAsync(request, c.Token));
    }

    [Fact]
    public async Task DeleteMenuItemAsync_Should_Delete_Sandwich()
    {
        var item = new Sandwich("X", "desc", 10);
        _uow.SandwichRepo.Store[item.Id] = item;

        using CancellationTokenSource c = new();

        await _service.DeleteMenuItemAsync(item.Id, c.Token);

        _uow.SandwichRepo.Deleted.Should().Contain(item.Id);
    }

    [Fact]
    public async Task DeleteMenuItemAsync_Should_Throw_When_Not_Found()
    {
        using CancellationTokenSource c = new();

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.DeleteMenuItemAsync("id", c.Token));
    }

    [Fact]
    public async Task GetAsync_Should_Return_Items()
    {
        _uow.SandwichRepo.Store["1"] = new Sandwich("X", "d", 10);

        using CancellationTokenSource c = new();

        var result = await _service.GetAsync(c.Token);

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetAsync_Should_Throw_When_Empty()
    {
        using CancellationTokenSource c = new();

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.GetAsync(c.Token));
    }

    [Fact]
    public async Task GetById_Should_Return_Item()
    {
        var item = new Sandwich("n", "d", 10);
        _uow.MenuRepo.Store[item.Id] = item;

        using CancellationTokenSource c = new();

        var result = await _service.GetAsync(item.Id, c.Token);

        result.Should().NotBeNull();
        result!.Name.Should().Be("n");
    }

    [Fact]
    public async Task PatchDescription_Should_Update()
    {
        var item = new Sandwich("n", "d", 10);
        _uow.MenuRepo.Store[item.Id] = item;

        using CancellationTokenSource c = new();

        await _service.PatchMenuItemDescriptionAsync((item.Id, "new"), c.Token);

        item.Description.Should().Be("new");
    }

    [Fact]
    public async Task PatchName_Should_Update()
    {
        var item = new Sandwich("n", "d", 10);
        _uow.MenuRepo.Store[item.Id] = item;

        using CancellationTokenSource c = new();

        await _service.PatchMenuItemNameAsync((item.Id, "new"), c.Token);

        item.Name.Should().Be("new");
    }

    [Fact]
    public async Task PatchPrice_Should_Update()
    {
        var item = new Sandwich("n", "d", 10);
        _uow.MenuRepo.Store[item.Id] = item;

        using CancellationTokenSource c = new();

        await _service.PatchMenuItemPriceAsync((item.Id, 20), c.Token);

        item.Price.Should().Be(20);
    }

    [Fact]
    public async Task UpdateMenuItemAsync_Should_Update_All()
    {
        var item = new Sandwich("n", "d", 10);
        _uow.MenuRepo.Store[item.Id] = item;

        var request = new UpdateMenuItemRequest(item.Id, "new", "desc", 30);

        using CancellationTokenSource c = new();

        var result = await _service.UpdateMenuItemAsync(request, c.Token);

        result.Name.Should().Be("new");
        result.Price.Should().Be(30);
    }
}