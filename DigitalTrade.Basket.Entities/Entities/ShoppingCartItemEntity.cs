using LinqToDB.Mapping;

namespace DigitalTrade.Basket.Entities.Entities;

[Table(Schema = "basket", Name = "shopping_cart_items")]
public class ShoppingCartItemEntity
{
    [PrimaryKey, Identity]
    [Column(Name = "id")]
    public int Id { get; set; }

    [Column(Name = "client_id"), NotNull] public long ClientId { get; set; }

    [Column(Name = "product_id"), NotNull] public long ProductId { get; set; }

    [Column(Name = "name"), NotNull] public string Name { get; set; }

    [Column(Name = "quantity"), NotNull] public int Quantity { get; set; }

    [Column(Name = "price_at_adding"), NotNull]
    public decimal PriceAtAdding { get; set; }

    [Column(Name = "added_at"), NotNull] public DateTime AddedAt { get; set; }

    [Column(Name = "updated_at"), NotNull] public DateTime UpdatedAt { get; set; }
}