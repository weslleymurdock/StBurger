using FluentAssertions;
using StBurger.Application.Menu.Requests;
using StBurger.Domain.Menu.Entities;

namespace StBurger.UnitTests.Application.Menu.Requests;

public class MenuRequestsTests
{
    #region CreateMenuItemRequest

    [Fact]
    public void ToEntity_Should_Create_Sandwich_When_Type_Is_Sandwich()
    {
        var request = new CreateMenuItemRequest("Burger", 10m, "Desc", "sandwich");

        var result = CreateMenuItemRequest.ToEntity(request);

        result.Should().BeOfType<Sandwich>();
        result.Name.Should().Be("Burger");
        result.Price.Should().Be(10m);
        result.Description.Should().Be("Desc");
    }

    [Fact]
    public void ToEntity_Should_Create_Sandwich_When_Type_Is_Lanche()
    {
        var request = new CreateMenuItemRequest("Burger", 10m, "Desc", "lanche");

        var result = CreateMenuItemRequest.ToEntity(request);

        result.Should().BeOfType<Sandwich>();
    }

    [Fact]
    public void ToEntity_Should_Create_Drink_When_Type_Is_Drink()
    {
        var request = new CreateMenuItemRequest("Coke", 5m, "Desc", "drink");

        var result = CreateMenuItemRequest.ToEntity(request);

        result.Should().BeOfType<Drink>();
    }

    [Fact]
    public void ToEntity_Should_Create_Drink_When_Type_Is_Bebida()
    {
        var request = new CreateMenuItemRequest("Coke", 5m, "Desc", "bebida");

        var result = CreateMenuItemRequest.ToEntity(request);

        result.Should().BeOfType<Drink>();
    }

    [Fact]
    public void ToEntity_Should_Create_Side_When_Type_Is_Side()
    {
        var request = new CreateMenuItemRequest("Fries", 7m, "Desc", "side");

        var result = CreateMenuItemRequest.ToEntity(request);

        result.Should().BeOfType<Side>();
    }

    [Fact]
    public void ToEntity_Should_Create_Side_When_Type_Is_Acompanhamento()
    {
        var request = new CreateMenuItemRequest("Fries", 7m, "Desc", "acompanhamento");

        var result = CreateMenuItemRequest.ToEntity(request);

        result.Should().BeOfType<Side>();
    }

    [Fact]
    public void ToEntity_Should_Be_Case_Insensitive()
    {
        var request = new CreateMenuItemRequest("Burger", 10m, "Desc", "SaNdWiCh");

        var result = CreateMenuItemRequest.ToEntity(request);

        result.Should().BeOfType<Sandwich>();
    }

    [Fact]
    public void ToEntity_Should_Throw_When_Type_Is_Invalid()
    {
        var request = new CreateMenuItemRequest("Item", 1m, "Desc", "invalid");

        var act = () => CreateMenuItemRequest.ToEntity(request);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*Invalid type*");
    }

    #endregion

    #region PatchMenuItemDescriptionRequest

    [Fact]
    public void PatchMenuItemDescriptionRequest_Should_Create_Correctly()
    {
        var request = new PatchMenuItemDescriptionRequest("1", "Desc");

        request.Id.Should().Be("1");
        request.Description.Should().Be("Desc");
    }

    #endregion

    #region PatchMenuItemNameRequest

    [Fact]
    public void PatchMenuItemNameRequest_Should_Create_Correctly()
    {
        var request = new PatchMenuItemNameRequest("1", "Name");

        request.Id.Should().Be("1");
        request.Name.Should().Be("Name");
    }

    #endregion

    #region PatchMenuItemPriceRequest

    [Fact]
    public void PatchMenuItemPriceRequest_Should_Create_Correctly()
    {
        var request = new PatchMenuItemPriceRequest("1", 99.9m);

        request.Id.Should().Be("1");
        request.Price.Should().Be(99.9m);
    }

    #endregion

    #region UpdateMenuItemRequest

    [Fact]
    public void UpdateMenuItemRequest_Should_Create_Correctly()
    {
        var request = new UpdateMenuItemRequest("1", "Name", "Desc", 10m);

        request.Id.Should().Be("1");
        request.Name.Should().Be("Name");
        request.Description.Should().Be("Desc");
        request.Price.Should().Be(10m);
    }

    [Fact]
    public void UpdateMenuItemRequest_Should_Be_Equal_When_Same_Data()
    {
        var r1 = new UpdateMenuItemRequest("1", "Name", "Desc", 10m);
        var r2 = new UpdateMenuItemRequest("1", "Name", "Desc", 10m);

        r1.Should().BeEquivalentTo(r2);
    }

    #endregion
}