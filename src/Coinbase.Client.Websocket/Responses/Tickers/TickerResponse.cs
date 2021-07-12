using System.Reactive.Subjects;
using Coinbase.Client.Websocket.Json;
using Coinbase.Client.Websocket.Responses.Trades;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Coinbase.Client.Websocket.Responses.Tickers
{
    /*
         {
        "type": "ticker",
        "trade_id": 20153558,
        "sequence": 3262786978,
        "time": "2017-09-02T17:05:49.250000Z",
        "product_id": "BTC-USD",
        "price": "4388.01000000",
        "side": "buy", // Taker side
        "last_size": "0.03000000",
        "best_bid": "4388",
        "best_ask": "4388.01"
        }   
     */

    /// <summary>
    /// </summary>
    public class TickerResponse : ResponseBase
    {
        /// <summary>
        /// Target product id
        /// </summary>
        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        /// <summary>
        /// Last trade price
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Last trade taker side
        /// </summary>
        public TradeSide Side { get; set; }

        /// <summary>
        /// Last executed trade id
        /// </summary>
        [JsonProperty("trade_id")]
        public long TradeId { get; set; }

        /// <summary>
        /// Last trade size
        /// </summary>
        [JsonProperty("last_size")]
        public double LastSize { get; set; }

        /// <summary>
        /// Current best bid price
        /// </summary>
        [JsonProperty("best_bid")]
        public double BestBid { get; set; }

        /// <summary>
        /// Current best ask price
        /// </summary>
        [JsonProperty("best_ask")]
        public double BestAsk { get; set; }


        [JsonProperty("open_24h")] public double Open24H { get; set; }

        [JsonProperty("volume_24h")] public double Volume24H { get; set; }

        [JsonProperty("low_24h")] public double Low24H { get; set; }

        [JsonProperty("high_24h")] public double High24H { get; set; }

        [JsonProperty("volume_30d")] public double Volume30D { get; set; }


        internal static bool TryHandle(JObject response, ISubject<TickerResponse> subject)
        {
            if (response?["type"].Value<string>() != "ticker") return false;

            var parsed = response.ToObject<TickerResponse>(CoinbaseJsonSerializer.Serializer);
            subject.OnNext(parsed);
            return true;
        }
    }
}