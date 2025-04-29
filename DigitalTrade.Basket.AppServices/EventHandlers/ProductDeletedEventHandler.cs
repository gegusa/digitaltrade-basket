using DigitalTrade.Basket.Api.Contracts.Kafka;
using DigitalTrade.Basket.Entities;
using DigitalTrade.Catalog.Api.Contracts.Catalog.Dto;
using DigitalTrade.Catalog.Api.Contracts.Catalog.Kafka.Events;
using KafkaFlow;
using KafkaFlow.Producers;
using LinqToDB;

namespace DigitalTrade.Basket.AppServices.EventHandlers;

public class ProductDeletedEventHandler : IMessageHandler<ProductDeletedEvent>
{
    private readonly BasketDataConnection _db;

    public ProductDeletedEventHandler(BasketDataConnection db)
    {
        _db = db;
    }

    public async Task Handle(IMessageContext context, ProductDeletedEvent msg)
    {
        await _db.Items
            .Where(i => i.ProductId == msg.ProductId)
            .DeleteAsync();
    }
}
