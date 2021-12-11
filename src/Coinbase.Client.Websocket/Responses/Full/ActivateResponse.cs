using System;
using System.Reactive.Subjects;
using Coinbase.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Coinbase.Client.Websocket.Responses.Full;

/// <summary>
/// An activate message is sent when a stop order is placed.
/// </summary>
public class ActivateResponse : ResponseBase
{
    /// <summary>
    /// Target product id
    /// </summary>
    public string ProductId { get; set; }

    /// <summary>
    /// Timestamp
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// User id
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Profile id
    /// </summary>
    public string ProfileId { get; set; }

    /// <summary>
    /// Order id
    /// </summary>
    public string OrderId { get; set; }

    /// <summary>
    /// Stop type
    /// </summary>
    public string StopType { get; set; }

    /// <summary>
    /// Order side
    /// </summary>
    public TradeSide Side { get; set; }

    /// <summary>
    /// Stop price
    /// </summary>
    public double StopPrice { get; set; }

    /// <summary>
    /// Order size (base currency)
    /// </summary>
    public double Size { get; set; }

    /// <summary>
    /// Order funds (quote currency)
    /// </summary>
    public double Funds { get; set; }

    /// <summary>
    /// Private
    /// </summary>
    public bool Private { get; set; }

    internal static bool TryHandle(JObject response, ISubject<ActivateResponse> subject)
    {
        if (response?["type"].Value<string>() != "activate")
            return false;

        var parsed = response.ToObject<ActivateResponse>(CoinbaseJsonSerializer.Serializer);
        subject.OnNext(parsed);
        return true;
    }
}