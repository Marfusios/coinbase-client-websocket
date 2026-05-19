using System.Security.Cryptography;
using System.Text;

namespace Coinbase.Client.Websocket.Utils
{
    public static class CoinbaseAuthentication
    {

        public static long CreateAuthNonce(long? time = null)
        {
            var timeSafe = time ?? UnixTime.NowMs();
            return timeSafe * 1000;
        }

        public static string CreateAuthPayload(long nonce)
        {
            return "/users/self/verify" + nonce;
        }

        public static string CreateSignature(string key, string message)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return ToLowerHex(hashmessage);
            }
        }

        private static string ToLowerHex(byte[] bytes)
        {
            var chars = new char[bytes.Length * 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                var value = bytes[i];
                chars[i * 2] = GetLowerHexChar(value >> 4);
                chars[i * 2 + 1] = GetLowerHexChar(value & 0xF);
            }

            return new string(chars);
        }

        private static char GetLowerHexChar(int value)
        {
            return (char)(value < 10 ? '0' + value : 'a' + value - 10);
        }
    }
}
