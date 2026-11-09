using InventoryService.Application.Features.Inventory.Commands.ReserveInventory;
using MassTransit;
using Shared.Contracts;
using MediatR;

namespace InventoryService.Infrastructure.Messaging.Consumers;

public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly IMediator _mediator;

    public OrderCreatedEventConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(
        ConsumeContext<OrderCreatedEvent> context)
    {
        var order = context.Message;

        var reserved = await _mediator.Send(
            new ReserveInventoryCommand(
                order.ProductId,
                order.Quantity),
            context.CancellationToken);

        if (reserved)
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