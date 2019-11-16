namespace Coinbase.Client.Websocket.Channels
{
    /// <summary>
    ///     Unique subscription type for channel
    /// </summary>
    public enum ChannelSubscriptionType
    {
        /// <summary>
        ///     Subscribe request type
        /// </summary>
        Subscribe,

        /// <summary>
        ///     The status channel will send all products and currencies on a preset interval.
        /// </summary>
        Status,

        /// <summary>
        ///     Heartbeat/ping-pong subscription
        /// </summary>
        Heartbeat,

        /// <summary>
        ///     Ticker/quotes subscription
        /// </summary>
        Ticker,

        /// <summary>
        ///     Order book subscription (snapshot + updates stream)
        /// </summary>
        Level2,

        /// <summary>
        ///     Trades subscription
        /// </summary>
        Matches
    }
}