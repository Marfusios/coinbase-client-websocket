using System;
using Coinbase.Client.Websocket.Requests;

namespace Coinbase.Client.Websocket.Client;

/// <summary>
/// Coinbase websocket client.
/// Use method `Send()` to subscribe to channels.
/// And `Streams` to subscribe. 
/// </summary>
public interface ICoinbaseWebsocketClient : IDisposable
{
    /// <summary>
    /// Serializes request and sends message via websocket client. 
    /// It logs and re-throws every exception. 
    /// </summary>
    /// <param name="request">Request/message to be sent</param>
    void Send<T>(T request) where T : RequestBase;

    /// <summary>
    /// Provided message streams.
    /// </summary>
    CoinbaseClientStreams Streams { get; }
}