using DigitalTrade.Basket.Api.Contracts;
using DigitalTrade.Basket.Api.Contracts.Request;
using DigitalTrade.Basket.Api.Contracts.Response;
using DigitalTrade.Basket.AppServices.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalTrade.Basket.Host.Controllers;

[ApiController]
[Route(BasketWebRoutes.BasePath)]
public class BasketController : ControllerBase
{
    private readonly IBasketHandler _basketHandler;

    public BasketController(IBasketHandler basketHandler)
    {
        _basketHandler = basketHandler;
    }

    [HttpPost(BasketWebRoutes.CheckoutBasket)]
    public Task CheckoutBasket([FromBody] CheckoutBasketRequest request, CancellationToken ct)
    {
        return _basketHandler.CheckoutBasket(request, ct);
    }

    [HttpPost(BasketWebRoutes.AddItemToBasket)]
    public Task<AddItemToBasketResponse> AddItemToBasket([FromBody] AddItemToBasketRequest request, CancellationToken ct)
    {
        return _basketHandler.AddItemToBasket(request, ct);
    }

    [HttpPost(BasketWebRoutes.ChangeItemQuantity)]
    public Task ChangeItemQuantity([FromBody] ChangeItemQuantityRequest request, CancellationToken ct)
    {
        return _basketHandler.ChangeItemQuantity(request, ct);
    }

    [HttpPost(BasketWebRoutes.DeleteBasket)]
    public Task DeleteBasket([FromBody] DeleteBasketRequest request, CancellationToken ct)
    {
        return _basketHandler.DeleteBasket(request, ct);
    }

    [HttpGet(BasketWebRoutes.GetBasket)]
    public Task<GetBasketResponse> GetBasket(long clientId, CancellationToken ct)
    {
        return _basketHandler.GetBasket(new GetBasketRequest
        {
            ClientId = clientId
        }, ct);
    }
}