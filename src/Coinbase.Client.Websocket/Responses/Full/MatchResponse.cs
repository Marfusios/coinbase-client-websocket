using System.Reactive.Subjects;
using Coinbase.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Coinbase.Client.Websocket.Responses.Full;

/// <summary>
/// A trade occurred between two orders.
/// </summary>
public class MatchResponse : ResponseBase
{
    /// <summary>
    /// Last executed trade id
    /// </summary>
    public long TradeId { get; set; }

    /// <summary>
    /// Maker order id
    /// </summary>
    public string MakerOrderId { get; set; }

    /// <summary>
    /// Taker order id
    /// </summary>
    public string TakerOrderId { get; set; }

    /// <summary>
    /// Target product id
    /// </summary>
    public string ProductId { get; set; }

    /// <summary>
    /// Trade size
    /// </summary>
    public double Size { get; set; }

    /// <summary>
    /// Trade price
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// The side field indicates the maker order side.
    /// If the side is sell this indicates the maker was a sell order and the match is considered an up-tick.
    /// A buy side match is a down-tick.
    /// </summary>
    public TradeSide Side { get; set; }

    /// <summary>
    /// User id (only populated when authenticated)
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Profile id (only populated when authenticated)
    /// </summary>
    public string ProfileId { get; set; }

    /// <summary>
    /// Maker profile id (only populated when authenticated)
    /// </summary>
    public string MakerProfileId { get; set; }

    /// <summary>
    /// Taker profile id (only populated when authenticated)
    /// </summary>
    public string TakerProfileId { get; set; }

    /// <summary>
    /// Maker fee rate (only populated when authenticated)
    /// </summary>
    public double? MakerFeeRate { get; set; }

    /// <summary>
    /// Taker fee rate (only populated when authenticated)
    /// </summary>
    public double? TakerFeeRate { get; set; }

    /// <summary>
    /// Trade side (from taker point of view)
    /// </summary>
    public TradeSide TradeSide => Side == TradeSide.Undefined ? TradeSide.Undefined :
        Side == TradeSide.Buy ? TradeSide.Sell : TradeSide.Buy;

    internal static bool TryHandle(JObject response, ISubject<MatchResponse> subject)
    {
        if (response?["type"].Value<string>() != "ticker")
            return false;

        var parsed = response.ToObject<MatchResponse>(CoinbaseJsonSerializer.Serializer);
        subject.OnNext(parsed);
        return true;
    }
}