namespace StBurger.Application.Core.Abstractions.Services;

/// <summary>
/// Serviço de aplicação responsável por gerenciar as operações do cardápio
/// </summary>
public interface IMenuService
{
    /// <summary>
    /// Cria um novo item no cardápio.
    /// </summary>
    /// <param name="data">Objeto da requisição</param>
    /// <param name="cancellationToken">Token de cancelamento da operação</param>
    /// <returns>Um objeto do tipo <see cref="CreateMenuItemResponse"/> com dados do item criado no cardápio</returns>
    /// <exception cref="InvalidCastException">Lançada quando o tipo do item é inválido</exception>
    /// <exception cref="ArgumentException">Lançada quando ocorre um erro ao criar o item do cardápio</exception>
    Task<CreateMenuItemResponse> CreateMenuItemAsync(CreateMenuItemRequest data, CancellationToken cancellationToken);
    
    /// <summary>
    /// Remove um item do cardápio com base no identificador único do item.
    /// </summary>
    /// <param name="id">Identificador único</param>
    /// <param name="cancellationToken">token de cancelamento da operação</param>
    /// <returns>Uma instancia da estrutura <see cref="Unit"/></returns>
    Task<Unit> DeleteMenuItemAsync(string id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Retorna todos os itens do cardápio.
    /// </summary>
    /// <param name="id">Identificador único</param>
    /// <param name="cancellationToken">token de cancelamento da operação</param>
    /// <returns>Uma coleção de entidades do tipo <see cref="MenuItemResponse"/> com os dados de todos os itens do cardápio</returns>
    /// <exception cref="KeyNotFoundException">Lançada quando nenhum item do cardápio é encontrado</exception>
    Task<IReadOnlyCollection<MenuItemResponse>> GetAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Retorna um item do cardápio com base no identificador único do item.
    /// </summary>
    /// <param name="id">Identificador único</param>
    /// <param name="cancellationToken">token de cancelamento da operação</param>
    /// <returns>Uma entidade do tipo <see cref="MenuItemResponse?"/> com os dados do item do cardápio referente ao id provido caso encontrado</returns>
    /// <exception cref="KeyNotFoundException">Lançada quando o item do cardápio não é encontrado</exception>
    Task<MenuItemResponse?> GetAsync(string id, CancellationToken cancellationToken);

    /// <summary>
    /// <summary>
    /// Atualiza apenas o nome de um item do cardápio
    /// </summary>
    /// <param name="data">tupla composta por id e nome</param>
    /// <param name="cancellationToken">token de cancelamento da operação</param>
    /// <returns>Uma entidade do tipo <see cref="MenuItem"/> atualizada</returns>
    /// <exception cref="KeyNotFoundException">Lançada quando o item do cardápio não é encontrado</exception>
    Task<Unit> PatchMenuItemNameAsync((string id, string name) data, CancellationToken cancellationToken);

    /// <summary>
    /// Atualiza apenas a descrição de um item do cardápio
    /// </summary>
    /// <param name="data">tupla composta por id e descrição</param>
    /// <param name="cancellationToken">token de cancelamento da operação</param>
    /// <returns>Uma entidade do tipo <see cref="MenuItem"/> atualizada</returns>
    /// <exception cref="KeyNotFoundException">Lançada quando o item do cardápio não é encontrado</exception>
    Task<Unit> PatchMenuItemDescriptionAsync((string id, string description) data, CancellationToken cancellationToken);

    /// <summary>
    /// Atualiza apenas o preço do item do cardápio
    /// </summary>
    /// <param name="data">tupla composta por id e preço</param>
    /// <param name="cancellationToken">token de cancelamento da operação</param>
    /// <returns>Uma entidade do tipo <see cref="MenuItem"/> atualizada</returns>
    /// <exception cref="KeyNotFoundException">Lançada quando o item do cardápio não é encontrado</exception>
    Task<Unit> PatchMenuItemPriceAsync((string id, decimal price) data, CancellationToken cancellationToken);

    /// <summary>
    /// Atualiza um item do cardápio com base no identificador único do item, 
    /// alterando a entidade inteira do item do cardápio
    /// </summary>
    /// <param name="data">Objeto da requisição</param>
    /// <param name="cancellationToken">Token de cancelamento da operação</param>
    /// <returns>Um objeto do tipo <see cref="UpdateMenuItemResponse"/> com o resultado da operação</returns>
    Task<UpdateMenuItemResponse> UpdateMenuItemAsync(UpdateMenuItemRequest data, CancellationToken cancellationToken);
}
