using DigitalTrade.Basket.Entities;
using DigitalTrade.Catalog.Api.Contracts.Catalog.Kafka.Events;
using KafkaFlow;
using LinqToDB;

namespace DigitalTrade.Basket.AppServices.EventHandlers;

public class ProductUpdatedEventHandler : IMessageHandler<ProductUpdatedEvent>
{
    private readonly BasketDataConnection _db;

    public ProductUpdatedEventHandler(BasketDataConnection db)
    {
        _db = db;
    }

    public async Task Handle(IMessageContext context, ProductUpdatedEvent msg)
    {
        await _db.Items
            .Where(i => i.ProductId == msg.ProductId)
            .Set(i => i.Name, i => msg.Name)
            .Set(i => i.PriceAtAdding, i => msg.Price)
            .Set(i => i.UpdatedAt, i => DateTime.Now)
            .UpdateAsync();
    }
}
