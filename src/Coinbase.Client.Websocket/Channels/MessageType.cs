using System.Runtime.Serialization;

namespace Coinbase.Client.Websocket.Channels;

/// <summary>
/// Unique message type
/// </summary>
public enum MessageType
{
    Unknown = 0,
    Subscriptions,
    Heartbeat,
    Ticker,
    Snapshot,
    L2Update,
    Received,
    Open,
    Done,
    Match,
    Change,
    Activate,
    Auction,
    Error
}