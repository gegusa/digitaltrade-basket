using DigitalTrade.Basket.Api.Contracts.Dto;
using DigitalTrade.Basket.Entities.Entities;

namespace DigitalTrade.Basket.AppServices;

public static class BasketMapper
{
    public static ShoppingCartItem ToDto(ShoppingCartItemEntity entity)
    {
        return new ShoppingCartItem
        {
            ProductId = entity.ProductId,
            Quantity = entity.Quantity,
            ClientId = entity.ClientId,
            Name = entity.Name,
            PriceAtAdding = entity.PriceAtAdding,
            Id = entity.Id
        };
    }
}