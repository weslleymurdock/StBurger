namespace StBurger.App.Controllers;

[ApiController]
[Route(Application.Core.Constants.Routes.Menu.Base)]
[Tags(Application.Core.Constants.Routes.Menu.Tag)]

public class MenuController(IMediator mediator) : ControllerBase
{

    /// <summary>
    /// Cria um item para o menu
    /// </summary>
    /// <param name="request">Corpo do item do menu a ser criado</param>
    /// <returns>uma response do tipo <see cref="BaseResponse{CreateMenuItemResponse}"/> contendo o item do menu criado e status http 201 created</returns>
    [HttpPost]
    [ProducesResponseType<BaseResponse<CreateMenuItemResponse>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateMenuItemRequest request)
    {
        var command = new CreateMenuItemCommand(request);
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(Create), new { id = result.Id }, BaseResponse<CreateMenuItemResponse>.Ok("Menu item created successfully", 201, result));
    }


    /// <summary>
    /// Remove um item do menu utilizando seu id
    /// </summary>
    /// <param name="id">O id do item do menu a ser removido</param>
    /// <returns>Uma instância de <see cref="IActionResult"/> com status http 204 no content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType<BaseResponse>(StatusCodes.Status202Accepted)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute(Name = "id")] string id)
    {
        var command = new DeleteMenuItemCommand(id);
        await mediator.Send(command);
        return Accepted(BaseResponse.Ok("Menu item deleted successfully", 202));
    }

    /// <summary>
    /// Busca todos os items do menu
    /// </summary>
    /// <returns>Uma instancia de <see cref="IActionResult"/> com a lista de items do menu no corpo da response e contendo status http 200 ok</returns>
    [HttpGet]
    [ProducesResponseType<BaseResponse<MenuItemsResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status405MethodNotAllowed)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status408RequestTimeout)]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetMenuQuery();
        var result = await mediator.Send(query);
        return Ok(BaseResponse<MenuItemsResponse>.Ok("Menu items retrieved successfully", 200, result));
    }

    /// <summary>
    /// Busca um item específico do menu pelo seu id
    /// </summary>
    /// <param name="id">O id do item do menu</param>
    /// <returns>Uma instancia de <see cref="IActionResult"/> com o item do menu no corpo da response e contendo status http 200 ok</returns>
    [HttpGet("{id}")]
    [ProducesResponseType<BaseResponse<MenuItemResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute(Name = "id")] string id)
    {
        var query = new GetMenuItemQuery(id);
        var result = await mediator.Send(query);
        return Ok(BaseResponse<MenuItemResponse>.Ok("Menu item retrieved successfully", 200, result));
    }
     
    /// <summary>
    /// Atualiza a descrição de um item do menu
    /// </summary>
    /// <param name="id">Id do item</param>
    /// <param name="description">Descrição do item</param>
    /// <returns>Uma instância de <see cref="IActionResult"/> contendo status http 204 no content caso a atualização seja bem sucedida"/></returns>
    [HttpPatch("description")]
    [ProducesResponseType<BaseResponse>(StatusCodes.Status202Accepted)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> PatchDescription([FromBody] PatchMenuItemDescriptionRequest request)
    {
        var command = new PatchMenuItemDescriptionCommand(request.Id, request.Description);
        var result = await mediator.Send(command);
        return Accepted(BaseResponse.Ok("Menu item description updated successfully", 202));
    }
     
    /// <summary>
    /// Atualiza o nome de um item do menu
    /// </summary>
    /// <param name="id">id do item</param>
    /// <param name="name">nome do item</param>
    /// <returns>Uma instância de <see cref="IActionResult"/> contendo status http 204 no content caso a atualização seja bem sucedida"/></returns>
    [HttpPatch("name")]
    [ProducesResponseType<BaseResponse>(StatusCodes.Status202Accepted)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> PatchName([FromBody] PatchMenuItemNameRequest request)
    {
        var command = new PatchMenuItemNameCommand(request.Id, request.Name);
        _ = await mediator.Send(command);
        return Accepted(BaseResponse.Ok("Menu item name updated successfully", 202));
    }
     
    /// <summary>
    /// Atualiza o preço de um item do menu
    /// </summary>
    /// <param name="id">id do item</param>
    /// <param name="price">preço do item</param>
    /// <returns>Uma instância de <see cref="IActionResult"/> contendo status http 204 no content caso a atualização seja bem sucedida"/></returns>
    [HttpPatch("price")]
    [ProducesResponseType<BaseResponse>(StatusCodes.Status202Accepted)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> PatchPrice([FromBody] PatchMenuItemPriceRequest request)
    {
        var command = new PatchMenuItemPriceCommand(request.Id, request.Price);
        _ = await mediator.Send(command);
        return Accepted(BaseResponse.Ok("Menu item price updated successfully", 202));
    }
     
    /// <summary>
    /// Atualiza um item do menu
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Uma instância de <see cref="IActionResult"/> com o item do menu atualizado no corpo da response e contendo status http 200 ok"/></returns>
    [HttpPut]
    [ProducesResponseType<BaseResponse<UpdateMenuItemResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromBody] UpdateMenuItemRequest request)
    {
        var command = new UpdateMenuItemCommand(request);
        var result = await mediator.Send(command);
        return Ok(BaseResponse<UpdateMenuItemResponse>.Ok("Menu item updated successfully", 200, result));
    }

}
