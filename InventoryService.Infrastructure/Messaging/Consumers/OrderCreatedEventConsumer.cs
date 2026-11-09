using InventoryService.Application.Interfaces;
using MassTransit;
using Shared.Contracts;

namespace InventoryService.Infrastructure.Messaging.Consumers;

public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly IInventoryRepository _inventoryRepository;

    public OrderCreatedEventConsumer(
        IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task Consume(
        ConsumeContext<OrderCreatedEvent> context)
    {
        var order = context.Message;

        var result = await _inventoryRepository.ProcessOrderAsync(
            order.OrderId,
            order.ProductId,
            order.Quantity,
            context.CancellationToken);

        if (result.WasReserved)
        {
            await context.Publish(
                new InventoryReservedEvent(order.OrderId));
        }
        else
        {
            await context.Publish(
                new InventoryFailedEvent(
                    order.OrderId,
                    "Product not found or insufficient inventory."));
        }
    }
}
