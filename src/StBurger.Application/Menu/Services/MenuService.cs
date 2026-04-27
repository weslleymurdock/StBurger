using Microsoft.Extensions.Logging;
using StBurger.Domain.Menu.Entities;
using System.Linq.Expressions;

namespace StBurger.Application.Menu.Services;

/// <summary>
/// Implementação do serviço do cardápio.
/// </summary>
/// <param name="uow.Repository<Sandwich>()">Repositório da entidade <see cref="Sandwich"/></param>
/// <param name="await uow.Repository<Side>()">Repositório da entidade <see cref="Side"/></param>
/// <param name="uow.Repository<Drink>">Repositório da entidade <see cref="Drink"/></param>
/// <param name="uow">Unidade de trabalho para gerenciar transações</param>
/// <param name="logger">Logger para registrar informações e erros</param>
public sealed class MenuService(IUnitOfWork<string> uow) : IMenuService
{
    /// <inheritdoc/>
    public async Task<CreateMenuItemResponse> CreateMenuItemAsync(CreateMenuItemRequest data, CancellationToken cancellationToken)
    {
        var type = data.Type.ToLower();

        MenuItem item = type switch
        {
            "sandwich" or "sanduiche" => await uow.Repository<Sandwich>().AddAsync(new Sandwich(data.Name, data.Description, data.Price)),
            "drink" or "bebida" => await uow.Repository<Drink>().AddAsync(new Drink(data.Name, data.Description, data.Price)),
            "side" or "acompanhamento" => await uow.Repository<Side>().AddAsync(new Side(data.Name, data.Description, data.Price)),
            _ => throw new ArgumentException("Tipo de produto não presente no cardápio"),
        };
        
        return new CreateMenuItemResponse(item.Id, item.Name, item.Price, item.Description);

    }

    /// <inheritdoc/>
    public async Task<Unit> DeleteMenuItemAsync(string id, CancellationToken cancellationToken)
    {
        var menuItem = (await uow.Repository<Sandwich>().GetByIdAsync(id) as MenuItem
            ?? await uow.Repository<Drink>().GetByIdAsync(id) as MenuItem
            ?? await uow.Repository<Side>().GetByIdAsync(id) as MenuItem)
            ?? throw new KeyNotFoundException("Menu item not found");

        switch (menuItem)
        {
            case Sandwich sandwich:
                await uow.Repository<Sandwich>().DeleteAsync(sandwich);
                break;
            case Drink drink:
                await uow.Repository<Drink>().DeleteAsync(drink);
                break;
            case Side side:
                await uow.Repository<Side>().DeleteAsync(side);
                break;
        }
        
        return Unit.Value;

    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<MenuItemResponse>> GetAsync(CancellationToken cancellationToken)
    {
        List<MenuItem> items = [];
        items.AddRange(await uow.Repository<Drink>().GetAllAsync());
        items.AddRange(await uow.Repository<Sandwich>().GetAllAsync());
        items.AddRange(await uow.Repository<Side>().GetAllAsync());

        return items.Any(i => i is not null)
            ? [.. items.Select(MenuItemResponse.FromEntity)]
            : throw new KeyNotFoundException("No menu items found");

    }

    /// <inheritdoc/>
    public async Task<MenuItemResponse?> GetAsync(string id, CancellationToken cancellationToken)
    {
        var item = await uow.Repository<MenuItem>().GetByIdAsync(id);

        return MenuItemResponse.FromEntity(item);
    }

    /// <inheritdoc/>
    public async Task<Unit> PatchMenuItemDescriptionAsync((string id, string description) data, CancellationToken cancellationToken)
    {
        _ = await UpdateMenuItem((data.id, name: string.Empty, data.description, price: decimal.Zero));

        return Unit.Value;
    }

    /// <inheritdoc/>
    public async Task<Unit> PatchMenuItemNameAsync((string id, string name) data, CancellationToken cancellationToken)
    {
        var item = await UpdateMenuItem((data.id, data.name, description: string.Empty, price: decimal.Zero));

        return Unit.Value;
    }

    /// <inheritdoc/>
    public async Task<Unit> PatchMenuItemPriceAsync((string id, decimal price) data, CancellationToken cancellationToken)
    {
        _ = await UpdateMenuItem((data.id, name: string.Empty, description: string.Empty, data.price));

        return Unit.Value;
    }

    /// <summary>
    /// Atualiza um item com os dados fornecidos, mantendo os valores inalterados para dados não informados
    /// </summary>
    /// <param name="data">Tupla de dados a serem atualizados</param>
    /// <returns>O item de menu atualizado, não salvo no banco de dados</returns>
    /// <exception cref="KeyNotFoundException">Lançado quando o item não é encontrado no banco de dados</exception>
    private async Task<MenuItem> UpdateMenuItem((string id, string name, string description, decimal? price) data = default!)
    {
        var item = await uow.Repository<MenuItem>().GetByIdAsync(data.id!)
            ?? throw new KeyNotFoundException("Menu item not found");
     
        item.Update(
            string.IsNullOrEmpty(data.name) || 
            string.IsNullOrWhiteSpace(data.name) ? 
            item.Name : data.name, 
            string.IsNullOrEmpty(data.description) || 
            string.IsNullOrWhiteSpace(data.description) ? 
            item.Description : data.description, 
            data.price.HasValue && data.price.Value > 0 ? 
            data.price.Value : item.Price);
        return item;
    }

    /// <inheritdoc/>
    public async Task<UpdateMenuItemResponse> UpdateMenuItemAsync(UpdateMenuItemRequest data, CancellationToken cancellationToken)
    {
        var item = await UpdateMenuItem((id: data.Id, name: data.Name, description: data.Description, price: data.Price));

        return UpdateMenuItemResponse.FromEntity(item);
    }
}
