namespace StBurger.Application.Menu.Validators;

public sealed class UpdateMenuItemCommandValidator : AbstractValidator<UpdateMenuItemCommand>
{
    public UpdateMenuItemCommandValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .NotEmpty()
            .WithMessage("Os dados do item de menu são obrigatórios e não podem ser nulos ou vazios.");
        
        RuleFor(x => x.Data)
            .NotNull()
            .NotEmpty()
            .WithMessage("O objeto da requisição do item de menu é obrigatório e não pode ser nulo ou vazio.");
        
        RuleFor(x => x.Data.Id)
            .NotNull()
            .NotEmpty()
            .WithMessage("O Id do item de menu é obrigatório e não pode ser nulo ou vazio.")
            .Must(id => Guid.TryParse(id, out _))
            .WithMessage("O Id do item de menu deve ser um GUID válido.");

        RuleFor(x => x.Data.Description)
            .MaximumLength(1024)
            .WithMessage("A descrição do item de menu deve ter no máximo 1024 caracteres.");

        RuleFor(x => x.Data.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("O nome do item de menu é obrigatório e não pode ser nulo ou vazio.")
            .MinimumLength(2)
            .WithMessage("O nome do item de menu deve ter pelo menos 2 caracteres.")
            .MaximumLength(96)
            .WithMessage("O nome do item de menu deve ter no máximo 96 caracteres.");
        
        RuleFor(x => x.Data.Price)
            .NotNull()
            .WithMessage("O preço do item de menu é obrigatório e não pode ser nulo.")
            .GreaterThan(0)
            .WithMessage("O preço do item de menu deve ser maior que zero.");
    }
}
