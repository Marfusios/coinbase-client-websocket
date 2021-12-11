using System;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Client.Websocket.Channels;
using Coinbase.Client.Websocket.Client;
using Coinbase.Client.Websocket.Requests;
using Coinbase.Client.Websocket.Responses;
using Microsoft.Extensions.Logging.Abstractions;
using Websocket.Client;
using Xunit;

namespace Coinbase.Client.Websocket.Tests.Integration;

public class CoinbaseWebsocketClientTests
{
    [Fact]
    public async Task Heartbeat()
    {
        var url = CoinbaseValues.ApiWebsocketUrl;
        using var apiClient = new WebsocketClient(url);
        HeartbeatResponse received = null;
        var receivedEvent = new ManualResetEvent(false);

        using var client = new CoinbaseWebsocketClient(NullLogger.Instance, apiClient);
        client.Streams.HeartbeatStream.Subscribe(pong =>
        {
            received = pong;
            receivedEvent.Set();
        });

        await apiClient.Start();

        client.Send(new SubscribeRequest(new[] { "BTC-EUR" }, ChannelType.Heartbeat));

        receivedEvent.WaitOne(TimeSpan.FromSeconds(30));

        Assert.NotNull(received);
    }

}