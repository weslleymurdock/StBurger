using FluentValidation.TestHelper;
using StBurger.Application.Menu.Commands;
using StBurger.Application.Menu.Queries;
using StBurger.Application.Menu.Requests;
using StBurger.Application.Menu.Validators;
namespace StBurger.UnitTests.Application.Menu.Validators;

public class MenuValidatorsTests
{
    #region CreateMenuItemCommandValidator

    [Fact]
    public void Validator_Should_Fail_When_Name_Invalid()
    {
        var validator = new CreateMenuItemCommandValidator();

        var command = new CreateMenuItemCommand(
            new CreateMenuItemRequest("", 10, "desc", "drink"));

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Data.Name);
    }

    [Fact]
    public void Should_Fail_When_Type_Invalid()
    {
        var validator = new CreateMenuItemCommandValidator();

        var command = new CreateMenuItemCommand(
            new CreateMenuItemRequest("Name", 10, "desc", "invalid"));

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Data.Type);
    }

    [Fact]
    public void Validator_Should_Fail_When_Price_Invalid()
    {
        var validator = new CreateMenuItemCommandValidator();

        var command = new CreateMenuItemCommand(
            new CreateMenuItemRequest("Name", 0, "desc", "drink"));

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Data.Price);
    }

    [Fact]
    public void Validator_Should_Pass_When_Valid()
    {
        var validator = new CreateMenuItemCommandValidator();

        var command = new CreateMenuItemCommand(
            new CreateMenuItemRequest("Name", 10, "desc", "drink"));

        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion

    #region DeleteMenuItemCommandValidator

    [Fact]
    public void Validator_Should_Fail_When_Id_Invalid()
    {
        var validator = new DeleteMenuItemCommandValidator();

        var result = validator.TestValidate(new DeleteMenuItemCommand("invalid"));

        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    #endregion

    #region GetMenuItemQueryValidator

    [Fact]
    public void Should_Fail_When_Id_Invalid_Query()
    {
        var validator = new GetMenuItemQueryValidator();

        var result = validator.TestValidate(new GetMenuItemQuery("invalid"));

        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    #endregion

    #region PatchMenuItemDescriptionCommandValidator

    [Fact]
    public void Should_Fail_When_Description_Empty()
    {
        var validator = new PatchMenuItemDescriptionCommandValidator();

        var result = validator.TestValidate(
            new PatchMenuItemDescriptionCommand(Guid.NewGuid().ToString(), ""));

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    #endregion

    #region PatchMenuItemNameCommandValidator

    [Fact]
    public void PatchMenuItemNameCommandValidator_Should_Fail_When_Name_Invalid()
    {
        var validator = new PatchMenuItemNameCommandValidator();

        var result = validator.TestValidate(
            new PatchMenuItemNameCommand(Guid.NewGuid().ToString(), "A"));

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    #endregion

    #region PatchMenuItemPriceCommandValidator

    [Fact]
    public void PatchMenuItemPriceCommandValidator_Should_Fail_When_Price_Invalid()
    {
        var validator = new PatchMenuItemPriceCommandValidator();

        var result = validator.TestValidate(
            new PatchMenuItemPriceCommand(Guid.NewGuid().ToString(), 0));

        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    #endregion

    #region UpdateMenuItemCommandValidator

    [Fact]
    public void Should_Fail_When_Id_Invalid()
    {
        var validator = new UpdateMenuItemCommandValidator();

        var command = new UpdateMenuItemCommand(
            new UpdateMenuItemRequest("invalid", "Name", "Desc", 10));

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Data.Id);
    }

    [Fact]
    public void Should_Fail_When_Name_Invalid()
    {
        var validator = new UpdateMenuItemCommandValidator();

        var command = new UpdateMenuItemCommand(
            new UpdateMenuItemRequest(Guid.NewGuid().ToString(), "", "Desc", 10));

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Data.Name);
    }

    [Fact]
    public void Should_Fail_When_Price_Invalid()
    {
        var validator = new UpdateMenuItemCommandValidator();

        var command = new UpdateMenuItemCommand(
            new UpdateMenuItemRequest(Guid.NewGuid().ToString(), "Name", "Desc", 0));

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Data.Price);
    }

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var validator = new UpdateMenuItemCommandValidator();

        var command = new UpdateMenuItemCommand(
            new UpdateMenuItemRequest(Guid.NewGuid().ToString(), "Name", "Desc", 10));

        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion
}