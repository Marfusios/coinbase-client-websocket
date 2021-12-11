using System.Collections.Generic;
using System.Reactive.Subjects;
using Coinbase.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Coinbase.Client.Websocket.Responses.Status;

/// <summary>
/// Status response
/// </summary>
public class StatusResponse : ResponseBase
{
    public IReadOnlyCollection<Product> Products { get; set; }

    public IReadOnlyCollection<Currency> Currencies { get; set; }

    internal static bool TryHandle(JObject response, ISubject<StatusResponse> subject)
    {
        if (response?["type"].Value<string>() != "status")
            return false;

        var parsed = response.ToObject<StatusResponse>(CoinbaseJsonSerializer.Serializer);
        subject.OnNext(parsed);
        return true;
    }
}