﻿using System.Security.Cryptography;
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


            string ByteToString(byte[] buff)
            {
                var builder = new StringBuilder();

                for (var i = 0; i < buff.Length; i++) builder.Append(buff[i].ToString("X2")); // hex format
                return builder.ToString();
            }

            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                var hashmessage = hmacsha256.ComputeHash(messageBytes);
                return ByteToString(hashmessage).ToLower();
            }
        }
    }
}