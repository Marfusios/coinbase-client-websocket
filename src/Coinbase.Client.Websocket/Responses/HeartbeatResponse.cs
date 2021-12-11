using System.Reactive.Subjects;
using Coinbase.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Coinbase.Client.Websocket.Responses;

/// <summary>
/// Heartbeat response
/// </summary>
public class HeartbeatResponse : ResponseBase
{
    /// <summary>
    /// Target product id
    /// </summary>
    public string ProductId { get; set; }

    /// <summary>
    /// Last executed trade id
    /// </summary>
    public long? LastTradeId { get; set; }

    internal static bool TryHandle(JObject response, ISubject<HeartbeatResponse> subject)
    {
        if (response?["type"].Value<string>() != "heartbeat")
            return false;
            
        var parsed = response.ToObject<HeartbeatResponse>(CoinbaseJsonSerializer.Serializer);
        subject.OnNext(parsed);
        return true;
    }
}