using DigitalTrade.Basket.Api.Contracts.Request;
using DigitalTrade.Basket.Api.Contracts.Response;

namespace DigitalTrade.Basket.AppServices.Handlers;

public interface IBasketHandler
{
    public Task CheckoutBasket(CheckoutBasketRequest request, CancellationToken ct);

    public Task<AddItemToBasketResponse> AddItemToBasket(AddItemToBasketRequest request, CancellationToken ct);

    public Task ChangeItemQuantity(ChangeItemQuantityRequest request, CancellationToken ct);

    public Task DeleteBasket(DeleteBasketRequest request, CancellationToken ct);

    public Task<GetBasketResponse> GetBasket(GetBasketRequest request, CancellationToken ct);
}