using System.Reactive.Subjects;
using Coinbase.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Coinbase.Client.Websocket.Responses.Full;

/// <summary>
/// An order has changed.
/// </summary>
public class ChangeResponse : ResponseBase
{
    /// <summary>
    /// Order id
    /// </summary>
    public string OrderId { get; set; }

    /// <summary>
    /// Target product id
    /// </summary>
    public string ProductId { get; set; }

    /// <summary>
    /// New order size
    /// </summary>
    public double NewSize { get; set; }

    /// <summary>
    /// Old order size
    /// </summary>
    public double OldSize { get; set; }

    /// <summary>
    /// Order price
    /// </summary>
    public double Price { get; set; }

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

    internal static bool TryHandle(JObject response, ISubject<ChangeResponse> subject)
    {
        if (response?["type"].Value<string>() != "change")
            return false;

        var parsed = response.ToObject<ChangeResponse>(CoinbaseJsonSerializer.Serializer);
        subject.OnNext(parsed);
        return true;
    }
}