namespace StBurger.Application.Menu.Validators;

public sealed class PatchMenuItemNameCommandValidator : AbstractValidator<PatchMenuItemNameCommand>
{
    public PatchMenuItemNameCommandValidator()
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
        
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("O nome do item de menu é obrigatório e não pode ser nulo ou vazio.")
            .MinimumLength(2)
            .WithMessage("O nome do item de menu deve ter pelo menos 2 caracteres.")
            .MaximumLength(96)
            .WithMessage("O nome do item de menu deve ter no máximo 96 caracteres.");
    }
}
