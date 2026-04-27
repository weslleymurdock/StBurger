using FluentAssertions;
using StBurger.Application.Menu.Responses;
using StBurger.Domain.Menu.Entities;
using StBurger.Domain.Orders.Entities;

namespace StBurger.UnitTests.Application.Menu.Responses;

public class MenuResponsesTests
{
    #region CreateMenuItemResponse

    [Fact]
    public void CreateMenuItemResponse_Should_Create_Correctly()
    {
        var response = new CreateMenuItemResponse("1", "Name", 10m, "Desc");

        response.Id.Should().Be("1");
        response.Name.Should().Be("Name");
        response.Price.Should().Be(10m);
        response.Description.Should().Be("Desc");
    }

    [Fact]
    public void CreateMenuItemResponse_Should_Be_Equal_When_Same_Data()
    {
        var r1 = new CreateMenuItemResponse("1", "Name", 10m, "Desc");
        var r2 = new CreateMenuItemResponse("1", "Name", 10m, "Desc");

        r1.Should().BeEquivalentTo(r2);
    }

    #endregion

    #region MenuItemResponse_From_MenuItem

    [Fact]
    public void FromEntity_Should_Map_Sandwich_Correctly()
    {
        var entity = new Sandwich("Burger", "Desc", 10m);

        var result = MenuItemResponse.FromEntity(entity);

        result.Type.Should().Be("Sandwich");
        result.Name.Should().Be("Burger");
        result.Price.Should().Be(10m);
    }

    [Fact]
    public void FromEntity_Should_Map_Drink_Correctly()
    {
        var entity = new Drink("Coke", "Desc", 5m);

        var result = MenuItemResponse.FromEntity(entity);

        result.Type.Should().Be("Drink");
    }

    [Fact]
    public void FromEntity_Should_Map_Side_Correctly()
    {
        var entity = new Side("Fries", "Desc", 7m);

        var result = MenuItemResponse.FromEntity(entity);

        result.Type.Should().Be("Side");
    }

    [Fact]
    public void MenuItemResponse_FromEntity_Should_Throw_When_Null()
    {
        var act = () => MenuItemResponse.FromEntity((MenuItem)null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region MenuItemResponse_From_OrderItem

    [Fact]
    public void FromEntity_OrderItem_Should_Map_Correctly()
    {
        var menuItem = new Sandwich("Burger", "Desc", 10m);
        var orderItem = new OrderItem("orderId", menuItem);

        var result = MenuItemResponse.FromEntity(orderItem);

        result.Type.Should().Be("Sandwich");
        result.Id.Should().Be(menuItem.Id);
    }

    [Fact]
    public void FromEntity_OrderItem_Should_Throw_When_OrderItem_Null()
    {
        var act = () => MenuItemResponse.FromEntity((OrderItem)null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void FromEntity_OrderItem_Should_Throw_When_MenuItem_Null()
    {
        var orderItem = new OrderItem();

        var act = () => MenuItemResponse.FromEntity(orderItem);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region MenuItemResponse_FromOrderItemResponse

    [Fact]
    public void FromOrderItemResponse_Should_Map_Correctly()
    {
        var menuItem = new Drink("Coke", "Desc", 5m);
        var orderItem = new OrderItem("orderId", menuItem);

        var result = MenuItemResponse.FromOrderItemResponse(orderItem);

        result.Type.Should().Be("Drink");
    }

    [Fact]
    public void FromOrderItemResponse_Should_Throw_When_Null()
    {
        var act = () => MenuItemResponse.FromOrderItemResponse(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region MenuItemsResponse

    [Fact]
    public void MenuItemsResponse_Should_Create_Correctly()
    {
        var items = new List<MenuItemResponse>
        {
            new("1", "Name", 10m, "Desc", "Drink")
        };

        var response = new MenuItemsResponse(items);

        response.Items.Should().HaveCount(1);
    }

    [Fact]
    public void MenuItemsResponse_Should_Be_Equal_When_Same_Data()
    {
        var items = new List<MenuItemResponse>
        {
            new("1", "Name", 10m, "Desc", "Drink")
        };

        var r1 = new MenuItemsResponse(items);
        var r2 = new MenuItemsResponse(items);

        r1.Should().BeEquivalentTo(r2);
    }

    #endregion

    #region UpdateMenuItemResponse

    [Fact]
    public void FromEntity_Should_Map_Correctly()
    {
        var entity = new Sandwich("Burger", "Desc", 10m);

        var result = UpdateMenuItemResponse.FromEntity(entity);

        result.Id.Should().Be(entity.Id);
        result.Name.Should().Be(entity.Name);
        result.Description.Should().Be(entity.Description);
        result.Price.Should().Be(entity.Price);
    }

    [Fact]
    public void UpdateMenuItemResponse_FromEntity_Should_Throw_When_Null()
    {
        var act = () => UpdateMenuItemResponse.FromEntity(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion
}