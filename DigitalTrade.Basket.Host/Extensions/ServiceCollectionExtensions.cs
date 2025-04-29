using DigitalTrade.Basket.Api.Contracts.Kafka;
using DigitalTrade.Basket.AppServices.EventHandlers;
using DigitalTrade.Basket.AppServices.Options;
using DigitalTrade.Basket.Host.Middlewares;
using KafkaFlow;
using KafkaFlow.Serializer;

namespace DigitalTrade.Basket.Host.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaFlow(this IServiceCollection services, IConfiguration configuration)
    {
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
}