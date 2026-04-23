using Microsoft.Extensions.Logging;
using StBurger.Domain.Menu.Entities;
using System.Linq.Expressions;

namespace StBurger.Application.Menu.Services;

/// <summary>
/// Implementação do serviço do cardápio.
/// </summary>
/// <param name="sandwichRepository">Repositório da entidade <see cref="Sandwich"/></param>
/// <param name="sideRepository">Repositório da entidade <see cref="Side"/></param>
/// <param name="drinkRepository">Repositório da entidade <see cref="Drink"/></param>
/// <param name="uow">Unidade de trabalho para gerenciar transações</param>
/// <param name="logger">Logger para registrar informações e erros</param>
public sealed class MenuService(
    IRepository<Sandwich, string> sandwichRepository,
    IRepository<Side, string> sideRepository,
    IRepository<Drink, string> drinkRepository,
    IUnitOfWork<string> uow,
    ILogger<MenuService> logger) : IMenuService
{
    /// <inheritdoc/>
    public async Task<CreateMenuItemResponse> CreateMenuItemAsync(CreateMenuItemRequest data, CancellationToken cancellationToken)
    {
        try
        {
            var type = data.Type.ToLower();

            MenuItem item = type switch
            {
                "sandwich" or "sanduiche" => await sandwichRepository.AddAsync(new Sandwich(data.Name, data.Description, data.Price)),
                "drink" or "bebida" => await drinkRepository.AddAsync(new Drink(data.Name, data.Description, data.Price)),
                "side" or "acompanhamento" => await sideRepository.AddAsync(new Side(data.Name, data.Description, data.Price)),
                _ => throw new ArgumentException("Invalid menu item type"),
            };
            await uow.Commit(cancellationToken);
            return new CreateMenuItemResponse(item.Id, item.Name, item.Price, item.Description);
        }
        catch (Exception e)
        {
            await uow.Rollback();
            logger.LogWarning(e, "Failed to create menu item: {Message}", e.Message);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<Unit> DeleteMenuItemAsync(string id, CancellationToken cancellationToken)
    {
        try
        {
            var menuItem = (await sandwichRepository.GetByIdAsync(id) as MenuItem
                ?? await drinkRepository.GetByIdAsync(id) as MenuItem
                ?? await sideRepository.GetByIdAsync(id) as MenuItem)
                ?? throw new KeyNotFoundException("Menu item not found");

            switch (menuItem)
            {
                case Sandwich sandwich:
                    await sandwichRepository.DeleteAsync(sandwich);
                    break;
                case Drink drink:
                    await drinkRepository.DeleteAsync(drink);
                    break;
                case Side side:
                    await sideRepository.DeleteAsync(side);
                    break;
            }
            await uow.Commit(cancellationToken);
            return Unit.Value;
        }
        catch (Exception e)
        {
            await uow.Rollback();
            logger.LogWarning(e, "Failed to delete menu item: {Message}", e.Message);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<MenuItemResponse>> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            List<MenuItem> items = [];
            items.AddRange(await sandwichRepository.GetAllAsync());
            items.AddRange(await drinkRepository.GetAllAsync());
            items.AddRange(await sideRepository.GetAllAsync());

            return items.Any(i => i is not null)
                ? [.. items.Select(MenuItemResponse.FromEntity)]
                : throw new KeyNotFoundException("No menu items found");
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "Failed to retrieve menu items: {Message}", e.Message);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<MenuItemResponse?> GetAsync(string id, CancellationToken cancellationToken)
    {
        try
        {
            var item = await uow.Repository<MenuItem>().GetByIdAsync(id);

            return MenuItemResponse.FromEntity(item);
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "Failed to retrieve menu item: {Message}", e.Message);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<Unit> PatchMenuItemDescriptionAsync((string id, string description) data, CancellationToken cancellationToken)
    {
        try
        {
            var item = await UpdateMenuItem((data.id, name: string.Empty, data.description, price: decimal.Zero));
            switch (item)
            {
                case Sandwich sandwich:
                    await sandwichRepository.UpdateAsync(sandwich);
                    break;
                case Drink drink:
                    await drinkRepository.UpdateAsync(drink);
                    break;
                case Side side:
                    await sideRepository.UpdateAsync(side);
                    break;
            }
            await uow.Commit(cancellationToken);
            return Unit.Value;
        }
        catch (Exception e)
        {
            await uow.Rollback();
            logger.LogWarning(e, "Failed to update menu item description: {Message}", e.Message);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<Unit> PatchMenuItemNameAsync((string id, string name) data, CancellationToken cancellationToken)
    {
        try
        {
            var item = await UpdateMenuItem((data.id, data.name, description: string.Empty, price: decimal.Zero));
            switch (item)
            {
                case Sandwich sandwich:
                    await sandwichRepository.UpdateAsync(sandwich);
                    break;
                case Drink drink:
                    await drinkRepository.UpdateAsync(drink);
                    break;
                case Side side:
                    await sideRepository.UpdateAsync(side);
                    break;
            }
            await uow.Commit(cancellationToken);

            return Unit.Value;
        }
        catch (Exception e)
        {
            await uow.Rollback();
            logger.LogWarning(e, "Failed to update menu item name: {Message}", e.Message);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<Unit> PatchMenuItemPriceAsync((string id, decimal price) data, CancellationToken cancellationToken)
    {
        try
        {
            var item = await UpdateMenuItem((data.id, name: string.Empty, description: string.Empty, data.price));
            switch (item)
            {
                case Sandwich sandwich:
                    await sandwichRepository.UpdateAsync(sandwich);
                    break;
                case Drink drink:
                    await drinkRepository.UpdateAsync(drink);
                    break;
                case Side side:
                    await sideRepository.UpdateAsync(side);
                    break;
            }
            await uow.Commit(cancellationToken);
            return Unit.Value;
        }
        catch (Exception e)
        {
            await uow.Rollback();
            logger.LogWarning(e, "Failed to update menu item price: {Message}", e.Message);
            throw;
        }
    }

    /// <summary>
    /// Atualiza um item com os dados fornecidos, mantendo os valores inalterados para dados não informados
    /// </summary>
    /// <param name="data">Tupla de dados a serem atualizados</param>
    /// <returns>O item de menu atualizado, não salvo no banco de dados</returns>
    /// <exception cref="KeyNotFoundException">Lançado quando o item não é encontrado no banco de dados</exception>
    private async Task<MenuItem> UpdateMenuItem((string id, string name, string description, decimal? price) data = default!)
    {
        try
        {
            var item = await uow.Repository<MenuItem>().GetByIdAsync(data.id!)
                ?? throw new KeyNotFoundException("Menu item not found");
            item.Update(string.IsNullOrEmpty(data.name) || string.IsNullOrWhiteSpace(data.name) ? item.Name : data.name,string.IsNullOrEmpty(data.description) || string.IsNullOrWhiteSpace(data.description) ? item.Description : data.description, data.price.HasValue && data.price.Value > 0? data.price.Value : item.Price);
            return item;
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "Error updating menu item entity: {Message}", e.Message);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UpdateMenuItemResponse> UpdateMenuItemAsync(UpdateMenuItemRequest data, CancellationToken cancellationToken)
    {
        try
        {
            var item = await UpdateMenuItem((id: data.Id, name: data.Name, description: data.Description, price: data.Price));
            switch (item)
            {
                case Sandwich sandwich:
                    await sandwichRepository.UpdateAsync(sandwich);
                    break;
                case Drink drink:
                    await drinkRepository.UpdateAsync(drink);
                    break;
                case Side side:
                    await sideRepository.UpdateAsync(side);
                    break;
            }
            await uow.Commit(cancellationToken);
            return UpdateMenuItemResponse.FromEntity(item);
        }
        catch (Exception e)
        {
            await uow.Rollback();
            logger.LogWarning(e, "Failed to update menu item: {Message}", e.Message);
            throw;
        }
    }

}
