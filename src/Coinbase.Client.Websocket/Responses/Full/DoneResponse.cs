using System.Reactive.Subjects;
using Coinbase.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Coinbase.Client.Websocket.Responses.Full;

/// <summary>
/// The order is no longer on the order book.
/// </summary>
public class DoneResponse : ResponseBase
{
    /// <summary>
    /// Target product id
    /// </summary>
    public string ProductId { get; set; }

    /// <summary>
    /// Order price
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// Order id
    /// </summary>
    public string OrderId { get; set; }

    /// <summary>
    /// Reason (filled or cancelled)
    /// </summary>
    public string Reason { get; set; }

    /// <summary>
    /// Order side
    /// </summary>
    public TradeSide Side { get; set; }

    /// <summary>
    /// Remaining order size
    /// </summary>
    public double RemainingSize { get; set; }

    /// <summary>
    /// User id (only populated when authenticated)
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Profile id (only populated when authenticated)
    /// </summary>
    public string ProfileId { get; set; }

    internal static bool TryHandle(JObject response, ISubject<DoneResponse> subject)
    {
        if (response?["type"].Value<string>() != "done")
            return false;

        var parsed = response.ToObject<DoneResponse>(CoinbaseJsonSerializer.Serializer);
        subject.OnNext(parsed);
        return true;
    }
}