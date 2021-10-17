﻿using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Client.Websocket.Channels;
using Coinbase.Client.Websocket.Client;
using Coinbase.Client.Websocket.Communicator;
using Coinbase.Client.Websocket.Json;
using Coinbase.Client.Websocket.Requests;
using Coinbase.Client.Websocket.Utils;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;

namespace Coinbase.Client.Websocket.Sample
{
    class Program
    {
        private static readonly ManualResetEvent ExitEvent = new ManualResetEvent(false);

        private static readonly string API_KEY = "xxx";
        private static readonly string API_SECRET = "xxx";
        private static readonly string API_PASSPHRASE = "xxx";

        static void Main(string[] args)
        {
            InitLogging();

            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnProcessExit;
            AssemblyLoadContext.Default.Unloading += DefaultOnUnloading;
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;

            Console.WriteLine("|=======================|");
            Console.WriteLine("|  COINBASE PRO CLIENT  |");
            Console.WriteLine("|=======================|");
            Console.WriteLine();

            Log.Debug("====================================");
            Log.Debug("              STARTING              ");
            Log.Debug("====================================");


            var url = CoinbaseValues.ApiWebsocketUrl;
            using (var communicator = new CoinbaseWebsocketCommunicator(url))
            {
                communicator.Name = "Coinbase-1";
                communicator.ReconnectTimeout = TimeSpan.FromMinutes(1);

                using (var client = new CoinbaseWebsocketClient(communicator))
                {
                    SubscribeToStreams(client);

                    communicator.ReconnectionHappened.Subscribe(async type =>
                    {
                        Log.Information($"Reconnection happened, type: {type}, resubscribing..");
                        SendSubscriptionRequests(client);

                        // Authenticated subscription
                        // Fails without valid api key, secret and passphrase
                        //await SendSubscriptionRequestsAuthenticated(client);
                    });

                    communicator.Start().Wait();

                    ExitEvent.WaitOne();
                }
            }

            Log.Debug("====================================");
            Log.Debug("              STOPPING              ");
            Log.Debug("====================================");
            Log.CloseAndFlush();
        }

        private static void SendSubscriptionRequests(CoinbaseWebsocketClient client)
        {
            var subscription = new SubscribeRequest(
                new[]
                {
                    "BTC-EUR",
                    "BTC-USD"
                },
                new[]
                {
                    ChannelSubscriptionType.Heartbeat,
                    // ChannelSubscriptionType.Ticker,
                    // ChannelSubscriptionType.Matches,
                    // ChannelSubscriptionType.User,
                    ChannelSubscriptionType.Level2,
                    // ChannelSubscriptionType.Status
                });

            client.Send(subscription);
        }

        private static async Task SendSubscriptionRequestsAuthenticated(CoinbaseWebsocketClient client)
        {
            //create an authenticator with your apiKey, apiSecret and passphrase
            var authenticator = new CoinbaseAuthentication(API_KEY, API_SECRET, API_PASSPHRASE);

            var subscription = new SubscribeRequest(
                new[]
                {
                    "BTC-EUR",
                    "BTC-USD"
                },
                new[]
                {
                    ChannelSubscriptionType.Heartbeat,
                    ChannelSubscriptionType.Ticker,
                    ChannelSubscriptionType.Matches,
                    ChannelSubscriptionType.Heartbeat,
                    ChannelSubscriptionType.User,
                    ChannelSubscriptionType.Level2,
                    //ChannelSubscriptionType.Status
                },
                authenticator);

            client.Send(subscription);
        }

        private static void SubscribeToStreams(CoinbaseWebsocketClient client)
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


            client.Streams.TickerStream.Subscribe(x =>
                Log.Information($"Ticker {x.ProductId}. Bid: {x.BestBid} Ask: {x.BestAsk} Last size: {x.LastSize}, Price: {x.Price}")
            );

            client.Streams.TradesStream.Subscribe(x =>
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

            client.Streams.OrderStream.Subscribe(orders =>
            {
                Log.Information($"Order Stream..");
                // foreach (var order in orders)
                //{
                Log.Information($"Order: {orders.ProductId} {orders.Side} {orders.Price} {orders.OrderStatus} {orders.OrderType}");
                // }
            });

            // example of unsubscribe requests
            //Task.Run(async () =>
            //{
            //    await Task.Delay(5000);
            //    await client.Send(new BookSubscribeRequest("XBTUSD") {IsUnsubscribe = true});
            //    await Task.Delay(5000);
            //    await client.Send(new TradesSubscribeRequest() {IsUnsubscribe = true});
            //});
        }

        private static void InitLogging()
        {
            var executingDir = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            var logPath = Path.Combine(executingDir, "logs", "verbose.log");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .WriteTo.Console(LogEventLevel.Debug,
                    "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }

        private static void CurrentDomainOnProcessExit(object sender, EventArgs eventArgs)
        {
            Log.Warning("Exiting process");
            ExitEvent.Set();
        }

        private static void DefaultOnUnloading(AssemblyLoadContext assemblyLoadContext)
        {
            Log.Warning("Unloading process");
            ExitEvent.Set();
        }

        private static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Log.Warning("Canceling process");
            e.Cancel = true;
            ExitEvent.Set();
        }
    }
}
