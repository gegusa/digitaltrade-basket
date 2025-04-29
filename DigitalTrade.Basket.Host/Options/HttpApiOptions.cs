namespace DigitalTrade.Basket.Host.Options;

public abstract class HttpApiOptions
{
    public string BaseAddress { get; set; }

    public string Timeout { get; set; }
}