namespace InventoryService.Application.Features.Inventory.Commands.ReserveInventory;

public record OrderProcessingResult(
    bool WasReserved,
    bool IsDuplicate);
