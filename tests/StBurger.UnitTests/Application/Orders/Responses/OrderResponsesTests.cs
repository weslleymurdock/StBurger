using FluentAssertions;
using StBurger.Application.Order.Responses;
using StBurger.Domain.Menu.Entities;
using StBurger.Domain.Orders.Entities;
namespace StBurger.UnitTests.Application.Orders.Responses;

public class OrderResponsesTests
{
    #region CreateOrderResponse

    [Fact]
    public void CreateOrderResponse_ToResponse_Should_Map_Correctly()
    {
        var order = new Order("att", "cust");

        var sandwich = new Sandwich("Burger", "desc", 10);
        var drink = new Drink("Coke", "desc", 5);

        order.AddItem(sandwich);
        order.AddItem(drink);

        var response = CreateOrderResponse.ToResponse(order);

        response.Id.Should().Be(order.Id);
        response.Attendant.Should().Be(order.Attendant);
        response.Customer.Should().Be(order.Customer);

        response.Items.Should().HaveCount(2);

        response.SubTotal.Should().Be(order.Subtotal);
        response.Discountered.Should().Be(order.Discount);
        response.TotalPrice.Should().Be(order.Total);
    }

    [Fact]
    public void CreateOrderResponse_ToResponse_Should_Throw_When_Null()
    {
        var act = () => CreateOrderResponse.ToResponse(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region OrderResponse

    [Fact]
    public void OrderResponse_FromEntity_Should_Map_Correctly()
    {
        var order = new Order("att", "cust");

        var sandwich = new Sandwich("Burger", "desc", 10);
        order.AddItem(sandwich);

        var response = OrderResponse.FromEntity(order);

        response.Id.Should().Be(order.Id);
        response.Attendant.Should().Be(order.Attendant);
        response.Customer.Should().Be(order.Customer);

        response.Items.Should().HaveCount(1);

        response.SubTotal.Should().Be(order.Subtotal);
        response.Discountered.Should().Be(order.Discount);
        response.TotalPrice.Should().Be(order.Total);
    }

    [Fact]
    public void OrderResponse_FromEntity_Should_Throw_When_Null()
    {
        var act = () => OrderResponse.FromEntity(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region OrdersResponse

    [Fact]
    public void OrdersResponse_Should_Create_Correctly()
    {
        var orders = new List<OrderResponse>
        {
            new("1", "a", "c", [], 0, 0, 0)
        };

        var response = new OrdersResponse(orders);

        response.Orders.Should().HaveCount(1);
    }

    [Fact]
    public void OrdersResponse_Should_Be_Equal_When_Same_Data()
    {
        var list = new List<OrderResponse>
        {
            new("1", "a", "c", [], 0, 0, 0)
        };

        var r1 = new OrdersResponse(list);
        var r2 = new OrdersResponse(list);

        r1.Should().BeEquivalentTo(r2);
    }

    #endregion

    #region UpdateOrderResponse

    [Fact]
    public void UpdateOrderResponse_ToResponse_Should_Map_Correctly()
    {
        var order = new Order("att", "cust");

        var sandwich = new Sandwich("Burger", "desc", 10);
        order.AddItem(sandwich);

        var response = UpdateOrderResponse.ToResponse(order);

        response.Id.Should().Be(order.Id);
        response.Attendant.Should().Be(order.Attendant);
        response.Customer.Should().Be(order.Customer);

        response.Items.Should().HaveCount(1);

        response.TotalPrice.Should().Be(order.Total);
    }

    [Fact]
    public void UpdateOrderResponse_ToResponse_Should_Throw_When_Null()
    {
        var act = () => UpdateOrderResponse.ToResponse(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion
}