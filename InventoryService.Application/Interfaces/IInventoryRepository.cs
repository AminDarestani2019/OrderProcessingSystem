using InventoryService.Domain;

namespace InventoryService.Application.Interfaces;

public interface IInventoryRepository
{
    Task AddAsync(
        ProductInventory inventory,
        CancellationToken cancellationToken = default);

    Task<ProductInventory?> GetByProductIdAsync(
        int productId,
        CancellationToken cancellationToken = default);
    
    Task<bool> TryReserveAsync(
        int productId,
        int quantity,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
