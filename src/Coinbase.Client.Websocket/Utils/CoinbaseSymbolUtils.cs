using System.Collections.Generic;

namespace Coinbase.Client.Websocket.Utils
{
    public static class CoinbaseSymbolUtils
    {
        /// <summary>
        /// Format product Id list into CoinbasePro trading symbol (BTCUSD --> BTC-USD)
        /// </summary>
        /// <param name="symbols"> new List<string> BTCUSD,ETHUSD</string></param>
        public static List<string> FormatToTProductId(List<string> symbols)
        {
            var symbolsListCoinbase = new List<string>();
            foreach (var symbol in symbols) symbolsListCoinbase.Add(symbol.Insert(3, "-").ToUpperInvariant());

            return symbolsListCoinbase;
        }

        /// <summary>
        /// Format product Id into CoinbasePro trading symbol (BTCUSD --> BTC-USD)
        /// </summary>
        /// <param name="symbol">BTCUSD, BTCUSD, etc</param>
        public static string FormatToTProductId(string symbol)
        {
            return symbol.Insert(3, "-").ToUpperInvariant();
        }

        /// <summary>
        /// Extract pair from symbol (tBTCUSD --> BTCUSD)
        /// </summary>
        /// <param name="symbol">tBTCUSD, fbtcusd, etc</param>
        public static string ExtractPair(string symbol)
        {
            var formatted = FormatPair(symbol);
            return !string.IsNullOrWhiteSpace(formatted) && formatted.Length > 6
                ? formatted.Remove(0, 1)
                : string.Empty;
        }

        /// <summary>
        /// Extract base symbol from pair (BTCUSD --> BTC)
        /// </summary>
        /// <param name="pair">BTC/USD, BTCUSD, etc</param>
        public static string ExtractBaseSymbol(string pair)
        {
            var formatted = FormatPair(pair);
            return !string.IsNullOrWhiteSpace(formatted) && formatted.Length > 5
                ? formatted.Substring(0, 3)
                : string.Empty;
        }

        /// <summary>
        /// Extract quote symbol from pair (BTCUSD --> USD)
        /// </summary>
        /// <param name="pair">BTC/USD, BTCUSD, etc</param>
        public static string ExtractQuoteSymbol(string pair)
        {
            var formatted = FormatPair(pair);
            return !string.IsNullOrWhiteSpace(formatted) && formatted.Length > 5
                ? formatted.Substring(3, 3)
                : string.Empty;
        }

        /// <summary>
        /// Format pair into unified style (btc/usd --> BTCUSD)
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        public static string FormatPair(string pair)
        {
            var safe = pair ?? string.Empty;
            return safe
                .Trim()
                .Replace("/", string.Empty)
                .Replace("\\", string.Empty)
                .Replace("-", string.Empty)
                .ToUpper();
        }

        public static string FormatPairToLower(string pair)
        {
            var safe = pair ?? string.Empty;
            return safe
                .Trim()
                .Replace("/", string.Empty)
                .Replace("\\", string.Empty)
                .Replace("-", string.Empty)
                .ToLower();
        }
    }
}