using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Client.Websocket.Utils;

namespace Coinbase.Client.Websocket.Network
{
    public class CoinbaseHttpClient
    {
        private readonly ICoinbaseAuthenticaton authentication;

        public static async Task<string> SendHttpRequest(ICoinbaseAuthenticaton authentication,
            string apiKey,
            string apiSecret,
            string passphrase, string endPoint)
        {
            var timestamp = authentication.NowS();
            var signature = authentication.CreateSignature(HttpMethod.Get, apiSecret, timestamp, endPoint);

            var client = new HttpClient();
            var contentBody = "";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get,
                new Uri(new Uri("https://api.pro.coinbase.com"), endPoint))
            {
                Content = contentBody == string.Empty
                    ? null
                    : new StringContent(contentBody, Encoding.UTF8, "application/json")
            };

            requestMessage.Headers.Add("User-Agent", "CoinbaseClientWebsocket");
            requestMessage.Headers.Add("CB-ACCESS-KEY", apiKey);
            requestMessage.Headers.Add("CB-ACCESS-TIMESTAMP",
                timestamp.ToString("F0", CultureInfo.InvariantCulture));
            requestMessage.Headers.Add("CB-ACCESS-SIGN", signature);
            requestMessage.Headers.Add("CB-ACCESS-PASSPHRASE", passphrase);


            var result = await client.SendAsync(requestMessage, CancellationToken.None);
            return await result.Content.ReadAsStringAsync();
        }
    }
}