namespace StBurger.App.Controllers;

[ApiController]
[Route(Application.Core.Constants.Routes.Order.Base)]
[Tags(Application.Core.Constants.Routes.Order.Tag)]
public class OrderController(IMediator mediator) : ControllerBase
{

    /// <summary>
    /// Cria um item para o Order
    /// </summary>
    /// <param name="request">Corpo do item do Order a ser criado</param>
    /// <returns>uma response do tipo <see cref="BaseResponse{CreateOrderItemResponse}"/> contendo o item do Order criado e status http 201 created</returns>
    [HttpPost("{id}/items")]
    [ProducesResponseType(typeof(BaseResponse<OrderResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> AddItem([FromRoute(Name="id")] string id, [FromBody] NewOrderItemRequest request)
    {
        var command = new AddOrderItemCommand(id, request);
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(Create), result, new BaseResponse<OrderResponse>(result));
    }

    /// <summary>
    /// Cria um pedido
    /// </summary>
    /// <param name="request">Corpo do item do Order a ser criado</param>
    /// <returns>uma response do tipo <see cref="BaseResponse{CreateOrderItemResponse}"/> contendo o item do Order criado e status http 201 created</returns>
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<CreateOrderResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        var command = new CreateOrderCommand(request);
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(Create), result, new BaseResponse<CreateOrderResponse>(result));
    }

    /// <summary>
    /// Busca todos os pedidos
    /// </summary>
    /// <returns>Uma instancia de <see cref="IActionResult"/> com a lista de items do Order no corpo da response e contendo status http 200 ok</returns>
    [HttpGet]
    [ProducesResponseType(typeof(BaseResponse<OrdersResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get()
    {
        var query = new GetOrdersQuery();
        var result = await mediator.Send(query);
        return Ok(new BaseResponse<OrdersResponse>(result));
    }

    /// <summary>
    /// Busca um item específico do Order pelo seu id
    /// </summary>
    /// <param name="id">O id do item do Order</param>
    /// <returns>Uma instancia de <see cref="IActionResult"/> com o item do Order no corpo da response e contendo status http 200 ok</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BaseResponse<OrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute(Name = "id")] string id)
    {
        var query = new GetOrderByIdQuery(id);
        var result = await mediator.Send(query);
        return Ok(new BaseResponse<OrderResponse>(result));
    }



    /// <summary>
    /// Atualiza um item do Order
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Uma instância de <see cref="IActionResult"/> com o item do Order atualizado no corpo da response e contendo status http 200 ok"/></returns>
    [HttpPut]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromBody] UpdateOrderRequest request)
    {
        var command = new UpdateOrderCommand(request);
        var result = await mediator.Send(command);
        return Ok(new BaseResponse<UpdateOrderResponse>(result));
    }

    /// <summary>
    /// Remove um pedido utilizando seu id
    /// </summary>
    /// <param name="id">O id do pedido a ser removido</param>
    /// <returns>Uma instância de <see cref="IActionResult"/> com status http 204 no content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<ProblemDetails>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute(Name = "id")] string id)
    {
        var command = new DeleteOrderCommand(id);
        await mediator.Send(command);
        return NoContent();
    }
}
