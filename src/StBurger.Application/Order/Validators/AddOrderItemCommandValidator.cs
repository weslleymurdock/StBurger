namespace StBurger.Application.Order.Validators;

public sealed class AddOrderItemCommandValidator : AbstractValidator<AddOrderItemCommand>
{
    public AddOrderItemCommandValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage("A operação não pode ser nula.")
            .NotEmpty()
            .WithMessage("A operação não pode ser vazia.");
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("O id da operação não pode ser nulo")
            .NotEmpty()
            .WithMessage("O id da operação não pode ser vazio.")
            .Must(x => Guid.TryParse(x, out var _))
            .WithMessage("O id da operação deve ser uma string GUID válida.");

        RuleFor(x => x.Data)
            .NotNull()
            .WithMessage("Os dados do item não podem ser nulos.")
            .NotEmpty()
            .WithMessage("Os dados do item não podem estar vazios");

        RuleFor(x => x.Data.Id)
            .NotNull()
            .WithMessage("O id do item não pode ser nulo")
            .NotEmpty()
            .WithMessage("O id do item não pode ser vazio.")
            .Must(x => Guid.TryParse(x, out var _))
            .WithMessage("O id do item deve ser uma string GUID válida.");

    }
}
