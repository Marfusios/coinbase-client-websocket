using System;
using Coinbase.Client.Websocket.Responses.Trades;

namespace Coinbase.Client.Websocket.Responses.Orders
{
    public static class OrderUtils
    {
        public static double OrderSideToAmount(TradeSide side, double size)
        {
            switch (side)
            {
                case TradeSide.Buy:
                    return size;
                case TradeSide.Sell:
                    return -size;
                case TradeSide.Undefined:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }

            return 0;
        }
    }
}