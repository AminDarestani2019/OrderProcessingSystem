namespace OrderService.Application.Inerfaces;

public interface IEventPublisher
{
    Task PublishAsync<T>(
        T message,
        CancellationToken cancellationToken = default)
        where T : class;
}
