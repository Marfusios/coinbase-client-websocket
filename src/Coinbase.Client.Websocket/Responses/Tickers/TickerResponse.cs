using System.Reactive.Subjects;
using Coinbase.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Coinbase.Client.Websocket.Responses.Tickers;

/// <summary>
/// Provides real-time price updates every time a match happens.
/// It batches updates in case of cascading matches, greatly reducing bandwidth requirements.
/// </summary>
public class TickerResponse : ResponseBase
{
    /// <summary>
    /// Last executed trade id
    /// </summary>
    public long TradeId { get; set; }

    /// <summary>
    /// Target product id
    /// </summary>
    public string ProductId { get; set; }

    /// <summary>
    /// Last trade price
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// Last trade taker side
    /// </summary>
    public TradeSide Side { get; set; }

    /// <summary>
    /// Last trade size
    /// </summary>
    public double LastSize { get; set; }

    /// <summary>
    /// Current best bid price
    /// </summary>
    public double BestBid { get; set; }

    /// <summary>
    /// Current best ask price
    /// </summary>
    public double BestAsk { get; set; }

    // NOTE: Not documented
    public double Open24H { get; set; }

    // NOTE: Not documented
    public double Volume24H { get; set; }

    // NOTE: Not documented
    public double Low24H { get; set; }

    // NOTE: Not documented
    public double High24H { get; set; }

    // NOTE: Not documented
    public double Volume30D { get; set; }

    internal static bool TryHandle(JObject response, ISubject<TickerResponse> subject)
    {
        if (response?["type"].Value<string>() != "ticker")
            return false;

        var parsed = response.ToObject<TickerResponse>(CoinbaseJsonSerializer.Serializer);
        subject.OnNext(parsed);
        return true;
    }
}