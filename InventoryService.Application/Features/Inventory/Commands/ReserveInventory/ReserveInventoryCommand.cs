using MediatR;

namespace InventoryService.Application.Features.Inventory.Commands.ReserveInventory;

public record ReserveInventoryCommand(
    int ProductId,
    int Quantity
) : IRequest<bool>;
