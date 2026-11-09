using InventoryService.Application.Features.Inventory.Commands.AddInventory;
using InventoryService.Application.Features.Inventory.Commands.ReserveInventory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.API.Controllers;

[ApiController]
[Route("inventory")]
public class InventoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddInventory(
    [FromBody] AddInventoryCommand command,
    CancellationToken cancellationToken)
    {
        var inventoryId = await _mediator.Send(
            command,
            cancellationToken);

        return Created(
            $"/inventory/{inventoryId}",
            new
            {
                Id = inventoryId,
                Message = "Inventory created successfully."
            });
    }

    [HttpPost("reserve")]
    public async Task<IActionResult> Reserve(
        [FromBody] ReserveInventoryCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command,
            cancellationToken);

        if (!result)
        {
            return Conflict(new
            {
                Message = "Product not found or insufficient inventory."
            });
        }

        return Ok(new
        {
            Message = "Inventory reserved successfully."
        });
    }
}
