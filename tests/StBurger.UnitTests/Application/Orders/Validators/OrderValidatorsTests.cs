using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using StBurger.Application.Core.Abstractions.Repositories;
using StBurger.Application.Order.Commands;
using StBurger.Application.Order.Requests;
using StBurger.Application.Order.Validators;
using StBurger.Domain.Menu.Entities;

namespace StBurger.UnitTests.Application.Orders.Validators;

public class OrderValidatorsTests
{
    // -------------------------
    // AddOrderItemCommandValidator
    // -------------------------

    [Fact]
    public void AddOrderItemValidator_Should_Fail_When_Id_Invalid()
    {
        var validator = new AddOrderItemCommandValidator();

        var command = new AddOrderItemCommand("invalid", new("invalid"));

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Id);
        result.ShouldHaveValidationErrorFor(x => x.Data.Id);
    }

    [Fact]
    public void AddOrderItemValidator_Should_Pass_When_Valid()
    {
        var validator = new AddOrderItemCommandValidator();

        var command = new AddOrderItemCommand(
            Guid.NewGuid().ToString(),
            new(Guid.NewGuid().ToString()));

        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    // -------------------------
    // DeleteOrderCommandValidator
    // -------------------------

    [Fact]
    public void DeleteOrderValidator_Should_Fail_When_Id_Invalid()
    {
        var validator = new DeleteOrderCommandValidator();

        var result = validator.TestValidate(new DeleteOrderCommand("invalid"));

        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    // -------------------------
    // DeleteOrderItemCommandValidator
    // -------------------------

    [Fact]
    public void DeleteOrderItemValidator_Should_Fail_When_Ids_Invalid()
    {
        var validator = new DeleteOrderItemCommandValidator();

        var command = new DeleteOrderItemCommand("invalid", "invalid");

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Id);
        result.ShouldHaveValidationErrorFor(x => x.OrderId);
    }

    // -------------------------
    // CreateOrderCommandValidator
    // -------------------------

    [Fact]
    public void CreateOrderValidator_Should_Fail_When_Duplicated_Items()
    {
        var uowMock = new Mock<IUnitOfWork<string>>();

        var repoMock = new Mock<IRepository<MenuItem, string>>();
        repoMock.Setup(x => x.Entities)
            .Returns(new List<MenuItem>
            {
                new Sandwich("s", "", 10) { Id = "1" },
                new Drink("d", "", 5) { Id = "1" }
            }.AsQueryable());

        uowMock.Setup(x => x.Repository<MenuItem>())
            .Returns(repoMock.Object);

        var validator = new CreateOrderCommandValidator(uowMock.Object);

        var request = new CreateOrderCommand(
            new CreateOrderRequest("att", "cust",
                new List<NewOrderItemRequest>
                {
                    new("1"),
                    new("1")
                }));

        var result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Data.Items);
    }

    [Fact]
    public void CreateOrderValidator_Should_Fail_When_Same_Type()
    {
        var uowMock = new Mock<IUnitOfWork<string>>();

        var repoMock = new Mock<IRepository<MenuItem, string>>();
        repoMock.Setup(x => x.Entities)
            .Returns(new List<MenuItem>
            {
                new Sandwich("s1", "", 10) { Id = "1" },
                new Sandwich("s2", "", 12) { Id = "2" }
            }.AsQueryable());

        uowMock.Setup(x => x.Repository<MenuItem>())
            .Returns(repoMock.Object);

        var validator = new CreateOrderCommandValidator(uowMock.Object);

        var request = new CreateOrderCommand(
            new CreateOrderRequest("att", "cust",
                new List<NewOrderItemRequest>
                {
                    new("1"),
                    new("2")
                }));

        var result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Data.Items);
    }

    [Fact]
    public void CreateOrderValidator_Should_Pass_When_Valid()
    {
        var uowMock = new Mock<IUnitOfWork<string>>();

        var repoMock = new Mock<IRepository<MenuItem, string>>();
        repoMock.Setup(x => x.Entities)
            .Returns(new List<MenuItem>
            {
                new Sandwich("s", "", 10) { Id = "1" },
                new Drink("d", "", 5) { Id = "2" }
            }.AsQueryable());

        uowMock.Setup(x => x.Repository<MenuItem>())
            .Returns(repoMock.Object);

        var validator = new CreateOrderCommandValidator(uowMock.Object);

        var request = new CreateOrderCommand(
            new CreateOrderRequest("att", "cust",
                new List<NewOrderItemRequest>
                {
                    new("1"),
                    new("2")
                }));

        var result = validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    // -------------------------
    // OrderItemsValidator (custom)
    // -------------------------

    [Fact]
    public void OrderItemsValidator_Should_Fail_When_Null()
    {
        var validator = new OrderItemsValidator();

        var context = new ValidationContext<UpdateOrderCommand>(
            new UpdateOrderCommand(new("1", "a", "c", null!)));

        var result = validator.IsValid(context, null!);

        result.Should().BeFalse();
    }

    [Fact]
    public void OrderItemsValidator_Should_Fail_When_Empty()
    {
        var validator = new OrderItemsValidator();

        var items = new List<NewOrderItemRequest>();

        var context = new ValidationContext<UpdateOrderCommand>(
            new UpdateOrderCommand(new("1", "a", "c", items)));

        var result = validator.IsValid(context, items);

        result.Should().BeFalse();
    }

    [Fact]
    public void OrderItemsValidator_Should_Fail_When_Invalid_Guid()
    {
        var validator = new OrderItemsValidator();

        var items = new List<NewOrderItemRequest>
        {
            new("invalid")
        };

        var context = new ValidationContext<UpdateOrderCommand>(
            new UpdateOrderCommand(new("1", "a", "c", items)));

        var result = validator.IsValid(context, items);

        result.Should().BeFalse();
    }

    [Fact]
    public void OrderItemsValidator_Should_Fail_When_Duplicated()
    {
        var validator = new OrderItemsValidator();

        var items = new List<NewOrderItemRequest>
        {
            new("1"),
            new("1")
        };

        var context = new ValidationContext<UpdateOrderCommand>(
            new UpdateOrderCommand(new("1", "a", "c", items)));

        var result = validator.IsValid(context, items);

        result.Should().BeFalse();
    }

    // -------------------------
    // UpdateOrderCommandValidator
    // -------------------------

    [Fact]
    public void UpdateOrderValidator_Should_Fail_When_Invalid_Id()
    {
        var validator = new UpdateOrderCommandValidator();

        var command = new UpdateOrderCommand(
            new("invalid", "a", "c", new List<NewOrderItemRequest>()));

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Data.Id);
    }

    [Fact]
    public void UpdateOrderValidator_Should_Fail_When_Items_Invalid()
    {
        var validator = new UpdateOrderCommandValidator();

        var command = new UpdateOrderCommand(
            new("1", "a", "c", new List<NewOrderItemRequest> { new("invalid") }));

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrors();
    }

    [Fact]
    public void UpdateOrderValidator_Should_Pass_When_Valid()
    {
        var validator = new UpdateOrderCommandValidator();

        var command = new UpdateOrderCommand(
            new(Guid.NewGuid().ToString(), "a", "c",
                new List<NewOrderItemRequest>
                {
                    new(Guid.NewGuid().ToString())
                }));

        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}