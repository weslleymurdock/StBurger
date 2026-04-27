namespace StBurger.Application.Order.Validators;

public sealed class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage("A operação não pode ser nula.")
            .NotEmpty()
            .WithMessage("A operação não pode ser vazia.");

        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("O id do pedido não pode ser nulo")
            .NotEmpty()
            .WithMessage("O id do pedido não pode ser vazio.")
            .Must(x => Guid.TryParse(x, out var _))
            .WithMessage("O id do pedido deve ser uma string GUID válida.");
    }
}
