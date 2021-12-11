using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Client.Websocket.Channels;
using Coinbase.Client.Websocket.Client;
using Coinbase.Client.Websocket.Json;
using Coinbase.Client.Websocket.Requests;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Websocket.Client;

namespace Coinbase.Client.Websocket.Sample;

class Program
{
    static readonly ManualResetEvent ExitEvent = new(false);

    static readonly string API_KEY = "your api key";
    static readonly string API_SECRET = "";

    static void Main(string[] args)
    {
        InitLogging();

        AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnProcessExit;
        AssemblyLoadContext.Default.Unloading += DefaultOnUnloading;
        Console.CancelKeyPress += ConsoleOnCancelKeyPress;

        Console.WriteLine("|=======================|");
        Console.WriteLine("|    COINBASE CLIENT    |");
        Console.WriteLine("|=======================|");
        Console.WriteLine();

        Log.Debug("====================================");
        Log.Debug("              STARTING              ");
        Log.Debug("====================================");



        var url = CoinbaseValues.ApiWebsocketUrl;
        using var apiClient = new WebsocketClient(url);
        apiClient.Name = "Coinbase-1";
        apiClient.ReconnectTimeout = TimeSpan.FromMinutes(1);

        using var client = new CoinbaseWebsocketClient(NullLogger.Instance, apiClient);
        SubscribeToStreams(client);

        apiClient.ReconnectionHappened.Subscribe(async type =>
        {
            Log.Information($"Reconnection happened, type: {type}, resubscribing..");
            await SendSubscriptionRequests(client);
        });

        apiClient.Start().Wait();

        ExitEvent.WaitOne();

        Log.Debug("====================================");
        Log.Debug("              STOPPING              ");
        Log.Debug("====================================");
        Log.CloseAndFlush();
    }

    static async Task SendSubscriptionRequests(CoinbaseWebsocketClient client)
    {
        var subscription = new SubscribeRequest
        {
            ProductIds = new[]
            {
                "BTC-EUR",
                "BTC-USD"
            },
            Channels = new[]
            {
                ChannelType.Heartbeat,
                ChannelType.Ticker,
                ChannelType.Level2,
                ChannelType.Full
            }
        };

        client.Send(subscription);
        client.Send(new StatusSubscribeRequest());
    }

    static void SubscribeToStreams(CoinbaseWebsocketClient client)
    {
        client.Streams.ErrorStream.Subscribe(x =>
            Log.Warning($"Error received, message: {x.Message}"));


        client.Streams.SubscribeStream.Subscribe(x =>
        {
            Log.Information($"Subscribed, " +
                            $"channels: {JsonConvert.SerializeObject(x.Channels, CoinbaseJsonSerializer.Settings)}");
        });

        client.Streams.HeartbeatStream.Subscribe(x =>
            Log.Information($"Heartbeat received, product: {x.ProductId}, seq: {x.Sequence}, time: {x.Time}"));

        client.Streams.StatusStream.Subscribe(x =>
            Log.Information($"Status [{x.Products.Count} products]. [{x.Currencies.Count} currencies]")
        );

        client.Streams.TickerStream.Subscribe(x =>
            Log.Information($"Ticker {x.ProductId}. Bid: {x.BestBid} Ask: {x.BestAsk} Last size: {x.LastSize}, Price: {x.Price}")
        );

        client.Streams.MatchesStream.Subscribe(x =>
        {
            Log.Information($"Trade executed [{x.ProductId}] {x.TradeSide} price: {x.Price} size: {x.Size}");
        });

        client.Streams.OrderBookSnapshotStream.Subscribe(x =>
        {
            Log.Information($"OB snapshot [{x.ProductId}] bids: {x.Bids.Length}, asks: {x.Asks.Length}");
        });

        client.Streams.OrderBookUpdateStream.Subscribe(x =>
        {
            Log.Information($"OB updates [{x.ProductId}] changes: {x.Changes.Length}");
        });

        client.Streams.ReceivedStream.Subscribe(x =>
        {
            Log.Information($"Order received: {x.OrderId}");
        });

        client.Streams.OpenStream.Subscribe(x =>
        {
            Log.Information($"Order opened: {x.OrderId}");
        });

        client.Streams.ChangeStream.Subscribe(x =>
        {
            Log.Information($"Order changed: {x.OrderId}");
        });

        client.Streams.DoneStream.Subscribe(x =>
        {
            Log.Information($"Order done: {x.OrderId}");
        });

        // example of unsubscribe requests
        //_ = Task.Run(async () =>
        //{
        //    await Task.Delay(5000);
        //    client.Send(new UnsubscribeRequest(new[] { "BTC-EUR" }, new[] { ChannelType.Level2 }));
        //    await Task.Delay(5000);
        //    client.Send(new UnsubscribeRequest(new[] { "BTC-EUR" }, new[] { ChannelType.Matches }));
        //});
    }

    static void InitLogging()
    {
        var executingDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var logPath = Path.Combine(executingDir, "logs", "verbose.log");
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
            .WriteTo.ColoredConsole(LogEventLevel.Debug)
            .CreateLogger();
    }

    static void CurrentDomainOnProcessExit(object sender, EventArgs eventArgs)
    {
        Log.Warning("Exiting process");
        ExitEvent.Set();
    }

    static void DefaultOnUnloading(AssemblyLoadContext assemblyLoadContext)
    {
        Log.Warning("Unloading process");
        ExitEvent.Set();
    }

    static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
    {
        Log.Warning("Canceling process");
        e.Cancel = true;
        ExitEvent.Set();
    }
}