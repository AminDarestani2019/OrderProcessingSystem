using InventoryService.Application.Interfaces;
using InventoryService.Domain;
using Microsoft.EntityFrameworkCore;
using InventoryService.Application.Features.Inventory.Commands.ReserveInventory;
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

public async Task<OrderProcessingResult> ProcessOrderAsync(
    Guid orderId,
    int productId,
    int quantity,
    CancellationToken cancellationToken = default)
{
    if (quantity <= 0)
        throw new ArgumentOutOfRangeException(nameof(quantity));

    await using var transaction =
        await _context.Database.BeginTransactionAsync(cancellationToken);

    var insertedRows = await _context.Database.ExecuteSqlInterpolatedAsync(
        $"""
        INSERT INTO "ProcessedOrders"
            ("OrderId", "ProductId", "Quantity", "WasReserved", "ProcessedAt")
        VALUES
            ({orderId}, {productId}, {quantity}, FALSE, {DateTime.UtcNow})
        ON CONFLICT ("OrderId") DO NOTHING
        """,
        cancellationToken);

    if (insertedRows == 0)
    {
        var existingOrder = await _context.ProcessedOrders
            .AsNoTracking()
            .SingleAsync(
                x => x.OrderId == orderId,
                cancellationToken);

        if (existingOrder.ProductId != productId ||
            existingOrder.Quantity != quantity)
        {
            throw new InvalidOperationException(
                "OrderId was reused with different order data.");
        }

        await transaction.CommitAsync(cancellationToken);

        return new OrderProcessingResult(
            existingOrder.WasReserved,
            true);
    }

    var affectedRows = await _context.ProductInventories
        .Where(x => x.ProductId == productId &&
                    x.AvailableQuantity >= quantity)
        .ExecuteUpdateAsync(
            setters => setters.SetProperty(
                x => x.AvailableQuantity,
                x => x.AvailableQuantity - quantity),
            cancellationToken);

    var wasReserved = affectedRows == 1;

    await _context.ProcessedOrders
        .Where(x => x.OrderId == orderId)
        .ExecuteUpdateAsync(
            setters => setters.SetProperty(
                x => x.WasReserved,
                wasReserved),
            cancellationToken);

    await transaction.CommitAsync(cancellationToken);

    return new OrderProcessingResult(wasReserved, false);
}

    public async Task<bool> TryReserveAsync(
        int productId,
        int quantity,
        CancellationToken cancellationToken = default)
    {
        if (quantity <= 0)
            return false;

        var affectedRows = await _context.ProductInventories
            .Where(x => x.ProductId == productId &&
                        x.AvailableQuantity >= quantity)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    x => x.AvailableQuantity,
                    x => x.AvailableQuantity - quantity),
                cancellationToken);

        return affectedRows == 1;
    }
}
