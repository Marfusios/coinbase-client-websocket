using System.Reactive.Subjects;
using Coinbase.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Coinbase.Client.Websocket.Responses.Full;

/// <summary>
/// A valid order has been received and is now active.
/// </summary>
public class ReceivedResponse : ResponseBase
{
    /// <summary>
    /// Target product id
    /// </summary>
    public string ProductId { get; set; }

    /// <summary>
    /// Order id
    /// </summary>
    public string OrderId { get; set; }

    /// <summary>
    /// Order size (only populated for limit orders)
    /// </summary>
    public double? Size { get; set; }

    /// <summary>
    /// Order price (only populated for limit orders)
    /// </summary>
    public double? Price { get; set; }

    /// <summary>
    /// Order price (only populated for market orders)
    /// </summary>
    public double? Funds { get; set; }

    /// <summary>
    /// Order side
    /// </summary>
    public TradeSide Side { get; set; }

    /// <summary>
    /// Order type (limit or market)
    /// </summary>
    public string OrderType { get; set; }

    /// <summary>
    /// User id (only populated when authenticated)
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Profile id (only populated when authenticated)
    /// </summary>
    public string ProfileId { get; set; }

    internal static bool TryHandle(JObject response, ISubject<ReceivedResponse> subject)
    {
        if (response?["type"].Value<string>() != "received")
            return false;

        var parsed = response.ToObject<ReceivedResponse>(CoinbaseJsonSerializer.Serializer);
        subject.OnNext(parsed);
        return true;
    }
}