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
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> AddItem([FromRoute(Name="id")] string id, [FromBody] NewOrderItemRequest request)
    {
        var command = new AddOrderItemCommand(id, request);
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(Create), result, BaseResponse<OrderResponse>.Ok("Order item added successfully", 201, result));
    }

    /// <summary>
    /// Cria um pedido
    /// </summary>
    /// <param name="request">Corpo do item do Order a ser criado</param>
    /// <returns>uma response do tipo <see cref="BaseResponse{CreateOrderItemResponse}"/> contendo o item do Order criado e status http 201 created</returns>
    [HttpPost]
    [ProducesResponseType<BaseResponse<CreateOrderResponse>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        var command = new CreateOrderCommand(request);
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(Create), result, BaseResponse<CreateOrderResponse>.Ok("Order created successfully", 201, result));
    }

    /// <summary>
    /// Busca todos os pedidos
    /// </summary>
    /// <returns>Uma instancia de <see cref="IActionResult"/> com a lista de items do Order no corpo da response e contendo status http 200 ok</returns>
    [HttpGet]
    [ProducesResponseType<BaseResponse<OrdersResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get()
    {
        var query = new GetOrdersQuery();
        var result = await mediator.Send(query);
        return Ok(BaseResponse<OrdersResponse>.Ok("Orders retrieved successfully", 200, result));
    }

    /// <summary>
    /// Busca um item específico do Order pelo seu id
    /// </summary>
    /// <param name="id">O id do item do Order</param>
    /// <returns>Uma instancia de <see cref="IActionResult"/> com o item do Order no corpo da response e contendo status http 200 ok</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BaseResponse<OrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute(Name = "id")] string id)
    {
        var query = new GetOrderByIdQuery(id);
        var result = await mediator.Send(query);
        return Ok(BaseResponse<OrderResponse>.Ok("Order retrieved successfully", 200, result));
    }



    /// <summary>
    /// Atualiza um item do Order
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Uma instância de <see cref="IActionResult"/> com o item do Order atualizado no corpo da response e contendo status http 200 ok"/></returns>
    [HttpPut]
    [ProducesResponseType<BaseResponse<UpdateOrderResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromBody] UpdateOrderRequest request)
    {
        var command = new UpdateOrderCommand(request);
        var result = await mediator.Send(command);
        return Ok(BaseResponse<UpdateOrderResponse>.Ok("Order updated successfully", 200, result));
    }

    /// <summary>
    /// Remove um pedido utilizando seu id
    /// </summary>
    /// <param name="id">O id do pedido a ser removido</param>
    /// <returns>Uma instância de <see cref="IActionResult"/> com status http 204 no content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType<BaseResponse>(StatusCodes.Status202Accepted)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute(Name = "id")] string id)
    {
        var command = new DeleteOrderCommand(id);
        await mediator.Send(command);
        return Accepted(BaseResponse.Ok("Order deleted successfully", 202));
    }
}
