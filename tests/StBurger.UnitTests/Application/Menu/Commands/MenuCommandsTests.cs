using FluentAssertions;
using MediatR;
using StBurger.Application.Menu.Commands;
using StBurger.Application.Menu.Requests;
using StBurger.Application.Menu.Responses;

namespace StBurger.UnitTests.Application.Menu.Commands;

public class MenuCommandsTests
{
    #region CreateMenuItemCommand

    [Fact]
    public void CreateMenuItemCommand_Should_Create_Correctly()
    {
        var request = new CreateMenuItemRequest("Burger", 10m, "Desc", "sandwich");

        var command = new CreateMenuItemCommand(request);

        command.Data.Should().Be(request);
    }

    [Fact]
    public void CreateMenuItemCommand_Should_Be_Equal_When_Same_Data()
    {
        var request = new CreateMenuItemRequest("Burger", 10m, "Desc", "sandwich");

        var c1 = new CreateMenuItemCommand(request);
        var c2 = new CreateMenuItemCommand(request);

        c1.Should().BeEquivalentTo(c2);
    }

    [Fact]
    public void CreateMenuItemCommand_Should_Implement_IRequest()
    {
        var command = new CreateMenuItemCommand(
            new CreateMenuItemRequest("Burger", 10m, "Desc", "sandwich"));

        command.Should().BeAssignableTo<IRequest<CreateMenuItemResponse>>();
    }

    #endregion

    #region DeleteMenuItemCommand

    [Fact]
    public void DeleteMenuItemCommand_Should_Create_Correctly()
    {
        var id = Guid.NewGuid().ToString();

        var command = new DeleteMenuItemCommand(id);

        command.Id.Should().Be(id);
    }

    [Fact]
    public void DeleteMenuItemCommand_Should_Be_Equal_When_Same_Data()
    {
        var id = "1";

        var c1 = new DeleteMenuItemCommand(id);
        var c2 = new DeleteMenuItemCommand(id);

        c1.Should().BeEquivalentTo(c2);
    }

    [Fact]
    public void DeleteMenuItemCommand_Should_Implement_IRequest()
    {
        var command = new DeleteMenuItemCommand("1");

        command.Should().BeAssignableTo<IRequest<Unit>>();
    }

    #endregion

    #region PatchMenuItemDescriptionCommand

    [Fact]
    public void PatchMenuItemDescriptionCommand_Should_Create_Correctly()
    {
        var command = new PatchMenuItemDescriptionCommand("1", "Nova desc");

        command.Id.Should().Be("1");
        command.Description.Should().Be("Nova desc");
    }

    [Fact]
    public void PatchMenuItemDescriptionCommand_Should_Be_Equal_When_Same_Data()
    {
        var c1 = new PatchMenuItemDescriptionCommand("1", "Desc");
        var c2 = new PatchMenuItemDescriptionCommand("1", "Desc");

        c1.Should().BeEquivalentTo(c2);
    }

    [Fact]
    public void PatchMenuItemDescriptionCommand_Should_Implement_IRequest()
    {
        var command = new PatchMenuItemDescriptionCommand("1", "Desc");

        command.Should().BeAssignableTo<IRequest<Unit>>();
    }

    #endregion

    #region PatchMenuItemNameCommand

    [Fact]
    public void PatchMenuItemNameCommand_Should_Create_Correctly()
    {
        var command = new PatchMenuItemNameCommand("1", "Novo nome");

        command.Id.Should().Be("1");
        command.Name.Should().Be("Novo nome");
    }

    [Fact]
    public void PatchMenuItemNameCommand_Should_Be_Equal_When_Same_Data()
    {
        var c1 = new PatchMenuItemNameCommand("1", "Nome");
        var c2 = new PatchMenuItemNameCommand("1", "Nome");

        c1.Should().BeEquivalentTo(c2);
    }

    [Fact]
    public void PatchMenuItemNameCommand_Should_Implement_IRequest()
    {
        var command = new PatchMenuItemNameCommand("1", "Nome");

        command.Should().BeAssignableTo<IRequest<Unit>>();
    }

    #endregion

    #region PatchMenuItemPriceCommand

    [Fact]
    public void PatchMenuItemPriceCommand_Should_Create_Correctly()
    {
        var command = new PatchMenuItemPriceCommand("1", 99.9m);

        command.Id.Should().Be("1");
        command.Price.Should().Be(99.9m);
    }

    [Fact]
    public void PatchMenuItemPriceCommand_Should_Be_Equal_When_Same_Data()
    {
        var c1 = new PatchMenuItemPriceCommand("1", 10m);
        var c2 = new PatchMenuItemPriceCommand("1", 10m);

        c1.Should().BeEquivalentTo(c2);
    }

    [Fact]
    public void PatchMenuItemPriceCommand_Should_Implement_IRequest()
    {
        var command = new PatchMenuItemPriceCommand("1", 10m);

        command.Should().BeAssignableTo<IRequest<Unit>>();
    }

    #endregion

    #region UpdateMenuItemCommand

    [Fact]
    public void UpdateMenuItemCommand_Should_Create_Correctly()
    {
        var request = new UpdateMenuItemRequest("1", "Nome", "Desc", 10m);

        var command = new UpdateMenuItemCommand(request);

        command.Data.Should().Be(request);
    }

    [Fact]
    public void UpdateMenuItemCommand_Should_Be_Equal_When_Same_Data()
    {
        var request = new UpdateMenuItemRequest("1", "Nome", "Desc", 10m);

        var c1 = new UpdateMenuItemCommand(request);
        var c2 = new UpdateMenuItemCommand(request);

        c1.Should().BeEquivalentTo(c2);
    }

    [Fact]
    public void UpdateMenuItemCommand_Should_Implement_IRequest()
    {
        var command = new UpdateMenuItemCommand(
            new UpdateMenuItemRequest("1", "Nome", "Desc", 10m));

        command.Should().BeAssignableTo<IRequest<UpdateMenuItemResponse>>();
    }

    #endregion
}