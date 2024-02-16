using System;
using Coinbase.Client.Websocket.Communicator;
using Coinbase.Client.Websocket.Json;
using Coinbase.Client.Websocket.Requests;
using Coinbase.Client.Websocket.Responses;
using Coinbase.Client.Websocket.Responses.Books;
using Coinbase.Client.Websocket.Responses.Tickers;
using Coinbase.Client.Websocket.Responses.Trades;
using Coinbase.Client.Websocket.Validations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace Coinbase.Client.Websocket.Client
{
    /// <summary>
    /// Coinbase websocket client.
    /// Use method `Send()` to subscribe to channels.
    /// And `Streams` to subscribe. 
    /// </summary>
    public class CoinbaseWebsocketClient : IDisposable
    {
        private readonly ILogger<CoinbaseWebsocketClient> _logger;
        private readonly ICoinbaseCommunicator _communicator;
        private readonly IDisposable _messageReceivedSubscription;

        /// <inheritdoc />
        public CoinbaseWebsocketClient(ICoinbaseCommunicator communicator, ILogger<CoinbaseWebsocketClient>? logger = null)
        {
            ConValidations.ValidateInput(communicator, nameof(communicator));

            _communicator = communicator;
            _logger = logger ?? NullLogger<CoinbaseWebsocketClient>.Instance;
            _messageReceivedSubscription = _communicator.MessageReceived.Subscribe(HandleMessage);
        }

        /// <summary>
        /// Provided message streams
        /// </summary>
        public CoinbaseClientStreams Streams { get; } = new CoinbaseClientStreams();

        /// <summary>
        /// Expose logger for this client
        /// </summary>
        public ILogger<CoinbaseWebsocketClient> Logger => _logger;

        /// <summary>
        /// Cleanup everything
        /// </summary>
        public void Dispose()
        {
            _messageReceivedSubscription?.Dispose();
        }

        /// <summary>
        /// Serializes request and sends message via websocket communicator. 
        /// It logs and re-throws every exception. 
        /// </summary>
        /// <param name="request">Request/message to be sent</param>
        public void Send<T>(T request) where T : RequestBase
        {
            try
            {
                ConValidations.ValidateInput(request, nameof(request));

                var serialized =
                    CoinbaseJsonSerializer.Serialize(request);
                _communicator.Send(serialized);
            }
            catch (Exception e)
            {
                _logger.LogError(e, L("Exception while sending message '{request}'. Error: {error}"), request, e.Message);
                throw;
            }
        }

        private string L(string msg)
        {
            return $"[BMX WEBSOCKET CLIENT] {msg}";
        }

        private void HandleMessage(ResponseMessage message)
        {
            try
            {
                bool handled;
                var messageSafe = (message.Text ?? string.Empty).Trim();

                if (messageSafe.StartsWith("{"))
                {
                    handled = HandleObjectMessage(messageSafe);
                    if (handled)
                        return;
                }

                handled = HandleRawMessage(messageSafe);
                if (handled)
                    return;

                _logger.LogWarning(L("Unhandled response: '{message}'"), messageSafe);
            }
            catch (Exception e)
            {
                _logger.LogError(e, L("Exception while receiving message, error: {error}"), e.Message);
            }
        }

        private bool HandleRawMessage(string msg)
        {
            // ********************
            // ADD RAW HANDLERS BELOW
            // ********************

            return false;
        }

        private bool HandleObjectMessage(string msg)
        {
            var response = CoinbaseJsonSerializer.Deserialize<JObject>(msg);

            // ********************
            // ADD OBJECT HANDLERS BELOW
            // ********************

            return

                HeartbeatResponse.TryHandle(response, Streams.HeartbeatSubject) ||
                TradeResponse.TryHandle(response, Streams.TradesSubject) ||
                OrderBookUpdateResponse.TryHandle(response, Streams.OrderBookUpdateSubject) ||
                OrderBookSnapshotResponse.TryHandle(response, Streams.OrderBookSnapshotSubject) ||
                TickerResponse.TryHandle(response, Streams.TickerSubject) ||

                ErrorResponse.TryHandle(response, Streams.ErrorSubject) ||
                SubscribeResponse.TryHandle(response, Streams.SubscribeSubject) ||

                false;
        }
    }
}
