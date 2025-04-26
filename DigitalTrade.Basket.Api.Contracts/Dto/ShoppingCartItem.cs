namespace DigitalTrade.Basket.Api.Contracts.Dto;

public class ShoppingCartItem
{
    public int Id { get; set; }

    public long ClientId { get; set; }

    public long ProductId { get; set; }

    public string Name { get; set; }

    public int Quantity { get; set; }

    public decimal PriceAtAdding { get; set; }
}