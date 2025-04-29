namespace DigitalTrade.Basket.AppServices.Options;

/// <summary>
/// Параметры настройки Kafka.
/// </summary>
public class KafkaOptions
{
    /// <summary>
    /// Название секции в конфигурации.
    /// </summary>
    public static string Section => "Kafka";

    /// <summary>
    /// Коллекция серверов.
    /// </summary>
    public string[] Servers { get; init; } = [];

    /// <summary>
    /// Название группы консьюмеров.
    /// </summary>
    public string? ConsumerGroup { get; init; }
}