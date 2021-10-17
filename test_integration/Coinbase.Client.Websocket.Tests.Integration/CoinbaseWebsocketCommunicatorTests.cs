using System;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Client.Websocket.Communicator;
using Xunit;

namespace Coinbase.Client.Websocket.Tests.Integration
{
    public class CoinbaseWebsocketCommunicatorTests
    {
        [Fact]
        public async Task OnStarting_ShouldGetInfoResponse()
        {
            var url = CoinbaseValues.ApiWebsocketUrl;
            using (var communicator = new CoinbaseWebsocketCommunicator(url))
            {
                string received = null;
                var receivedEvent = new ManualResetEvent(false);

                communicator.MessageReceived.Subscribe(msg =>
                {
                    received = msg.Text;
                    receivedEvent.Set();
                });

                await communicator.Start();

                communicator.Send("invalid test request");

                receivedEvent.WaitOne(TimeSpan.FromSeconds(30));

                Assert.NotNull(received);
            }
        }
    }
}