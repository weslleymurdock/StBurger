using FluentAssertions;
using MediatR;
using StBurger.Application.Menu.Queries;
using StBurger.Application.Menu.Responses;

namespace StBurger.UnitTests.Application.Menu.Queries;

public class MenuQueriesTests
{
    #region GetMenuQuery

    [Fact]
    public void GetMenuQuery_Should_Create_Correctly()
    {
        var query = new GetMenuQuery();

        query.Should().NotBeNull();
    }

    [Fact]
    public void GetMenuQuery_Should_Implement_IRequest()
    {
        var query = new GetMenuQuery();

        query.Should().BeAssignableTo<IRequest<MenuItemsResponse>>();
    }

    #endregion

    #region GetMenuItemQuery

    [Fact]
    public void GetMenuItemQuery_Should_Create_Correctly()
    {
        var id = Guid.NewGuid().ToString();

        var query = new GetMenuItemQuery(id);

        query.Id.Should().Be(id);
    }

    [Fact]
    public void GetMenuItemQuery_Should_Be_Equal_When_Same_Data()
    {
        var id = "1";

        var q1 = new GetMenuItemQuery(id);
        var q2 = new GetMenuItemQuery(id);

        q1.Should().BeEquivalentTo(q2);
    }

    [Fact]
    public void GetMenuItemQuery_Should_Not_Be_Equal_When_Different_Data()
    {
        var q1 = new GetMenuItemQuery("1");
        var q2 = new GetMenuItemQuery("2");

        q1.Should().NotBeEquivalentTo(q2);
    }

    [Fact]
    public void GetMenuItemQuery_Should_Implement_IRequest()
    {
        var query = new GetMenuItemQuery("1");

        query.Should().BeAssignableTo<IRequest<MenuItemResponse>>();
    }

    #endregion
}