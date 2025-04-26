using System.Reflection;
using DigitalTrade.Basket.AppServices.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalTrade.Basket.AppServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBasketHandler, BasketHandler>();

        return services;
    }

}