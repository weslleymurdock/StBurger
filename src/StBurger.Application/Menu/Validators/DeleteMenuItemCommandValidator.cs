namespace StBurger.Application.Menu.Validators;

public sealed class DeleteMenuItemCommandValidator : AbstractValidator<DeleteMenuItemCommand>
{
    public DeleteMenuItemCommandValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .NotEmpty()
            .WithMessage("A requisição não pode ser nula ou estar vazia");

        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty()
            .WithMessage("O Id do item de menu é obrigatório e não pode ser nulo ou vazio.")
            .Must(id => Guid.TryParse(id, out _))
            .WithMessage("O Id do item de menu deve ser um GUID válido.");
    }
}
