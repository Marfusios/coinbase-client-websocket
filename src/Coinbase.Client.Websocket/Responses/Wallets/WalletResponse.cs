using System;
using System.Globalization;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Coinbase.Client.Websocket.Channels;
using Coinbase.Client.Websocket.Client;
using Coinbase.Client.Websocket.Communicator;
using Coinbase.Client.Websocket.Json;
using Coinbase.Client.Websocket.Network;
using Coinbase.Client.Websocket.Responses.Orders;
using Coinbase.Client.Websocket.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace Coinbase.Client.Websocket.Responses.Wallets
{
    public partial class WalletResponse
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("currency")] public string Currency { get; set; }

        [JsonProperty("balance")] public double Balance { get; set; }

        [JsonProperty("available")] public double Available { get; set; }

        [JsonProperty("hold")] public double Hold { get; set; }

        [JsonProperty("profile_id")] public Guid ProfileId { get; set; }

        [JsonProperty("trading_enabled")] public bool TradingEnabled { get; set; }

        internal static bool TryHandle(JObject response, Subject<WalletResponse> subject)
        {
            if (response?["type"].Value<string>() == "wallet")
            {
                var parsed = response.ToObject<WalletResponse>(CoinbaseJsonSerializer.Serializer);
                subject.OnNext(parsed);
                return true;
            }

            return false;
        }
    }

    public partial class WalletResponse
    {
        public static WalletResponse[] FromJson(string json)
        {
            return JsonConvert.DeserializeObject<WalletResponse[]>(json, CoinbaseJsonSerializer.Settings);
        }
    }

    public static partial class Serialize
    {
        public static string ToJson(this WalletResponse[] self)
        {
            return JsonConvert.SerializeObject(self, CoinbaseJsonSerializer.Settings);
        }
    }
}