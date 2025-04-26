using DigitalTrade.Basket.Entities.Entities;
using LinqToDB;

namespace DigitalTrade.Basket.Entities.Repositories;

internal class BasketRepository : IBasketRepository
{
    private readonly BasketDataConnection _db;

    public BasketRepository(BasketDataConnection db)
    {
        _db = db;
    }

    public async Task<ShoppingCartItemEntity[]> GetBasketItemsByClientId(long clientId, CancellationToken ct)
    {
        return await _db.Items
            .Where(i => i.ClientId == clientId)
            .ToArrayAsync(ct);
    }

    public async Task<ShoppingCartItemEntity> GetBasketItemById(long id, CancellationToken ct)
    {
        return await _db.Items
            .Where(i => i.Id == id)
            .SingleAsync(ct);
    }

    public async Task<long> AddItemToClientBasket(ShoppingCartItemEntity entity, CancellationToken ct)
    {
        return await _db.InsertWithInt64IdentityAsync(entity, token: ct);
    }

    public async Task UpdateCartItem(ShoppingCartItemEntity entity, CancellationToken ct)
    {
        await _db.UpdateAsync(entity, token: ct);
    }

    public async Task RemoveItemFromClientBasket(long itemId, CancellationToken ct)
    {
        await _db.Items
            .Where(i => i.Id == itemId)
            .DeleteAsync(ct);
    }

    public async Task RemoveClientBasket(long clientId, CancellationToken ct)
    {
        await _db.Items
            .Where(i => i.ClientId == clientId)
            .DeleteAsync(ct);
    }
}