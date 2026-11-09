using MediatR;
using OrderService.Application.Inerfaces;
using OrderService.Domain;
using Shared.Contracts;

namespace OrderService.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEventPublisher _eventPublisher;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository, IEventPublisher eventPublisher)
    {
        _orderRepository = orderRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<Guid> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            Status = OrderStatus.Pending,
            CreatedDate = DateTime.UtcNow
        };

        await _orderRepository.AddAsync(order,cancellationToken);

        await _orderRepository.SaveChangesAsync(cancellationToken);

        await _eventPublisher.PublishAsync(
            new OrderCreatedEvent(
                order.Id,
                order.ProductId,
                order.Quantity),
            cancellationToken);

        return order.Id;
    }
}
