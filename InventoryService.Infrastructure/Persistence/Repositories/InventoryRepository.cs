using InventoryService.Application.Interfaces;
using InventoryService.Domain;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Infrastructure.Persistence.Repositories;

public class InventoryRepository: IInventoryRepository
{
    private readonly InventoryDbContext _context;

    public InventoryRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        ProductInventory inventory,
        CancellationToken cancellationToken = default)
    {
        await _context.ProductInventories.AddAsync(
            inventory,
            cancellationToken);
    }

    public async Task<ProductInventory?> GetByProductIdAsync(
        int productId,
        CancellationToken cancellationToken = default)
    {
        return await _context.ProductInventories
            .FirstOrDefaultAsync(
                x => x.ProductId == productId,
                cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
