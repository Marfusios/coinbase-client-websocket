using System.Reactive.Subjects;
using System.Threading.Tasks;
using Coinbase.Client.Websocket.Channels;
using Coinbase.Client.Websocket.Communicator;
using Coinbase.Client.Websocket.Json;
using Coinbase.Client.Websocket.Network;
using Coinbase.Client.Websocket.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace Coinbase.Client.Websocket.Responses.Wallets
{
    public partial class WalletsSnapshotResponse : ResponseBase
    {
        public WalletResponse[] Wallets { get; set; }

        internal static bool TryHandle(JObject response, Subject<WalletsSnapshotResponse> subject)
        {
            if (response?["type"].Value<string>() == "walletsSnapshot")
            {
                var parsed = response.ToObject<WalletsSnapshotResponse>(CoinbaseJsonSerializer.Serializer);
                subject.OnNext(parsed);
                return true;
            }

            return false;
        }

        /// Stream wallet snapshot manually
        /// </summary>
        /// <param name="communicator"></param>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="passphrase"></param>
        /// <returns></returns>
        public static async Task StreamWalletsSnapshot(ICoinbaseCommunicator communicator,
            string apiKey,
            string apiSecret,
            string passphrase)
        {
            var authentication = new CoinbaseAuthentication(apiKey, apiSecret, passphrase);
            var request =
                await CoinbaseHttpClient.SendHttpRequest(authentication, apiKey, apiSecret, passphrase, "/accounts");
            var wallets = WalletResponse.FromJson(request);

            var snapshot = new WalletsSnapshotResponse();
            snapshot.Wallets = wallets;
            snapshot.Type = ChannelType.WalletsSnapshot;

            var serialized = JsonConvert.SerializeObject(snapshot, CoinbaseJsonSerializer.Settings);
            communicator.StreamFakeMessage(ResponseMessage.TextMessage(serialized));
        }
        internal static bool TryHandle(JObject response, ISubject<WalletsSnapshotResponse> subject)
        {
            if (response?["type"].Value<string>() == "walletsSnapshot")
            {
                var parsed = response.ToObject<WalletsSnapshotResponse>(CoinbaseJsonSerializer.Serializer);
                subject.OnNext(parsed);
                return true;
            }

            return false;
        }
        
    }

    public partial class WalletsSnapshotResponse
    {
        public static WalletsSnapshotResponse FromJson(string json) =>
            JsonConvert.DeserializeObject<WalletsSnapshotResponse>(json, CoinbaseJsonSerializer.Settings);
    }

    public static partial class Serialize
    {
        public static string ToJson(WalletsSnapshotResponse self) =>
            JsonConvert.SerializeObject(self, CoinbaseJsonSerializer.Settings);
    }
}