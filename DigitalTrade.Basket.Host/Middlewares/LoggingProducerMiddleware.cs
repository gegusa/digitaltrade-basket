using KafkaFlow;

namespace DigitalTrade.Basket.Host.Middlewares;

public class LoggingProducerMiddleware : IMessageMiddleware
{
    public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
    {
        Console.WriteLine($"Producing message to topic {context.ProducerContext.Topic} with key {context.Message.Key}");
        await next(context);
    }
}