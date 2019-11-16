using Coinbase.Client.Websocket.Channels;
using Newtonsoft.Json;

namespace Coinbase.Client.Websocket.Requests
{
    /// <summary>
    ///     Subscribe request
    /// </summary>
    public class UnsubscribeRequest : RequestBase
    {
        /// <inheritdoc />
        public UnsubscribeRequest()
        {
        }

        /// <inheritdoc />
        public UnsubscribeRequest(string[] products, ChannelSubscriptionType[] channels)
        {
            ProductIds = products;
            Channels = channels;
        }

        /// <inheritdoc />
        public UnsubscribeRequest(Channel[] channels)
        {
            Channels = channels;
        }

        /// <inheritdoc />
        public override string Type => "unsubscribe";

        /// <summary>
        ///     Target products/pairs.
        ///     Example:
        ///     "ETH-USD", "ETH-EUR"
        /// </summary>
        [JsonProperty("product_ids")]
        public string[] ProductIds { get; set; }

        /// <summary>
        ///     Target channels.
        ///     Could be simple string as "level2", "heartbeat".
        ///     Or complex object, example:
        ///     {
        ///     "name": "ticker",
        ///     "product_ids": [
        ///     "ETH-BTC",
        ///     "ETH-USD"
        ///     ]
        ///     }
        /// </summary>
        public object Channels { get; set; }
    }
}