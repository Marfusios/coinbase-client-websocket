using System.Net.Http;

namespace Coinbase.Client.Websocket.Utils
{
    public interface ICoinbaseAuthenticaton
    {
        string ApiKey { get; }

        string UnsignedSignature { get; }

        string Passphrase { get; }

        /// <summary>
        /// Create signature for authentication
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="secret"></param>
        /// <param name="timestamp"></param>
        /// <param name="requestUri"></param>
        /// <param name="contentBody"></param>
        /// <returns></returns>
        string CreateSignature(
            HttpMethod httpMethod,
            string secret,
            decimal timestamp,
            string requestUri,
            string contentBody = "");

        /// <summary>
        /// Get timestamp in seconds
        /// </summary>
        /// <returns></returns>
        long NowS();
    }
}