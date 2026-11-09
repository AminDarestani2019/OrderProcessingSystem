namespace Shared.Contracts;

public record InventoryFailedEvent(
    Guid OrderId,
    string Reason
);
