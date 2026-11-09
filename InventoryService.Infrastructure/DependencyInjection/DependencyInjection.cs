using InventoryService.Application.Interfaces;
using InventoryService.Infrastructure.Messaging.Consumers;
using InventoryService.Infrastructure.Persistence;
using InventoryService.Infrastructure.Persistence.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryService.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<InventoryDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("InventoryDatabase")));

        services.AddScoped<IInventoryRepository, InventoryRepository>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderCreatedEventConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(
                    configuration["RabbitMQ:Host"],
                    configuration["RabbitMQ:VirtualHost"],
                    h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"]);
                        h.Password(configuration["RabbitMQ:Password"]);
                    });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
