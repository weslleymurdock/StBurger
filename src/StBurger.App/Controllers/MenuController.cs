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
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateMenuItemRequest request)
    {
        var command = new CreateMenuItemCommand(request);
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(Create), new { id = result.Id }, new BaseResponse<CreateMenuItemResponse>(result));
    }


    /// <summary>
    /// Remove um item do menu utilizando seu id
    /// </summary>
    /// <param name="id">O id do item do menu a ser removido</param>
    /// <returns>Uma instância de <see cref="IActionResult"/> com status http 204 no content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute(Name = "id")] string id)
    {
        var command = new DeleteMenuItemCommand(id);
        await mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Busca todos os items do menu
    /// </summary>
    /// <returns>Uma instancia de <see cref="IActionResult"/> com a lista de items do menu no corpo da response e contendo status http 200 ok</returns>
    [HttpGet]
    [ProducesResponseType(typeof(BaseResponse<MenuItemsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status405MethodNotAllowed)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status408RequestTimeout)]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetMenuQuery();
        var result = await mediator.Send(query);
        return Ok(new BaseResponse<MenuItemsResponse>(result));
    }

    /// <summary>
    /// Busca um item específico do menu pelo seu id
    /// </summary>
    /// <param name="id">O id do item do menu</param>
    /// <returns>Uma instancia de <see cref="IActionResult"/> com o item do menu no corpo da response e contendo status http 200 ok</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BaseResponse<MenuItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute(Name = "id")] string id)
    {
        var query = new GetMenuItemQuery(id);
        var result = await mediator.Send(query);
        return Ok(new BaseResponse<MenuItemResponse>(result));
    }
     
    /// <summary>
    /// Atualiza a descrição de um item do menu
    /// </summary>
    /// <param name="id">Id do item</param>
    /// <param name="description">Descrição do item</param>
    /// <returns>Uma instância de <see cref="IActionResult"/> contendo status http 204 no content caso a atualização seja bem sucedida"/></returns>
    [HttpPatch("description")]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> PatchDescription([FromBody] s request)
    {
        var command = new PatchMenuItemDescriptionCommand(request.Id, request.Description);
        var result = await mediator.Send(command);
        return NoContent();
    }
     
    /// <summary>
    /// Atualiza o nome de um item do menu
    /// </summary>
    /// <param name="id">id do item</param>
    /// <param name="name">nome do item</param>
    /// <returns>Uma instância de <see cref="IActionResult"/> contendo status http 204 no content caso a atualização seja bem sucedida"/></returns>
    [HttpPatch("name")]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> PatchName([FromRoute(Name = "id")] string id, [FromRoute(Name = "name")] string name)
    {
        var command = new PatchMenuItemNameCommand(id, name);
        var result = await mediator.Send(command);
        return NoContent();
    }
     
    /// <summary>
    /// Atualiza um item do menu
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Uma instância de <see cref="IActionResult"/> com o item do menu atualizado no corpo da response e contendo status http 200 ok"/></returns>
    [HttpPut]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromBody] UpdateMenuItemRequest request)
    {
        var command = new UpdateMenuItemCommand(request);
        var result = await mediator.Send(command);
        return Ok(new BaseResponse<UpdateMenuItemResponse>(result));
    }

}
