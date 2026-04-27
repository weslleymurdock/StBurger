using FluentValidation.Validators;

namespace StBurger.Application.Order.Validators;

public sealed class OrderItemsValidator
    : PropertyValidator<UpdateOrderCommand, IEnumerable<NewOrderItemRequest>>
{
    public override string Name => "OrderItemsValidator";

    public override bool IsValid(
        ValidationContext<UpdateOrderCommand> context,
        IEnumerable<NewOrderItemRequest> value)
    { 
        if (value is null)
        {
            context.AddFailure("Data.Items", "Os itens do pedido não podem ser nulos.");
            return false;
        }

        var items = value.ToList();
         
        if (items.Count <= 0)
        {
            context.AddFailure("Data.Items", "Os itens do pedido não podem estar vazios.");
            return false;
        }

        bool isValid = true;
         
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];

            if (string.IsNullOrWhiteSpace(item.Id) || !Guid.TryParse(item.Id, out _))
            {
                context.AddFailure(
                    $"Data.Items[{i}].Id",
                    $"O id '{item.Id}' não é um GUID válido.");

                isValid = false;
            }
        }
         
        var duplicatedIds = items
            .GroupBy(x => x.Id)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList()
            .Any(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x));

        if (duplicatedIds)
        {
            context.AddFailure(
                "Data.Items",
                $"Itens duplicados não são permitidos. Ids duplicados: {string.Join(", ", duplicatedIds)}");

            isValid = false;
        }

        return isValid;
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Itens inválidos.";
}