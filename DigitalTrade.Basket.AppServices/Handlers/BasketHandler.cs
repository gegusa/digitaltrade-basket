using DigitalTrade.Basket.Api.Contracts.Kafka;
using DigitalTrade.Basket.Api.Contracts.Kafka.Events;
using DigitalTrade.Basket.Api.Contracts.Request;
using DigitalTrade.Basket.Api.Contracts.Response;
using DigitalTrade.Basket.Entities.Entities;
using DigitalTrade.Basket.Entities.Repositories;
using DigitalTrade.Catalog.Api.Contracts.Catalog.Web;
using KafkaFlow.Producers;

namespace DigitalTrade.Basket.AppServices.Handlers;

public class BasketHandler : IBasketHandler
{
    private readonly IBasketRepository _basketRepository;
    private readonly IProducerAccessor _producers;
    private readonly ICatalogApi _catalogApi;

    public BasketHandler(
        IBasketRepository basketRepository, IProducerAccessor producers, ICatalogApi catalogApi)
    {
        _basketRepository = basketRepository;
        _producers = producers;
        _catalogApi = catalogApi;
    }

    public async Task CheckoutBasket(CheckoutBasketRequest request, CancellationToken ct)
    {
        var entities = await _basketRepository.GetBasketItemsByClientId(request.ClientId, ct);

        var basket = entities.Select(BasketMapper.ToDto).ToArray();

        var checkoutRequestedEvent = new BasketCheckoutRequestedEvent
        {
            ClientId = request.ClientId,
            Basket = basket,
            TotalPrice = basket.Sum(i => i.PriceAtAdding * i.Quantity)
        };

        await _producers[Topics.BasketCheckoutRequestedProducerName].ProduceAsync(
            Topics.BasketCheckoutRequestedName,
            checkoutRequestedEvent.ClientId.ToString(),
            checkoutRequestedEvent);
    }

    public async Task<AddItemToBasketResponse> AddItemToBasket(AddItemToBasketRequest request, CancellationToken ct)
    {
        var itemInfo = await _catalogApi.GetProductById(request.ProductId, ct);

        var entity = new ShoppingCartItemEntity
        {
            AddedAt = DateTime.Now,
            ClientId = request.ClientId,
            Name = itemInfo.Product.Name,
            PriceAtAdding = itemInfo.Product.Price,
            Quantity = 1,
            ProductId = request.ProductId
        };

        var itemId = await _basketRepository.AddItemToClientBasket(entity, ct);

        return new AddItemToBasketResponse
        {
            ItemId = itemId,
        };
    }

    public async Task ChangeItemQuantity(ChangeItemQuantityRequest request, CancellationToken ct)
    {
        var itemEntity = await _basketRepository.GetBasketItemById(request.BasketItemId, ct);

        itemEntity.Quantity = request.NewQuantity;

        await _basketRepository.UpdateCartItem(itemEntity, ct);
    }

    public async Task DeleteBasket(DeleteBasketRequest request, CancellationToken ct)
    {
        await _basketRepository.RemoveClientBasket(request.ClientId, ct);
    }

    public async Task<GetBasketResponse> GetBasket(GetBasketRequest request, CancellationToken ct)
    {
        var entities = await _basketRepository.GetBasketItemsByClientId(request.ClientId, ct);

        return new GetBasketResponse
        {
            ClientId = request.ClientId,
            BasketItems = entities.Select(BasketMapper.ToDto).ToArray()
        };
    }
}