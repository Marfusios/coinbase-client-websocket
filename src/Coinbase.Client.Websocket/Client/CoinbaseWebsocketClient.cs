using System;
using System.Threading.Tasks;
using Coinbase.Client.Websocket.Communicator;
using Coinbase.Client.Websocket.Json;
using Coinbase.Client.Websocket.Logging;
using Coinbase.Client.Websocket.Requests;
using Coinbase.Client.Websocket.Responses;
using Coinbase.Client.Websocket.Responses.Books;
using Coinbase.Client.Websocket.Responses.Orders;
using Coinbase.Client.Websocket.Responses.Tickers;
using Coinbase.Client.Websocket.Responses.Trades;
using Coinbase.Client.Websocket.Responses.Wallets;
using Coinbase.Client.Websocket.Validations;
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
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        private readonly ICoinbaseCommunicator _communicator;
        private readonly IDisposable _messageReceivedSubscription;

        /// <inheritdoc />
        public CoinbaseWebsocketClient(ICoinbaseCommunicator communicator)
        {
            ConValidations.ValidateInput(communicator, nameof(communicator));

            _communicator = communicator;
            _messageReceivedSubscription = _communicator.MessageReceived.Subscribe(HandleMessage);
        }

        /// <summary>
        /// Provided message streams
        /// </summary>
        public CoinbaseClientStreams Streams { get; } = new CoinbaseClientStreams();

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
                Log.Error(e, L($"Exception while sending message '{request}'. Error: {e.Message}"));
                throw;
            }
        }

        private string L(string msg)
        {
            return $"[COINBASE PRO WEBSOCKET CLIENT] {msg}";
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
                    if (handled) return;
                }

                handled = HandleRawMessage(messageSafe);
                if (handled) return;

                Log.Warn(L($"Unhandled response:  '{messageSafe}'"));
            }
            catch (Exception e)
            {
                Log.Error(e, L("Exception while receiving message"));
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
                StatusResponse.TryHandle(response, Streams.StatusSubject) ||
                OrderResponse.TryHandle(response, Streams.OrdersSubject) ||
                WalletResponse.TryHandle(response, Streams.WalletsSubject) ||
                OrdersSnapshotResponse.TryHandle(response, Streams.OrdersSnapshotSubject) ||
                WalletsSnapshotResponse.TryHandle(response, Streams.WalletsSnapshotSubject);
        }
    }
}