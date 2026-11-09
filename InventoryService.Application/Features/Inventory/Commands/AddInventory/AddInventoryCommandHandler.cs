using InventoryService.Application.Interfaces;
using InventoryService.Domain;
using MediatR;

namespace InventoryService.Application.Features.Inventory.Commands.AddInventory;

public class AddInventoryCommandHandler
    : IRequestHandler<AddInventoryCommand, Guid>
{
    private readonly IInventoryRepository _inventoryRepository;

    public AddInventoryCommandHandler(
        IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task<Guid> Handle(
        AddInventoryCommand request,
        CancellationToken cancellationToken)
    {
        if (request.ProductId <= 0)
            throw new ArgumentException("Invalid ProductId.");

        if (request.AvailableQuantity < 0)
            throw new ArgumentException("Invalid AvailableQuantity.");

        var inventory = new ProductInventory
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            AvailableQuantity = request.AvailableQuantity
        };

        await _inventoryRepository.AddAsync(
            inventory,
            cancellationToken);

        await _inventoryRepository.SaveChangesAsync(
            cancellationToken);

        return inventory.Id;
    }
}