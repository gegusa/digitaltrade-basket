namespace DigitalTrade.Basket.Api.Contracts.Request;

public class AddItemToBasketRequest
{
    public long ClientId { get; set; }

    public long ProductId { get; set; }
}