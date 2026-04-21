using StBurger.Domain.Menu.Enums;

namespace StBurger.Application.Menu.Validators;

public sealed class CreateMenuItemCommandValidator : AbstractValidator<CreateMenuItemCommand>
{
    public CreateMenuItemCommandValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .NotEmpty()
            .WithMessage("Os dados do item de menu são obrigatórios e não podem ser nulos ou vazios.");

        RuleFor(x => x.Data)
            .NotNull()
            .NotEmpty()
            .WithMessage("O objeto da requisição do item de menu é obrigatório e não pode ser nulo ou vazio.");

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

        RuleFor(x => x.Data.Type)
            .NotNull()
            .NotEmpty()
            .WithMessage("O tipo do item de menu é obrigatório e não pode ser nulo ou vazio.")
            .Must(x => Enum.TryParse<MenuItemType>(x, true, out _))
            .WithMessage("O tipo do item de menu deve ser um valor válido.");

        RuleFor(x => x.Data.Price)
            .NotNull()
            .WithMessage("O preço do item de menu é obrigatório e não pode ser nulo.")
            .GreaterThan(0)
            .WithMessage("O preço do item de menu deve ser maior que zero.");
    }
}
