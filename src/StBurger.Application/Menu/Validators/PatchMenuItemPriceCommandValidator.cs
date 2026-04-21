namespace StBurger.Application.Menu.Validators;

public sealed class PatchMenuItemPriceCommandValidator : AbstractValidator<PatchMenuItemPriceCommand>
{
    public PatchMenuItemPriceCommandValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .NotEmpty()
            .WithMessage("A atualização parcial não pode ser nula ou estar vazia");

        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty()
            .WithMessage("O Id do item de menu é obrigatório e não pode ser nulo ou vazio.")
            .Must(id => Guid.TryParse(id, out _))
            .WithMessage("O Id do item de menu deve ser um GUID válido.");

        RuleFor(x => x.Price)
            .NotEmpty()
            .NotNull()
            .WithMessage("O preço do item de menu é obrigatório e não pode ser nulo ou vazio.")
            .GreaterThan(0)
            .WithMessage("O preço do item de menu deve ser maior que zero.");
    }
}
