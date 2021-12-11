using System;
using Coinbase.Client.Websocket.Json;
using Coinbase.Client.Websocket.Requests;
using Coinbase.Client.Websocket.Responses;
using Coinbase.Client.Websocket.Responses.Books;
using Coinbase.Client.Websocket.Responses.Full;
using Coinbase.Client.Websocket.Responses.Status;
using Coinbase.Client.Websocket.Responses.Tickers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace Coinbase.Client.Websocket.Client;

/// <inheritdoc />
public class CoinbaseWebsocketClient : ICoinbaseWebsocketClient
{
    readonly ILogger _logger;
    readonly IWebsocketClient _client;
    readonly IDisposable _messageReceivedSubscription;

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="logger">The logger to use for logging any warnings or errors.</param>
    /// <param name="client">The client to use for the trade websocket.</param>
    public CoinbaseWebsocketClient(ILogger logger, IWebsocketClient client)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));

        CoinbaseJsonSerializer.Logger = logger;

        _messageReceivedSubscription = _client.MessageReceived.Subscribe(HandleMessage);
    }

    /// <inheritdoc />
    public CoinbaseClientStreams Streams { get; } = new();

    /// <summary>
    /// Cleanup everything
    /// </summary>
    public void Dispose() => _messageReceivedSubscription?.Dispose();

    /// <inheritdoc />
    public void Send<T>(T request) where T : RequestBase
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        try
        {
            var serialized = CoinbaseJsonSerializer.Serialize(request);
            _client.Send(serialized);
        }
        catch (Exception e)
        {
            _logger.LogError(e, LogMessage($"Exception while sending message '{request}'. Error: {e.Message}"));
            throw;
        }
    }

    internal static string LogMessage(string message) => $"[COINBASE WEBSOCKET CLIENT] {message}";

    void HandleMessage(ResponseMessage message)
    {
        try
        {
            var messageSafe = (message.Text ?? string.Empty).Trim();

            if (messageSafe.StartsWith("{"))
                if (HandleObjectMessage(messageSafe))
                    return;

            _logger.LogWarning(LogMessage($"Unhandled response:  '{messageSafe}'"));
        }
        catch (Exception e)
        {
            _logger.LogError(e, LogMessage("Exception while receiving message"));
        }
    }

    bool HandleObjectMessage(string msg)
    {
        var response = CoinbaseJsonSerializer.Deserialize<JObject>(msg);

        return
            HeartbeatResponse.TryHandle(response, Streams.HeartbeatStream) ||
            OrderBookUpdateResponse.TryHandle(response, Streams.OrderBookUpdateStream) ||
            OrderBookSnapshotResponse.TryHandle(response, Streams.OrderBookSnapshotStream) ||
            TickerResponse.TryHandle(response, Streams.TickerStream) ||
            ErrorResponse.TryHandle(response, Streams.ErrorStream) ||
            SubscribeResponse.TryHandle(response, Streams.SubscribeStream) ||
            ActivateResponse.TryHandle(response, Streams.ActivateStream) ||
            ChangeResponse.TryHandle(response, Streams.ChangeStream) ||
            DoneResponse.TryHandle(response, Streams.DoneStream) ||
            MatchResponse.TryHandle(response, Streams.MatchesStream) ||
            OpenResponse.TryHandle(response, Streams.OpenStream) ||
            ReceivedResponse.TryHandle(response, Streams.ReceivedStream) ||
            StatusResponse.TryHandle(response, Streams.StatusStream) ||
            AuctionResponse.TryHandle(response, Streams.AuctionStream);
    }
}