using StBurger.Domain.Menu.Entities;

namespace StBurger.Application.Menu.Requests;

public sealed record CreateMenuItemRequest(string Name, decimal Price, string Description, string Type)
{
    public static MenuItem ToEntity(CreateMenuItemRequest request)
    {
        return request.Type.ToLower() switch
        {
            "sandwich" or "lanche" => new Sandwich(request.Name, request.Description, request.Price),
            "drink" or "bebida" => new Drink(request.Name, request.Description, request.Price),
            "side" or "acompanhamento" => new Side(request.Name, request.Description, request.Price),

            _ => throw new ArgumentException($"Invalid type: {request.Type}")
        };
    }
}
