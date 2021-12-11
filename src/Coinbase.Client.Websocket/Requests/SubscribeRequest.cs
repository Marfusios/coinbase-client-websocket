﻿using Coinbase.Client.Websocket.Channels;
using Newtonsoft.Json;

namespace Coinbase.Client.Websocket.Requests;

/// <summary>
/// Subscribe request
/// </summary>
public class SubscribeRequest : RequestBase
{
    /// <inheritdoc />
    public SubscribeRequest(string[] products, params ChannelType[] channels)
    {
        ProductIds = products;
        Channels = channels;
    }

    /// <inheritdoc />
    public SubscribeRequest(params Channel[] channels)
    {
        Channels = channels;
    }

    /// <inheritdoc />
    public override string Type => "subscribe";

    /// <summary>
    /// Target products/pairs.
    /// Example:
    /// "ETH-USD", "ETH-EUR"
    /// </summary>
    public string[] ProductIds { get; set; }

    /// <summary>
    /// Target channels.
    /// Could be simple string as "level2", "heartbeat".
    /// Or complex object, example:
    /// {
    ///   "name": "ticker",
    ///   "product_ids": [
    ///     "ETH-BTC",
    ///     "ETH-USD"
    ///   ]
    /// }
    /// </summary>
    public object Channels { get; set; }
}