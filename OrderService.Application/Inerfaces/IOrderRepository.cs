using OrderService.Domain;

namespace OrderService.Application.Inerfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order,CancellationToken cancellationToken = default);
    Task<Order?> GetByIdAsync(Guid id,CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task UpdateStatusAsync(Guid orderId, OrderStatus status,CancellationToken cancellationToken = default);
}
