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

namespace Coinbase.Client.Websocket.Responses.Orders
{
    public partial class OrdersSnapshotResponse : ResponseBase
    {
        public OrderResponse[] Orders { get; set; }

        /// <summary>
        /// Stream orders snapshot manually via communicator
        /// </summary>
        public static async Task StreamOrdersSnapshot(ICoinbaseCommunicator communicator,
            string apiKey,
            string apiSecret,
            string passphrase)
        {
            var authentication = new CoinbaseAuthentication(apiKey, apiSecret, passphrase);
            var request =
                await CoinbaseHttpClient.SendHttpRequest(authentication, apiKey, apiSecret, passphrase, "/orders");
            var orders = OrderResponse.FromJson(request);
            
            var snapshot = new OrdersSnapshotResponse();
            snapshot.Orders = orders;
            snapshot.Type = ChannelType.OrdersSnapshot;

            var serialized = JsonConvert.SerializeObject(snapshot, CoinbaseJsonSerializer.Settings);
            communicator.StreamFakeMessage(ResponseMessage.TextMessage(serialized));
        }
        
        internal static bool TryHandle(JObject response, ISubject<OrdersSnapshotResponse> subject)
        {
            if (response?["type"].Value<string>() != "ordersSnapshot") return false;
            var parsed = response.ToObject<OrdersSnapshotResponse>(CoinbaseJsonSerializer.Serializer);
            subject.OnNext(parsed);
            return true;

        }
    }

    public partial class OrdersSnapshotResponse
    {
        public static OrdersSnapshotResponse FromJson(string json) =>
            JsonConvert.DeserializeObject<OrdersSnapshotResponse>(json, CoinbaseJsonSerializer.Settings);
    }

    public static partial class Serialize
    {
        public static string ToJson(this OrdersSnapshotResponse self) =>
            JsonConvert.SerializeObject(self, CoinbaseJsonSerializer.Settings);
    }
}