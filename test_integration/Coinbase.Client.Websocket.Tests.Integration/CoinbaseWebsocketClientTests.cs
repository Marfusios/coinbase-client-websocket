using System;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Client.Websocket.Channels;
using Coinbase.Client.Websocket.Client;
using Coinbase.Client.Websocket.Communicator;
using Coinbase.Client.Websocket.Requests;
using Coinbase.Client.Websocket.Responses;
using Xunit;

namespace Coinbase.Client.Websocket.Tests.Integration
{
    public class CoinbaseWebsocketClientTests
    {
        private static readonly string API_KEY = "your_api_key";
        private static readonly string API_SECRET = "";

        [Fact]
        public async Task Heartbeat()
        {
            var url = CoinbaseValues.ApiWebsocketUrl;
            using (var communicator = new CoinbaseWebsocketCommunicator(url))
            {
                HeartbeatResponse received = null;
                var receivedEvent = new ManualResetEvent(false);

                using (var client = new CoinbaseWebsocketClient(communicator))
                {

                    client.Streams.HeartbeatStream.Subscribe(pong =>
                    {
                        received = pong;
                        receivedEvent.Set();
                    });

                    await communicator.Start();

                    client.Send(new SubscribeRequest(new []{"BTC-EUR"}, ChannelSubscriptionType.Heartbeat));

                    receivedEvent.WaitOne(TimeSpan.FromSeconds(30));

                    Assert.NotNull(received);
                }
            }
        }

    }
}
