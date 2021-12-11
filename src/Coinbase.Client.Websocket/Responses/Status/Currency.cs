using System.Collections.Generic;

namespace Coinbase.Client.Websocket.Responses.Status;

public class Currency
{
    public string Id { get; set; }
    public string Name { get; set; }
    public double MinSize { get; set; }
    public string Status { get; set; }
    public string StatusMessage { get; set; }
    public double MaxPrecision { get; set; }
    public IReadOnlyCollection<string> ConvertibleTo { get; set; }
    public object Details { get; set; }
}