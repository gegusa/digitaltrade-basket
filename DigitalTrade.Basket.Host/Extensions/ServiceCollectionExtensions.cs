using DigitalTrade.Basket.Api.Contracts.Kafka;
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