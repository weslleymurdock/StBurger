namespace StBurger.Application.Order.Validators;

public sealed class DeleteOrderItemCommandValidator : AbstractValidator<DeleteOrderItemCommand>
{
    public DeleteOrderItemCommandValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage("A operação não pode ser nula.")
            .NotEmpty()
            .WithMessage("A operação não pode ser vazia.");

        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("O id do item do pedido não pode ser nulo")
            .NotEmpty()
            .WithMessage("O id do item do pedido não pode ser vazio.")
            .Must(x => Guid.TryParse(x, out var _))
            .WithMessage("O id do item do pedido deve ser uma string GUID válida.");
        
        RuleFor(x => x.OrderId)
            .NotNull()
            .WithMessage("O id do pedido não pode ser nulo")
            .NotEmpty()
            .WithMessage("O id do pedido não pode ser vazio.")
            .Must(x => Guid.TryParse(x, out var _))
            .WithMessage("O id do pedido deve ser uma string GUID válida.");
    }
}