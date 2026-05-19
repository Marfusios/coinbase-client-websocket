using System.Collections.Generic;
using System.Linq;
using Coinbase.Client.Websocket.Responses.Books;

namespace Coinbase.Client.Websocket.Sample.WinForms.Statistics
{
    internal class OrderBookStatsComputer
    {
        private readonly Dictionary<double, OrderBookLevel> _bids = new Dictionary<double, OrderBookLevel>();
        private readonly Dictionary<double, OrderBookLevel> _asks = new Dictionary<double, OrderBookLevel>();

        public void HandleSnapshot(OrderBookSnapshotResponse response)
        {
            _bids.Clear();
            _asks.Clear();

            foreach (var bid in response.Bids ?? new OrderBookLevel[0])
            {
                _bids[bid.Price] = bid;
            }

            foreach (var ask in response.Asks ?? new OrderBookLevel[0])
            {
                _asks[ask.Price] = ask;
            }
        }

        public void HandleUpdate(OrderBookUpdateResponse response)
        {
            foreach (var level in response.Changes ?? new OrderBookLevel[0])
            {
                var levels = level.Side == OrderBookSide.Buy ? _bids : _asks;
                if (level.Amount <= 0)
                {
                    levels.Remove(level.Price);
                    continue;
                }

                levels[level.Price] = level;
            }
        }

        public OrderBookStats GetStats()
        {
            var bids = _bids.OrderByDescending(x => x.Value.Price).ToArray();
            var asks = _asks.OrderBy(x => x.Value.Price).ToArray();

            if (!bids.Any() || !asks.Any())
                return OrderBookStats.NULL;

            var bidAmounts = bids.Take(20).Sum(x => x.Value.Amount * x.Value.Price);
            var askAmounts = asks.Take(20).Sum(x => x.Value.Amount * x.Value.Price);
            var total = bidAmounts + askAmounts;

            return new OrderBookStats(
                bids[0].Value.Price,
                asks[0].Value.Price,
                bidAmounts / total * 100,
                askAmounts / total * 100,
                bidAmounts,
                askAmounts);
        }
    }

    internal class OrderBookStats
    {
        public static readonly OrderBookStats NULL = new OrderBookStats(0, 0, 0, 0, 0, 0);

        public OrderBookStats(double bid, double ask, double bidAmountPerc, double askAmountPerc,
            double bidAmount, double askAmount)
        {
            Bid = bid;
            Ask = ask;
            BidAmountPerc = bidAmountPerc;
            AskAmountPerc = askAmountPerc;
            BidAmount = bidAmount;
            AskAmount = askAmount;
        }

        public double Bid { get; }

        public double Ask { get; }

        public double BidAmountPerc { get; }

        public double AskAmountPerc { get; }

        public double BidAmount { get; }

        public double AskAmount { get; }
    }
}
