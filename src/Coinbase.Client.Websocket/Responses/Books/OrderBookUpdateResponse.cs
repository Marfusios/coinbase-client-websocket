using System.Reactive.Subjects;
using Coinbase.Client.Websocket.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Coinbase.Client.Websocket.Responses.Books;

/// <summary>
/// Order book update/diff
/// </summary>
public class OrderBookUpdateResponse : ResponseBase
{
    /// <summary>
    /// Target product id
    /// </summary>
    public string ProductId { get; set; }

    /// <summary>
    /// Order book changes.
    /// Please note that size is the updated size at that price level, not a delta.
    /// A size of "0" indicates the price level can be removed.
    /// </summary>
    [JsonConverter(typeof(OrderBookLevelConverter))]
    public OrderBookLevel[] Changes { get; set; }

    internal static bool TryHandle(JObject response, ISubject<OrderBookUpdateResponse> subject)
    {
        if (response?["type"].Value<string>() != "l2update")
            return false;

        var parsed = response.ToObject<OrderBookUpdateResponse>(CoinbaseJsonSerializer.Serializer);
        subject.OnNext(parsed);
        return true;
    }
}