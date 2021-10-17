using Newtonsoft.Json;

namespace Coinbase.Client.Websocket.Channels
{
    /// <summary>
    /// Used in requests and responses.
    /// Information about subscribed channel.
    /// </summary>
    public class Channel
    {
        /// <summary>
        /// Target channel name
        /// </summary>
        public ChannelType Name { get; set; }

        /// <summary>
        /// Target products/pairs.
        /// Example:
        /// "ETH-USD", "ETH-EUR"
        /// </summary>
        [JsonProperty("product_ids")]
        public string[] ProductIds { get; set; }
    }
}