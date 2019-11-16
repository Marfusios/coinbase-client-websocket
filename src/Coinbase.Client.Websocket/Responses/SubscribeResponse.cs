using System.Reactive.Subjects;
using Coinbase.Client.Websocket.Channels;
using Coinbase.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Coinbase.Client.Websocket.Responses
{
    /// <summary>
    ///     Response for every subscribe and unsubscribe event
    /// </summary>
    public class SubscribeResponse : ResponseBase
    {
        /// <summary>
        ///     Information about subscribed channels
        /// </summary>
        public Channel[] Channels { get; set; }

        internal static bool TryHandle(JObject response, ISubject<SubscribeResponse> subject)
        {
            if (response?["type"].Value<string>() != "subscriptions")
                return false;

            var parsed = response.ToObject<SubscribeResponse>(CoinbaseJsonSerializer.Serializer);
            subject.OnNext(parsed);
            return true;
        }
    }
}