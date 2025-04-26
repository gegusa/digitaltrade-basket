using DigitalTrade.Basket.Api.Contracts.Dto;

namespace DigitalTrade.Basket.Api.Contracts.Kafka;

public class BasketCheckoutRequestedEvent
{
    public long ClientId { get; set; }

    public ShoppingCartItem[] Basket { get; set; }

    public decimal TotalPrice { get; set; }
}