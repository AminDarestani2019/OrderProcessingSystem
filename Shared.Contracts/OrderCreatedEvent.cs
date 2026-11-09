namespace Shared.Contracts;

public record OrderCreatedEvent(
    Guid OrderId,
    int ProductId,
    int Quantity
);