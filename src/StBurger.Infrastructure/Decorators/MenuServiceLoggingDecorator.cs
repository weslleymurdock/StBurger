using StBurger.Application.Core.Abstractions.Services;
using StBurger.Application.Menu.Requests;
using StBurger.Application.Menu.Responses;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StBurger.Infrastructure.Decorators;

public sealed class MenuServiceLoggingDecorator(
    IMenuService inner,
    ILogger<MenuServiceLoggingDecorator> logger)
    : IMenuService
{
    public async Task<CreateMenuItemResponse> CreateMenuItemAsync(CreateMenuItemRequest data, CancellationToken ct)
    {
        logger.LogInformation("Start MenuService.CreateMenuItemAsync");

        var result = await inner.CreateMenuItemAsync(data, ct);

        logger.LogInformation("End MenuService.CreateMenuItemAsync");

        return result;
    }

    public async Task<Unit> DeleteMenuItemAsync(string id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start MenuService.DeleteMenuItemAsync");

        var result = await inner.DeleteMenuItemAsync(id, cancellationToken);

        logger.LogInformation("End MenuService.DeleteMenuItemAsync");

        return result;
    }

    public async Task<IReadOnlyCollection<MenuItemResponse>> GetAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Start MenuService.GetAsync (collection)");

        var result = await inner.GetAsync(cancellationToken);

        logger.LogInformation("End MenuService.GetAsync (collection)");

        return result;
    }

    public async Task<MenuItemResponse?> GetAsync(string id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start MenuService.GetAsync (id)");

        var result = await inner.GetAsync(id, cancellationToken);

        logger.LogInformation("End MenuService.GetAsync (id)");

        return result;
    }

    public async Task<Unit> PatchMenuItemDescriptionAsync((string id, string description) data, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start MenuService.PatchMenuItemDescriptionAsync");

        var result = await inner.PatchMenuItemDescriptionAsync(data, cancellationToken);

        logger.LogInformation("End MenuService.PatchMenuItemDescriptionAsync");

        return result;
    }

    public async Task<Unit> PatchMenuItemNameAsync((string id, string name) data, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start MenuService.PatchMenuItemNameAsync");

        var result = await inner.PatchMenuItemNameAsync(data, cancellationToken);

        logger.LogInformation("End MenuService.PatchMenuItemNameAsync");

        return result;
    }

    public async Task<Unit> PatchMenuItemPriceAsync((string id, decimal price) data, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start MenuService.PatchMenuItemPriceAsync");

        var result = await inner.PatchMenuItemPriceAsync(data, cancellationToken);

        logger.LogInformation("End MenuService.PatchMenuItemPriceAsync");

        return result;
    }

    public async Task<UpdateMenuItemResponse> UpdateMenuItemAsync(UpdateMenuItemRequest data, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start MenuService.UpdateMenuItemAsync");

        var result = await inner.UpdateMenuItemAsync(data, cancellationToken);

        logger.LogInformation("End MenuService.UpdateMenuItemAsync");

        return result;
    }
}