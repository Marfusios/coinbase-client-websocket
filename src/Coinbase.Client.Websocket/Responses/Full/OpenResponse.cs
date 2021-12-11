using System.Reactive.Subjects;
using Coinbase.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Coinbase.Client.Websocket.Responses.Full;

/// <summary>
/// The order is now open on the order book.
/// </summary>
public class OpenResponse : ResponseBase
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
    /// Order price
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// Remaining order size
    /// </summary>
    public double RemainingSize { get; set; }

    /// <summary>
    /// Order side
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

    internal static bool TryHandle(JObject response, ISubject<OpenResponse> subject)
    {
        if (response?["type"].Value<string>() != "open")
            return false;

        var parsed = response.ToObject<OpenResponse>(CoinbaseJsonSerializer.Serializer);
        subject.OnNext(parsed);
        return true;
    }
}