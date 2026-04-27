namespace StBurger.Application.Order.Validators;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator(IUnitOfWork<string> uow)
    {
        RuleFor(x => x)
            .NotEmpty()
            .WithMessage("O objeto da requisição não pode ser nulo ou vazio");

        RuleFor(x => x.Data)
            .NotEmpty()
            .WithMessage("O conteudo da requisição não pode estar nulo ou vazio");

        RuleFor(x => x.Data.CustomerName)
            .MaximumLength(96)
            .WithMessage("O nome do cliente não pode conter mais do que 96 caracteres");

        RuleFor(x => x.Data.AttendantName)
            .MaximumLength(96)
            .WithMessage("O nome do atendente não pode conter mais do que 96 caracteres");

        RuleFor(x => x.Data.Items)
            .NotNull()
            .WithMessage("Os items do pedidos não podem serem nulos")
            .NotEmpty()
            .WithMessage("Os items do pedidos não podem serem vazios");
        
        RuleFor(x => x.Data.Items)
            .Must(items => items.Select(i => i.Id).Distinct().Count() == items.Count)
            .WithMessage("Não é permitido adicionar itens duplicados no pedido.");

        RuleFor(x => x.Data.Items)
            .Must(items =>
            {
                var ids = items.Select(i => i.Id).ToList();
                var types = uow
                    .Repository<Domain.Menu.Entities.MenuItem>()
                    .Entities
                    .Where(m => ids.Contains(m.Id))
                    .Select(m => m.GetType().Name)
                    .ToList();
                return types.Distinct().Count() == types.Count;
            })
            .WithMessage("Não é permitido mais de um item da mesma categoria.");
    }
}