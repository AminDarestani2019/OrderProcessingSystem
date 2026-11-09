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

        var inventory = await _inventoryRepository.GetByProductIdAsync(
            request.ProductId,
            cancellationToken);

        if (inventory == null)
            return false;

        if (inventory.AvailableQuantity < request.Quantity)
            return false;

        inventory.AvailableQuantity -= request.Quantity;

        await _inventoryRepository.SaveChangesAsync(
            cancellationToken);

        return true;
    }
}
