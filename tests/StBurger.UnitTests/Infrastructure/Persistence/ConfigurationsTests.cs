using Microsoft.EntityFrameworkCore.Internal;
using StBurger.Domain.Menu.Entities;
using StBurger.Domain.Orders.Entities;
using StBurger.UnitTests.Shared.Factories;

namespace StBurger.UnitTests.Infrastructure.Persistence;

public class ConfigurationsTests
{
    [Fact]
    public void MenuItem_Should_Have_Unique_Name_Index()
    {
        using var context = TestDbContextFactory.Create();

        var entity = context.Model.FindEntityType(typeof(MenuItem));
        var index = entity!.GetIndexes()
            .FirstOrDefault(i => i.Properties.Any(p => p.Name == "Name"));

        Assert.NotNull(index);
        Assert.True(index!.IsUnique);
    }

    [Fact]
    public void MenuItem_Should_Have_Discriminator_Configured()
    {
        using var context = TestDbContextFactory.Create();

        var entity = context.Model.FindEntityType(typeof(MenuItem));

        var discriminator = entity!.GetDiscriminatorPropertyName();

        Assert.NotNull(discriminator);
        Assert.Equal("MenuItemType", discriminator);
    }

    [Fact]
    public void Order_Should_Have_Relationship_With_OrderItems()
    {
        using var context = TestDbContextFactory.Create();

        var entity = context.Model.FindEntityType(typeof(Order));

        var navigation = entity!.FindNavigation(nameof(Order.Items));

        Assert.NotNull(navigation);
        Assert.True(navigation!.IsCollection);
    }

    [Fact]
    public void OrderItem_Should_Have_Unique_Index_OrderId_And_MenuItemType()
    {
        using var context = TestDbContextFactory.Create();

        var entity = context.Model.FindEntityType(typeof(OrderItem));

        var index = entity!.GetIndexes()
            .FirstOrDefault(i =>
                i.Properties.Any(p => p.Name == "OrderId") &&
                i.Properties.Any(p => p.Name == "MenuItemType"));

        Assert.NotNull(index);
        Assert.True(index!.IsUnique);
    }

    [Fact]
    public void MenuItem_Name_Should_Have_MaxLength_100()
    {
        using var context = TestDbContextFactory.Create();

        var entity = context.Model.FindEntityType(typeof(MenuItem));
        var prop = entity!.FindProperty(nameof(MenuItem.Name));

        Assert.Equal(100, prop!.GetMaxLength());
        Assert.False(prop.IsNullable);
    }
}