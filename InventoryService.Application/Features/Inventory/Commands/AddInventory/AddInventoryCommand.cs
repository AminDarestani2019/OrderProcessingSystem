using MediatR;

namespace InventoryService.Application.Features.Inventory.Commands.AddInventory;

public record AddInventoryCommand(
    int ProductId,
    int AvailableQuantity
) : IRequest<Guid>;
