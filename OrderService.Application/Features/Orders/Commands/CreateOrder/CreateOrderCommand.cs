using MediatR;

namespace OrderService.Application.Features.Orders.Commands.CreateOrder;

public record CreateOrderCommand(
    int CustomerId,
    int ProductId,
    int Quantity
) : IRequest<Guid>;
