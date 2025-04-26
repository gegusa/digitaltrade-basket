using DigitalTrade.Basket.Entities.Entities;
using LinqToDB;
using LinqToDB.Data;

namespace DigitalTrade.Basket.Entities;

/// <summary>
/// Абстракция подключения к базе данных.
/// </summary>
public class BasketDataConnection : DataConnection
{
    public BasketDataConnection(DataOptions<BasketDataConnection> options)
        : base(options.Options)
    {
    }

    /// <summary>
    /// Таблица Клиенты.
    /// </summary>
    public ITable<ShoppingCartItemEntity> Items => this.GetTable<ShoppingCartItemEntity>();
}
