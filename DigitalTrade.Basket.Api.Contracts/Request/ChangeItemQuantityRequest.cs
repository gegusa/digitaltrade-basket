namespace DigitalTrade.Basket.Api.Contracts.Request;

public class ChangeItemQuantityRequest
{
    public long BasketItemId { get; set; }

    public int NewQuantity { get; set; }
}