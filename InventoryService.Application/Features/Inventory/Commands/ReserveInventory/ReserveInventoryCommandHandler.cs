using InventoryService.Application.Interfaces;
using MediatR;

namespace InventoryService.Application.Features.Inventory.Commands.ReserveInventory;

public class ReserveInventoryCommandHandler
    : IRequestHandler<ReserveInventoryCommand, bool>
{
    private readonly IInventoryRepository _inventoryRepository;

    public ReserveInventoryCommandHandler(
        IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task<bool> Handle(
        ReserveInventoryCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Quantity <= 0)
            return false;

	return await _inventoryRepository.TryReserveAsync(
            request.ProductId,
            request.Quantity,
            cancellationToken);
    }
}
