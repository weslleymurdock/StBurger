using FluentAssertions;
using StBurger.Application.Order.Requests;

namespace StBurger.UnitTests.Application.Orders.Requests;

public class OrderRequestsTests
{
    #region CreateOrderRequest
 
    [Fact]
    public void CreateOrderRequest_ToEntity_Should_Map_Correctly()
    {
        var request = new CreateOrderRequest(
            AttendantName: "att",
            CustomerName: "cust",
            Items: []
        );

        var entity = CreateOrderRequest.ToEntity(request);

        entity.Should().NotBeNull();
        entity.Attendant.Should().Be("att");
        entity.Customer.Should().Be("cust");

        // entidade Order (pedido) inicia sem itens
        entity.Items.Should().BeEmpty();
    }

    [Fact]
    public void CreateOrderRequest_ToEntity_Should_Throw_When_Null()
    {
        CreateOrderRequest? request = null;

        var act = () => CreateOrderRequest.ToEntity(request!);

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("request");
    }

    #endregion

    #region NewOrderItemRequest
     
    [Fact]
    public void NewOrderItemRequest_Should_Create_Correctly()
    {
        var id = Guid.NewGuid().ToString();

        var request = new NewOrderItemRequest(id);

        request.Id.Should().Be(id);
    }

    [Fact]
    public void NewOrderItemRequest_Should_Be_Equal_When_Same_Value()
    {
        var id = "123";

        var r1 = new NewOrderItemRequest(id);
        var r2 = new NewOrderItemRequest(id);

        r1.Should().BeEquivalentTo(r2);
    }

    #endregion

    #region UpdateOrderRequest
     
    [Fact]
    public void UpdateOrderRequest_Should_Create_Correctly()
    {
        var id = Guid.NewGuid().ToString();

        var items = new List<NewOrderItemRequest>
        {
            new(Guid.NewGuid().ToString())
        };

        var request = new UpdateOrderRequest(
            Id: id,
            Attendant: "att",
            CustomerName: "cust",
            Items: items
        );

        request.Id.Should().Be(id);
        request.Attendant.Should().Be("att");
        request.CustomerName.Should().Be("cust");
        request.Items.Should().HaveCount(1);
    }

    [Fact]
    public void UpdateOrderRequest_Should_Be_Equal_When_Same_Data()
    {
        var id = "1";

        var items = new List<NewOrderItemRequest>
        {
            new("1")
        };

        var r1 = new UpdateOrderRequest(id, "a", "c", items);
        var r2 = new UpdateOrderRequest(id, "a", "c", items);

        r1.Should().BeEquivalentTo(r2);
    }

    #endregion
}
