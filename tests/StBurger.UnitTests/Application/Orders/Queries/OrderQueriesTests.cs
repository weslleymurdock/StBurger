using FluentAssertions;
using MediatR;
using StBurger.Application.Order.Queries;
using StBurger.Application.Order.Responses;

namespace StBurger.UnitTests.Application.Orders.Queries;

public class OrderQueriesTests
{
    #region GetOrderByIdQuery

    [Fact]
    public void GetOrderByIdQuery_Should_Create_Correctly()
    {
        var id = Guid.NewGuid().ToString();

        var query = new GetOrderByIdQuery(id);

        query.Id.Should().Be(id);
    }

    [Fact]
    public void GetOrderByIdQuery_Should_Be_Equal_When_Same_Data()
    {
        var id = "123";

        var q1 = new GetOrderByIdQuery(id);
        var q2 = new GetOrderByIdQuery(id);

        q1.Should().BeEquivalentTo(q2);
    }

    [Fact]
    public void GetOrderByIdQuery_Should_Not_Be_Equal_When_Different_Data()
    {
        var q1 = new GetOrderByIdQuery("1");
        var q2 = new GetOrderByIdQuery("2");

        q1.Should().NotBeEquivalentTo(q2);
    }

    #endregion

    #region GetOrdersQuery

    [Fact]
    public void GetOrdersQuery_Should_Create_Correctly()
    {
        var query = new GetOrdersQuery();

        query.Should().NotBeNull();
    }
     
    [Fact]
    public void GetOrdersQuery_Should_Implement_IRequest()
    {
        var query = new GetOrdersQuery();

        query.Should().BeAssignableTo<IRequest<OrdersResponse>>();
    }

    #endregion

}
