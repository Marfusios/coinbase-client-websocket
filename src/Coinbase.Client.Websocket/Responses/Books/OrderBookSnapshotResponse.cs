using Coinbase.Client.Websocket.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reactive.Subjects;

namespace Coinbase.Client.Websocket.Responses.Books
{
    /// <summary>
    ///     Order book snapshot
    /// </summary>
    public class OrderBookSnapshotResponse : ResponseBase
    {
        /// <summary>
        ///     Target product id
        /// </summary>
        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        /// <summary>
        ///     Order book bid levels
        /// </summary>
        [JsonConverter(typeof(OrderBookLevelConverter), OrderBookSide.Buy)]
        public OrderBookLevel[] Bids { get; set; }

        /// <summary>
        ///     Order book ask levels
        /// </summary>
        [JsonConverter(typeof(OrderBookLevelConverter), OrderBookSide.Sell)]
        public OrderBookLevel[] Asks { get; set; }


        internal static bool TryHandle(JObject response, ISubject<OrderBookSnapshotResponse> subject)
        {
            var type = response?["type"].Value<string>();
            if (type != "snapshot")
            {
                return false;
            }

            var parsed = response.ToObject<OrderBookSnapshotResponse>(CoinbaseJsonSerializer.Serializer);
            subject.OnNext(parsed);
            return true;
        }
    }
}