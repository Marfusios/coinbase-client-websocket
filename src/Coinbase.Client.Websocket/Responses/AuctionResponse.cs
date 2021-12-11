using System;
using System.Reactive.Subjects;
using Coinbase.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Coinbase.Client.Websocket.Responses;

/// <summary>
/// Will send information about the auction while the product is in auction mode.
/// </summary>
public class AuctionResponse : ResponseBase
{
    /// <summary>
    /// Target product id
    /// </summary>
    public string ProductId { get; set; }

    /// <summary>
    /// Auction state
    /// </summary>
    public string AuctionState { get; set; }

    /// <summary>
    /// Current best bid price
    /// </summary>
    public double BestBidPrice { get; set; }

    /// <summary>
    /// Current best bid size
    /// </summary>
    public double BestBidSize { get; set; }

    /// <summary>
    /// Current best ask price
    /// </summary>
    public double BestAskPrice { get; set; }

    /// <summary>
    /// Current best ask size
    /// </summary>
    public double BestAskSize { get; set; }

    /// <summary>
    /// Open price
    /// </summary>
    public double OpenPrice { get; set; }

    /// <summary>
    /// Open size
    /// </summary>
    public double OpenSize { get; set; }

    /// <summary>
    /// Can open
    /// </summary>
    public double CanOpen { get; set; }

    /// <summary>
    /// Timestamp
    /// </summary>
    public DateTime Timestamp { get; set; }

    internal static bool TryHandle(JObject response, ISubject<AuctionResponse> subject)
    {
        if (response?["type"].Value<string>() != "auction")
            return false;

        var parsed = response.ToObject<AuctionResponse>(CoinbaseJsonSerializer.Serializer);
        subject.OnNext(parsed);
        return true;
    }
}