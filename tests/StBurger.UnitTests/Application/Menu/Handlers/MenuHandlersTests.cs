using MediatR;
using Moq;
using StBurger.Application.Core.Abstractions.Services;
using StBurger.Application.Menu.Commands;
using StBurger.Application.Menu.Handlers;
using StBurger.Application.Menu.Queries;
using StBurger.Application.Menu.Requests;
using StBurger.Application.Menu.Responses;
using StBurger.Domain.Menu.Exceptions;

namespace StBurger.UnitTests.Application.Menu.Handlers;

public class MenuHandlersTests
{
    private readonly Mock<IMenuService> _serviceMock = new();

    #region CreateMenuItemCommandHandler

    [Fact]
    public async Task CreateMenuItemCommandHandler_Handle_Should_Call_Service_And_Return_Response()
    {
        var request = new CreateMenuItemCommand(
            new CreateMenuItemRequest("X", 10, "Desc", "drink"));

        var expected = new CreateMenuItemResponse("1", "X", 10, "Desc");

        _serviceMock
            .Setup(s => s.CreateMenuItemAsync(request.Data, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var handler = new CreateMenuItemCommandHandler(_serviceMock.Object);

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.Equal(expected, result);
    }

    #endregion

    #region DeleteMenuItemCommandHandler

    [Fact]
    public async Task Handle_Should_Call_Service()
    {
        var request = new DeleteMenuItemCommand("1");

        _serviceMock
            .Setup(s => s.DeleteMenuItemAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var handler = new DeleteMenuItemCommandHandler(_serviceMock.Object);

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.Equal(Unit.Value, result);
    }

    #endregion

    #region GetMenuItemQueryHandler

    [Fact]
    public async Task Handle_Should_Return_MenuItem()
    {
        var request = new GetMenuItemQuery("1");

        var response = new MenuItemResponse("1", "X", 10, "Desc", "Drink");

        _serviceMock
            .Setup(s => s.GetAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var handler = new GetMenuItemQueryHandler(_serviceMock.Object);

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.Equal(response, result);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_NotFound()
    {
        var request = new GetMenuItemQuery("1");

        _serviceMock
            .Setup(s => s.GetAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MenuItemResponse?)null);

        var handler = new GetMenuItemQueryHandler(_serviceMock.Object);

        await Assert.ThrowsAsync<MenuItemNotFoundException>(() =>
            handler.Handle(request, CancellationToken.None));
    }

    #endregion

    #region GetMenuQueryHandler

    [Fact]
    public async Task Handle_Should_Return_MenuItemsResponse()
    {
        var items = new List<MenuItemResponse>
        {
            new("1", "X", 10, "Desc", "Drink")
        };

        _serviceMock
            .Setup(s => s.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(items);

        var handler = new GetMenuQueryHandler(_serviceMock.Object);

        var result = await handler.Handle(new GetMenuQuery(), CancellationToken.None);

        Assert.Single(result.Items);
    }

    #endregion

    #region PatchMenuItemDescriptionCommandHandler

    [Fact]
    public async Task Handle_Should_Call_Service_With_Correct_Params()
    {
        var request = new PatchMenuItemDescriptionCommand("1", "NewDesc");
        (string id, string description) p = (request.Id, request.Description); 
        _serviceMock
            .Setup(s => s.PatchMenuItemDescriptionAsync(
                p,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var handler = new PatchMenuItemDescriptionCommandHandler(_serviceMock.Object);

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.Equal(Unit.Value, result);
    }

    #endregion

    #region PatchMenuItemNameCommandHandler

    [Fact]
    public async Task Handle_Should_Call_Service_Name()
    {
        var request = new PatchMenuItemNameCommand("1", "NewName");
        (string id, string name) p = (id: request.Id, name: request.Name);
        _serviceMock
            .Setup(s => s.PatchMenuItemNameAsync(
                p,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var handler = new PatchMenuItemNameCommandHandler(_serviceMock.Object);

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.Equal(Unit.Value, result);
    }

    #endregion

    #region PatchMenuItemPriceCommandHandler

    [Fact]
    public async Task Handle_Should_Call_Service_Price()
    {
        var request = new PatchMenuItemPriceCommand("1", 20);
        (string id, decimal price) p = (id: request.Id, price: request.Price);
        _serviceMock
            .Setup(s => s.PatchMenuItemPriceAsync(
                p,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var handler = new PatchMenuItemPriceCommandHandler(_serviceMock.Object);

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.Equal(Unit.Value, result);
    }

    #endregion

    #region UpdateMenuItemCommandHandler

    [Fact]
    public async Task Handle_Should_Call_Service_And_Return_Response()
    {
        var request = new UpdateMenuItemCommand(
            new UpdateMenuItemRequest("1", "Name", "Desc", 10));

        var expected = new UpdateMenuItemResponse("1", "Name", "Desc", 10);

        _serviceMock
            .Setup(s => s.UpdateMenuItemAsync(request.Data, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var handler = new UpdateMenuItemCommandHandler(_serviceMock.Object);

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.Equal(expected, result);
    }

    #endregion
}