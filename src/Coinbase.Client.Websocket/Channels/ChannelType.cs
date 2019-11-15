using System.Runtime.Serialization;

namespace Coinbase.Client.Websocket.Channels
{
    /// <summary>
    /// Unique message type
    /// </summary>
    public enum ChannelType
    {
        /// <summary>
        /// Unknown channel, most likely unimplemented, contact library developer
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Subscribed info channel
        /// </summary>
        Subscriptions,

        /// <summary>
        /// Status channel 
        /// </summary>
        Status,

        /// <summary>
        /// Heartbeat/ping-pong channel
        /// </summary>
        Heartbeat,

        /// <summary>
        /// Ticker/quotes channel
        /// </summary>
        Ticker,

        /// <summary>
        /// Order book level 2 channel
        /// </summary>
        Level2,

        /// <summary>
        /// Order book snapshot channel 
        /// </summary>
        Snapshot,

        /// <summary>
        /// Order book diff/updates channel
        /// </summary>
        L2Update,

        Received,
        Open,
        Done,

        /// <summary>
        /// Trades channel
        /// </summary>
        Match,

        /// <summary>
        /// Trades subscription
        /// </summary>
        Matches,

        [EnumMember(Value = "last_match")]
        LastMatch,

        Change,
        Activate,

        /// <summary>
        /// Server error channel
        /// </summary>
        Error
    }
}
