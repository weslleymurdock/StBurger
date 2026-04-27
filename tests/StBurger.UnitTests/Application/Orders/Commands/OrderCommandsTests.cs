using StBurger.Application.Order.Commands;
using StBurger.Application.Order.Requests;
using FluentAssertions;
namespace StBurger.UnitTests.Application.Orders.Commands;

public class OrderCommandsTests
{
    #region  AddOrderItemCommand

    [Fact]
    public void AddOrderItemCommand_Should_Create_Correctly()
    {
        var orderId = Guid.NewGuid().ToString();
        var item = new NewOrderItemRequest(Guid.NewGuid().ToString());

        var command = new AddOrderItemCommand(orderId, item);

        command.Id.Should().Be(orderId);
        command.Data.Should().Be(item);
    }

    [Fact]
    public void AddOrderItemCommand_Should_Be_Equal_When_Same_Data()
    {
        var id = Guid.NewGuid().ToString();
        var item = new NewOrderItemRequest("1");

        var c1 = new AddOrderItemCommand(id, item);
        var c2 = new AddOrderItemCommand(id, item);

        c1.Should().BeEquivalentTo(c2);
    }

    #endregion

    #region  CreateOrderCommand

    [Fact]
    public void CreateOrderCommand_Should_Create_Correctly()
    {
        var request = new CreateOrderRequest(
            "att",
            "cust",
            new List<NewOrderItemRequest> { new("1") });

        var command = new CreateOrderCommand(request);

        command.Data.Should().Be(request);
    }

    [Fact]
    public void CreateOrderCommand_Should_Be_Equal_When_Same_Data()
    {
        var request = new CreateOrderRequest("a", "c", new List<NewOrderItemRequest>());

        var c1 = new CreateOrderCommand(request);
        var c2 = new CreateOrderCommand(request);

        c1.Should().BeEquivalentTo(c2);
    }

    #endregion

    #region  DeleteOrderCommand

    [Fact]
    public void DeleteOrderCommand_Should_Create_Correctly()
    {
        var id = Guid.NewGuid().ToString();

        var command = new DeleteOrderCommand(id);

        command.Id.Should().Be(id);
    }

    [Fact]
    public void DeleteOrderCommand_Should_Be_Equal_When_Same_Data()
    {
        var id = "123";

        var c1 = new DeleteOrderCommand(id);
        var c2 = new DeleteOrderCommand(id);

        c1.Should().BeEquivalentTo(c2);
    }

    #endregion

    #region DeleteOrderItemCommand

    [Fact]
    public void DeleteOrderItemCommand_Should_Create_Correctly()
    {
        var orderId = "order";
        var itemId = "item";

        var command = new DeleteOrderItemCommand(orderId, itemId);

        command.OrderId.Should().Be(orderId);
        command.Id.Should().Be(itemId);
    }

    [Fact]
    public void DeleteOrderItemCommand_Should_Be_Equal_When_Same_Data()
    {
        var c1 = new DeleteOrderItemCommand("o", "i");
        var c2 = new DeleteOrderItemCommand("o", "i");

        c1.Should().BeEquivalentTo(c2);
    }
    
    #endregion

    #region UpdateOrderCommand
    [Fact]
    public void UpdateOrderCommand_Should_Create_Correctly()
    {
        var request = new UpdateOrderRequest(
            Guid.NewGuid().ToString(),
            "att",
            "cust",
            new List<NewOrderItemRequest> { new("1") });

        var command = new UpdateOrderCommand(request);

        command.Data.Should().Be(request);
    }

    [Fact]
    public void UpdateOrderCommand_Should_Be_Equal_When_Same_Data()
    {
        var request = new UpdateOrderRequest(
            "1",
            "a",
            "c",
            new List<NewOrderItemRequest>());

        var c1 = new UpdateOrderCommand(request);
        var c2 = new UpdateOrderCommand(request);

        c1.Should().BeEquivalentTo(c2);
    }
    #endregion
}
