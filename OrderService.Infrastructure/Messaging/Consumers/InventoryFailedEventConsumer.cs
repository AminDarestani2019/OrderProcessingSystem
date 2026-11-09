using MassTransit;
using OrderService.Application.Inerfaces;
using OrderService.Domain;
using Shared.Contracts;

namespace OrderService.Infrastructure.Messaging.Consumers;

public class InventoryFailedEventConsumer
    : IConsumer<InventoryFailedEvent>
{
    private readonly IOrderRepository _orderRepository;

    public InventoryFailedEventConsumer(
        IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Consume(
        ConsumeContext<InventoryFailedEvent> context)
    {
        await _orderRepository.UpdateStatusAsync(
            context.Message.OrderId,
            OrderStatus.Failed,
            context.CancellationToken);
    }
}
