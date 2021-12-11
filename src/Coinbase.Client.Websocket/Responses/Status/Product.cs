namespace Coinbase.Client.Websocket.Responses.Status;

public class Product
{
    public string Id { get; set; }
    public string BaseCurrency { get; set; }
    public string QuoteCurrency { get; set; }
    public double BaseMinSize { get; set; }
    public double BaseMaxSize { get; set; }
    public double BaseIncrement { get; set; }
    public double QuoteIncrement { get; set; }
    public string DisplayName { get; set; }
    public string Status { get; set; }
    public string StatusMessage { get; set; }
    public double MinMarketFunds { get; set; }
    public double MaxMarketFunds { get; set; }
    public bool PostOnly { get; set; }
    public bool LimitOnly { get; set; }
    public bool CancelOnly { get; set; }
    public bool FxStablecoin { get; set; }
}