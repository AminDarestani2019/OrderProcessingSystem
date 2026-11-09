using MassTransit;
using OrderService.Application.Inerfaces;
using OrderService.Domain;
using Shared.Contracts;

namespace OrderService.Infrastructure.Messaging.Consumers;

public class InventoryReservedEventConsumer
    : IConsumer<InventoryReservedEvent>
{
    private readonly IOrderRepository _orderRepository;

    public InventoryReservedEventConsumer(
        IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Consume(
        ConsumeContext<InventoryReservedEvent> context)
    {
        await _orderRepository.UpdateStatusAsync(
            context.Message.OrderId,
            OrderStatus.Confirmed,
            context.CancellationToken);
    }
}
