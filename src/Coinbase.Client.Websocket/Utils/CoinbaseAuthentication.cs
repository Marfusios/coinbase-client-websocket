using System;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Coinbase.Client.Websocket.Utils
{
    public class CoinbaseAuthentication : ICoinbaseAuthenticaton
    {
        public static readonly DateTime UnixBase = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Create Coinbase authentication details
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="unsignedSignature"></param>
        /// <param name="passphrase"></param>
        /// <exception cref="ArgumentException"></exception>
        public CoinbaseAuthentication(string apiKey,
            string unsignedSignature,
            string passphrase)
        {
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(unsignedSignature) ||
                string.IsNullOrEmpty(passphrase))
            {
                throw new ArgumentException(
                    $"{nameof(CoinbaseAuthentication)} requires parameters {nameof(apiKey)}, {nameof(unsignedSignature)} and {nameof(passphrase)} to be populated.");
            }

            ApiKey = apiKey;
            UnsignedSignature = unsignedSignature;
            Passphrase = passphrase;
        }

        public string ApiKey { get; }

        public string UnsignedSignature { get; }

        public string Passphrase { get; }

        long ICoinbaseAuthenticaton.NowS()
        {
            return NowS();
        }

        public static long NowS()
        {
            var substracted = DateTime.UtcNow.Subtract(UnixBase);
            return (long) substracted.TotalSeconds;
        }

        public string CreateSignature(HttpMethod httpMethod, string secret, decimal timestamp, string requestUri,
            string contentBody = "")
        {
            var convertedString = Convert.FromBase64String(secret);
            var prehash = timestamp.ToString("F0", CultureInfo.InvariantCulture) + httpMethod.ToString().ToUpper() +
                          requestUri + contentBody;
            return HashString(prehash, convertedString);
        }

        private string HashString(string str, byte[] secret)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            using (var hmaccsha = new HMACSHA256(secret))
            {
                return Convert.ToBase64String(hmaccsha.ComputeHash(bytes));
            }
        }

        /*
        public static string CreateAuthPayload(long nonce)
        {
            return "/users/self/verify" + nonce;
        }

        public static string CreateSignature(string key, string message)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            string ByteToString(byte[] buff)
            {
                var builder = new StringBuilder();

                for (var i = 0; i < buff.Length; i++)
                {
                    builder.Append(buff[i].ToString("X2")); // hex format
                }

                return builder.ToString();
            }

            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                var hashmessage = hmacsha256.ComputeHash(messageBytes);
                return ByteToString(hashmessage).ToLower();
            }
        }*/
    }
}