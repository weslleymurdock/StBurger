namespace StBurger.Application.Order.Validators;

public sealed class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage("A operação não pode ser nula.")
            .NotEmpty()
            .WithMessage("A operação não pode ser vazia.");

        RuleFor(x => x.Data)
            .NotNull()
            .WithMessage("Os dados da operação não podem ser nulos.")
            .NotEmpty()
            .WithMessage("Os dados da operação não podem estar vazios.");

        RuleFor(x => x.Data.Id)
            .NotNull()
            .WithMessage("O id do pedido não pode ser nulo")
            .NotEmpty()
            .WithMessage("O id do pedido não pode ser vazio.")
            .Must(x => Guid.TryParse(x, out var _))
            .WithMessage("O id do pedido deve ser uma string GUID válida.");

        RuleFor(x => x.Data.CustomerName)
            .MaximumLength(96)
            .WithMessage("O nome do cliente deve ter no máximo 96 caracteres.");

        RuleFor(x => x.Data.Items)
            .NotNull()
            .WithMessage("Os itens do pedido não podem ser nulos.")
            .NotEmpty()
            .WithMessage("Os itens do pedido não podem estar vazios.");
        
        RuleFor(x => x.Data.Items)
            .SetValidator(new OrderItemsValidator());
    }
}
