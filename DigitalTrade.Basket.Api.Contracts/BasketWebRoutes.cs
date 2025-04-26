namespace DigitalTrade.Basket.Api.Contracts;

public static class BasketWebRoutes
{
    public const string BasePath = "basket";

    public const string CheckoutBasket = "checkout";

    public const string AddItemToBasket = "add";

    public const string ChangeItemQuantity = "item/quantity";

    public const string DeleteBasket = "delete";

    public const string GetBasket = "get/{clientId:long}";
}