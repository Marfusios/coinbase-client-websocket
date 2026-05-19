using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Coinbase.Client.Websocket;
using Coinbase.Client.Websocket.Channels;
using Coinbase.Client.Websocket.Client;
using Coinbase.Client.Websocket.Communicator;
using Coinbase.Client.Websocket.Requests;
using Coinbase.Client.Websocket.Responses.Books;
using Coinbase.Client.Websocket.Responses.Tickers;
using Coinbase.Client.Websocket.Responses.Trades;
using Coinbase.Client.Websocket.Sample.WinForms.Statistics;
using Coinbase.Client.Websocket.Sample.WinForms.Views;
using Serilog;
using Websocket.Client;

namespace Coinbase.Client.Websocket.Sample.WinForms.Presenters
{
    internal class StatsPresenter
    {
        private readonly IStatsView _view;

        private TradeStatsComputer _tradeStatsComputer;
        private OrderBookStatsComputer _orderBookStatsComputer;

        private ICoinbaseCommunicator _communicator;
        private CoinbaseWebsocketClient _client;

        private readonly string _defaultPair = "BTC-USD";
        private readonly string _currency = "$";

        public StatsPresenter(IStatsView view)
        {
            _view = view;

            HandleCommands();
        }

        private void HandleCommands()
        {
            _view.OnInit = OnInit;
            _view.OnStart = async () => await OnStart();
            _view.OnStop = OnStop;
        }

        private void OnInit()
        {
            Clear();
        }

        private async Task OnStart()
        {
            var pair = _view.Pair;
            if (string.IsNullOrWhiteSpace(pair))
                pair = _defaultPair;
            pair = pair.ToUpperInvariant();

            _tradeStatsComputer = new TradeStatsComputer();
            _orderBookStatsComputer = new OrderBookStatsComputer();

            _communicator = new CoinbaseWebsocketCommunicator(CoinbaseValues.ApiWebsocketUrl);
            _client = new CoinbaseWebsocketClient(_communicator);

            Subscribe(_client);

            _communicator.ReconnectionHappened.Subscribe(info =>
            {
                _view.Status($"Reconnected (type: {info.Type})", StatusType.Info);
                SendSubscriptions(_client, pair);
            });

            _communicator.DisconnectionHappened.Subscribe(info =>
            {
                if (info.Type == DisconnectionType.Error)
                {
                    _view.Status($"Disconnected by error, next try in {_communicator.ErrorReconnectTimeout?.TotalSeconds} sec",
                        StatusType.Error);
                    return;
                }

                _view.Status($"Disconnected (type: {info.Type})", StatusType.Warning);
            });

            await _communicator.Start();
        }

        private void OnStop()
        {
            _client?.Dispose();
            _communicator?.Dispose();
            _client = null;
            _communicator = null;
            Clear();
        }

        private void Subscribe(CoinbaseWebsocketClient client)
        {
            client.Streams.TickerStream.ObserveOn(TaskPoolScheduler.Default).Subscribe(HandleTicker);
            client.Streams.TradesStream.ObserveOn(TaskPoolScheduler.Default).Subscribe(HandleTrades);
            client.Streams.OrderBookSnapshotStream.ObserveOn(TaskPoolScheduler.Default).Subscribe(HandleOrderBookSnapshot);
            client.Streams.OrderBookUpdateStream.ObserveOn(TaskPoolScheduler.Default).Subscribe(HandleOrderBookUpdate);
        }

        private void SendSubscriptions(CoinbaseWebsocketClient client, string pair)
        {
            client.Send(new SubscribeRequest(
                new[] { pair },
                ChannelSubscriptionType.Ticker,
                ChannelSubscriptionType.Matches,
                ChannelSubscriptionType.Level2));
        }

        private void HandleTicker(TickerResponse response)
        {
            _view.Bid = response.BestBid.ToString("#.00");
            _view.Ask = response.BestAsk.ToString("#.00");
            _view.Status("Connected", StatusType.Info);
        }

        private void HandleTrades(TradeResponse trade)
        {
            Log.Information($"Received [{trade.TradeSide}] trade, price: {trade.Price}, amount: {trade.Size}");
            _tradeStatsComputer.HandleTrade(trade);

            FormatTradesStats(_view.Trades1Min, _tradeStatsComputer.GetStatsFor(1));
            FormatTradesStats(_view.Trades5Min, _tradeStatsComputer.GetStatsFor(5));
            FormatTradesStats(_view.Trades15Min, _tradeStatsComputer.GetStatsFor(15));
            FormatTradesStats(_view.Trades1Hour, _tradeStatsComputer.GetStatsFor(60));
            FormatTradesStats(_view.Trades24Hours, _tradeStatsComputer.GetStatsFor(60 * 24));
        }

        private void FormatTradesStats(Action<string, Side> setAction, TradeStats trades)
        {
            if (trades == TradeStats.NULL)
                return;

            if (trades.BuysPerc >= trades.SellsPerc)
            {
                setAction($"{trades.BuysPerc:###}% buys{Environment.NewLine}{trades.TotalCount}", Side.Buy);
                return;
            }

            setAction($"{trades.SellsPerc:###}% sells{Environment.NewLine}{trades.TotalCount}", Side.Sell);
        }

        private void HandleOrderBookSnapshot(OrderBookSnapshotResponse response)
        {
            _orderBookStatsComputer.HandleSnapshot(response);
            UpdateOrderBookView();
        }

        private void HandleOrderBookUpdate(OrderBookUpdateResponse response)
        {
            _orderBookStatsComputer.HandleUpdate(response);
            UpdateOrderBookView();
        }

        private void UpdateOrderBookView()
        {
            var stats = _orderBookStatsComputer.GetStats();
            if (stats == OrderBookStats.NULL)
                return;

            _view.Bid = stats.Bid.ToString("#.00");
            _view.Ask = stats.Ask.ToString("#.00");

            _view.BidAmount = $"{stats.BidAmountPerc:###}%{Environment.NewLine}{FormatToMillions(stats.BidAmount)}";
            _view.AskAmount = $"{stats.AskAmountPerc:###}%{Environment.NewLine}{FormatToMillions(stats.AskAmount)}";
        }

        private string FormatToMillions(double amount)
        {
            var millions = amount / 1000000;
            return $"{_currency}{millions:#.00} M";
        }

        private void Clear()
        {
            _view.Bid = string.Empty;
            _view.Ask = string.Empty;
            _view.BidAmount = string.Empty;
            _view.AskAmount = string.Empty;
            _view.Ping = string.Empty;
            _view.Trades1Min(string.Empty, Side.Buy);
            _view.Trades5Min(string.Empty, Side.Buy);
            _view.Trades15Min(string.Empty, Side.Buy);
            _view.Trades1Hour(string.Empty, Side.Buy);
            _view.Trades24Hours(string.Empty, Side.Buy);
        }
    }
}
