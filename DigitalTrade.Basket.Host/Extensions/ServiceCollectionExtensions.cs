using DigitalTrade.Basket.Api.Contracts.Kafka;
using DigitalTrade.Basket.AppServices.EventHandlers;
using DigitalTrade.Basket.AppServices.Options;
using DigitalTrade.Basket.Host.Middlewares;
using DigitalTrade.Basket.Host.Options;
using KafkaFlow;
using KafkaFlow.Serializer;
using Refit;

namespace DigitalTrade.Basket.Host.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaFlow(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        var kafkaOptions = configuration
                               .GetSection(KafkaOptions.Section)
                               .Get<KafkaOptions>()
                           ?? throw new ArgumentNullException(KafkaOptions.Section);

        if (kafkaOptions.Servers.Length == 0)
        {
            throw new ArgumentException("kafkaOptions.Servers.Length == 0");
        }

        if (kafkaOptions.ConsumerGroup is null)
        {
            throw new ArgumentNullException(kafkaOptions.ConsumerGroup);
        }

        return services.AddKafka(kafka => kafka
            .UseConsoleLog()
            .AddCluster(cluster => cluster
                .WithBrokers(kafkaOptions.Servers)
                .CreateTopicIfNotExists(Topics.BasketCheckoutRequestedName, 1, 1)
                .AddConsumer(consumer => consumer
                    .Topic(Catalog.Api.Contracts.Catalog.Kafka.Topics.CatalogChangedName)
                    .WithGroupId(kafkaOptions.ConsumerGroup)
                    .WithBufferSize(100)
                    .WithWorkersCount(10)
                    .AddMiddlewares(m => m
                        .AddDeserializer<JsonCoreDeserializer>()
                        .AddTypedHandlers(h => h
                            .WithHandlerLifetime(InstanceLifetime.Scoped)
                            .AddHandler<ProductUpdatedEventHandler>()
                            .AddHandler<ProductDeletedEventHandler>()
                            .WhenNoHandlerFound(context =>
                                Console.WriteLine("Message not handled > Partition: {0} | Offset: {1}",
                                    context.ConsumerContext.Partition,
                                    context.ConsumerContext.Offset
                                )
                            )
                        )
                    )
                )
                .AddProducer(
                    Topics.BasketCheckoutRequestedProducerName,
                    producer => producer
                        .DefaultTopic(Topics.BasketCheckoutRequestedName)
                        .AddMiddlewares(m => m
                            .AddSerializer<JsonCoreSerializer>()
                            .Add<LoggingProducerMiddleware>()
                        )
                )
            )
        );
    }

    public static IHttpClientBuilder AddHttpClientFor<TApi, TOptions>(
        IServiceCollection services, IConfiguration configuration)
        where TApi : class
        where TOptions : HttpApiOptions, new()
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        var options = services.ConfigureOptions<TOptions>(configuration);

        return services
            .AddRefitClient<TApi>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(options.BaseAddress);
                c.Timeout = TimeSpan.Parse(options.Timeout);
            });
    }
    
    private static T ConfigureOptions<T>(this IServiceCollection services, IConfiguration configuration)
        where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        var optionsKey = typeof(T).Name;
        var optionsValue = configuration.GetSection(optionsKey);
        if (!optionsValue.Exists())
            throw new InvalidOperationException($"The configuration section '{optionsKey}' does not exist.");

        var options = new T();

        optionsValue.Bind(options);
        services.Configure<T>(optionsValue);

        return options;
    }
}