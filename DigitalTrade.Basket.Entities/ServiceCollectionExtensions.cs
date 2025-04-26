using DigitalTrade.Basket.Entities.Repositories;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalTrade.Basket.Entities;

/// <summary>
/// Расширения коллекции сервисов.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static void AddEntities(this IServiceCollection services, ConfigurationManager configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddSingleton<IBasketRepository, BasketRepository>();

        var dbConfig = configuration.GetConnectionString("DefaultConnection")!;

        services.AddLinqToDBContext<BasketDataConnection>((provider, options)
            => options
                .UsePostgreSQL(dbConfig)
                .UseDefaultLogging(provider));
    }
}
