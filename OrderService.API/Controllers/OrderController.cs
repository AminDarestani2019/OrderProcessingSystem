using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Features.Orders.Commands.CreateOrder;
using OrderService.Application.Features.Orders.Queries.GetOrderById;

namespace OrderService.API.Controllers;

[ApiController]
[Route("orders")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        var orderId = await _mediator.Send(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetOrderById),
            new { id = orderId },
            new { Id = orderId });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrderById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var order = await _mediator.Send(
            new GetOrderByIdQuery(id),
            cancellationToken);

        if (order == null)
            return NotFound();

        return Ok(order);
    }
}