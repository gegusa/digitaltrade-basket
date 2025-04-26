using DigitalTrade.Basket.Api.Contracts.Dto;

namespace DigitalTrade.Basket.Api.Contracts.Response;

public class GetBasketResponse
{
    public long ClientId { get; set; }

    public ShoppingCartItem[] BasketItems { get; set; }
}