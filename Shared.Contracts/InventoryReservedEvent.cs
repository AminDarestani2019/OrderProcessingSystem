namespace Shared.Contracts;

public record InventoryReservedEvent(
    Guid OrderId
);