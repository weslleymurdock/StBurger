namespace StBurger.Application.Menu.Validators;

public sealed class GetMenuItemQueryValidator : AbstractValidator<GetMenuItemQuery>
{
    public GetMenuItemQueryValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .NotEmpty()
            .WithMessage("A consulta não pode ser nula ou estar vazia");

        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty()
            .WithMessage("O Id do item de menu é obrigatório e não pode ser nulo ou vazio.")
            .Must(id => Guid.TryParse(id, out _))
            .WithMessage("O Id do item de menu deve ser um GUID válido.");
    }
}
