using System;

namespace Coinbase.Client.Websocket
{
    /// <summary>
    ///     Coinbase Pro static urls
    /// </summary>
    public static class CoinbaseValues
    {
        /// <summary>
        ///     Main Coinbase Pro url to websocket API
        /// </summary>
        public static readonly Uri ApiWebsocketUrl = new Uri("wss://ws-feed.pro.coinbase.com");

        /// <summary>
        ///     Sandbox Coinbase Pro url to websocket API
        /// </summary>
        public static readonly Uri ApiWebsocketSandboxUrl = new Uri("wss://ws-feed-public.sandbox.pro.coinbase.com");
    }
}