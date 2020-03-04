using System.Net.Http;
using Coinbase.Client.Websocket.Channels;
using Coinbase.Client.Websocket.Utils;
using Newtonsoft.Json;

namespace Coinbase.Client.Websocket.Requests
{
    /// <summary>
    /// Subscribe request
    /// </summary>
    public class SubscribeRequest : RequestBase
    {
        /// <inheritdoc />
        public SubscribeRequest()
        {
        }

        /// <inheritdoc />
        public SubscribeRequest(string[] products, params ChannelSubscriptionType[] channels)
        {
            ProductIds = products;
            Channels = channels;
        }

        /// <inheritdoc />
        public SubscribeRequest(params Channel[] channels)
        {
            Channels = channels;
        }

        /// <inheritdoc />
        public SubscribeRequest(string[] products, ChannelSubscriptionType[] channels, ICoinbaseAuthenticaton authenticaton)
        {
            ProductIds = products;
            Channels = channels;

            //Timestamp = CoinbaseAuthentication.CreateAuthNonce();

            Timestamp = authenticaton.NowS();
            Key = authenticaton.ApiKey;
            Passphrase = authenticaton.Passphrase;
            Signature = authenticaton.CreateSignature(HttpMethod.Get, authenticaton.UnsignedSignature, Timestamp, "/users/self/verify");
        }

        /// <inheritdoc />
        public override string Type => "subscribe";

        /// <summary>
        /// Target products/pairs.
        /// Example:
        /// "ETH-USD", "ETH-EUR"
        /// </summary>
        [JsonProperty("product_ids")]
        public string[] ProductIds { get; set; }

        /// <summary>
        /// Target channels.
        /// Could be simple string as "level2", "heartbeat".
        /// Or complex object, example:
        /// {
        /// "name": "ticker",
        /// "product_ids": [
        /// "ETH-BTC",
        /// "ETH-USD"
        /// ]
        /// }
        /// </summary>
        public object Channels { get; set; }

        public string Signature { get; set; }
        public string Key { get; set; }
        public string Passphrase { get; set; }
        public long Timestamp { get; set; }
    }
}