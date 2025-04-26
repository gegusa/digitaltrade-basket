using DigitalTrade.Basket.Entities.Entities;

namespace DigitalTrade.Basket.Entities.Repositories;

public interface IBasketRepository
{
    public Task<ShoppingCartItemEntity[]> GetBasketItemsByClientId(long clientId, CancellationToken ct);

    public Task<ShoppingCartItemEntity> GetBasketItemById(long id, CancellationToken ct);

    public Task<long> AddItemToClientBasket(ShoppingCartItemEntity entity, CancellationToken ct);

    public Task UpdateCartItem(ShoppingCartItemEntity entity, CancellationToken ct);

    public Task RemoveItemFromClientBasket(long itemId, CancellationToken ct);

    public Task RemoveClientBasket(long clientId, CancellationToken ct);
}