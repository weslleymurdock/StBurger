namespace StBurger.Application.Menu.Validators;

public sealed class PatchMenuItemDescriptionCommandValidator : AbstractValidator<PatchMenuItemDescriptionCommand>
{
    public PatchMenuItemDescriptionCommandValidator()
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

        RuleFor(x => x.Description)
            .NotNull()
            .NotEmpty()
            .WithMessage("A descrição do item de menu é obrigatória e não pode ser nula ou vazia.")
            .MaximumLength(1024)
            .WithMessage("A descrição do item de menu deve ter no máximo 1024 caracteres.");

    }
}
