using System.Reactive.Subjects;
using Coinbase.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Coinbase.Client.Websocket.Responses
{
    /// <summary>
    /// Error messages: Most failure cases will cause an error message (a message with the type "error") to be emitted.
    /// This can be helpful for implementing a client or debugging issues.
    /// </summary>
    public class ErrorResponse : ResponseBase
    {
        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }

        internal static bool TryHandle(JObject response, ISubject<ErrorResponse> subject)
        {
            if (response?["type"].Value<string>() != "error")
            {
                return false;
            }
            
            var parsed = response.ToObject<ErrorResponse>(CoinbaseJsonSerializer.Serializer);
            subject.OnNext(parsed);
            return true;
        }
    }
}
