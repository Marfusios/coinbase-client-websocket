using System.Reactive.Subjects;
using Coinbase.Client.Websocket.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Coinbase.Client.Websocket.Responses.Trades
{
    /*
         {
        "type": "match",
        "trade_id": 10,
        "sequence": 50,
        "maker_order_id": "ac928c66-ca53-498f-9c13-a110027a60e8",
        "taker_order_id": "132fb6ae-456b-4654-b4e0-d681ac05cea1",
        "time": "2014-11-07T08:19:27.028459Z",
        "product_id": "BTC-USD",
        "size": "5.23512",
        "price": "400.23",
        "side": "sell"
        }
     */

    /// <summary>
    /// Executed trade response
    /// </summary>
    public class TradeResponse : ResponseBase
    {
        /// <summary>
        /// Last executed trade id
        /// </summary>
        [JsonProperty("trade_id")]
        public long TradeId { get; set; }

        /// <summary>
        /// Reference to maker order (that was sitting on the order book)
        /// </summary>
        [JsonProperty("maker_order_id")]
        public string MakerOrderId { get; set; }

        /// <summary>
        /// Reference to taker order (aggressor, taking liquidity)
        /// </summary>
        [JsonProperty("taker_order_id")]
        public string TakerOrderId { get; set; }

        /// <summary>
        /// Target product id
        /// </summary>
        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        /// <summary>
        /// Trade price
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Trade size
        /// </summary>
        public double Size { get; set; }

        /// <summary>
        /// The side field indicates the maker order side.
        /// If the side is sell this indicates the maker was a sell order and the match is considered an up-tick.
        /// A buy side match is a down-tick.
        /// </summary>
        [JsonProperty("side")]
        public TradeSide MakerOrderSide { get; set; }

        /// <summary>
        /// Trade side (from taker point of view)
        /// </summary>
        public TradeSide TradeSide => MakerOrderSide == TradeSide.Undefined ? TradeSide.Undefined :
            MakerOrderSide == TradeSide.Buy ? TradeSide.Sell : TradeSide.Buy;


        internal static bool TryHandle(JObject response, ISubject<TradeResponse> subject)
        {
            var type = response?["type"].Value<string>();
            if (type != "match" && type != "last_match") return false;

            var parsed = response.ToObject<TradeResponse>(CoinbaseJsonSerializer.Serializer);
            subject.OnNext(parsed);
            return true;
        }
    }
}