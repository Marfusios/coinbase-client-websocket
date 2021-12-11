using System.Reactive.Subjects;
using Coinbase.Client.Websocket.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Coinbase.Client.Websocket.Responses.Books;

/// <summary>
/// Order book snapshot
/// </summary>
public class OrderBookSnapshotResponse : ResponseBase
{
    /// <summary>
    /// Target product id
    /// </summary>
    public string ProductId { get; set; }

    /// <summary>
    /// Order book bid levels
    /// </summary>
    [JsonConverter(typeof(OrderBookLevelConverter), OrderBookSide.Buy)]
    public OrderBookLevel[] Bids { get; set; }

    /// <summary>
    /// Order book ask levels
    /// </summary>
    [JsonConverter(typeof(OrderBookLevelConverter), OrderBookSide.Sell)]
    public OrderBookLevel[] Asks { get; set; }


    internal static bool TryHandle(JObject response, ISubject<OrderBookSnapshotResponse> subject)
    {
        if (response?["type"].Value<string>() != "snapshot")
            return false;

        var parsed = response.ToObject<OrderBookSnapshotResponse>(CoinbaseJsonSerializer.Serializer);
        subject.OnNext(parsed);
        return true;
    }
}