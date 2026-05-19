using System;
using System.Collections.Generic;
using System.Linq;
using Coinbase.Client.Websocket.Responses.Trades;

namespace Coinbase.Client.Websocket.Sample.WinForms.Statistics
{
    internal class TradeStatsComputer
    {
        private readonly List<TimedTrade> _lastTrades = new List<TimedTrade>();

        public void HandleTrade(TradeResponse newTrade)
        {
            _lastTrades.Add(new TimedTrade(DateTime.UtcNow, newTrade));
        }

        public TradeStats GetStatsFor(int minutes)
        {
            var timeLimit = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(minutes));
            var trades = _lastTrades.Where(x => x.Timestamp >= timeLimit).ToArray();

            var buys = trades.Where(x => x.Trade.TradeSide == TradeSide.Buy).Sum(x => x.Trade.Size);
            var sells = trades.Where(x => x.Trade.TradeSide == TradeSide.Sell).Sum(x => x.Trade.Size);

            if (buys <= 0 && sells <= 0)
                return TradeStats.NULL;

            var total = buys + sells;
            return new TradeStats(buys / total * 100, sells / total * 100, trades.Length);
        }

        private class TimedTrade
        {
            public TimedTrade(DateTime timestamp, TradeResponse trade)
            {
                Timestamp = timestamp;
                Trade = trade;
            }

            public DateTime Timestamp { get; }

            public TradeResponse Trade { get; }
        }
    }

    internal class TradeStats
    {
        public static readonly TradeStats NULL = new TradeStats(0, 0, 0);

        public TradeStats(double buysPerc, double sellsPerc, int totalCount)
        {
            BuysPerc = buysPerc;
            SellsPerc = sellsPerc;
            TotalCount = totalCount;
        }

        public double BuysPerc { get; }

        public double SellsPerc { get; }

        public int TotalCount { get; }
    }
}
