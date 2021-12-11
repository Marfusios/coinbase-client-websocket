namespace Coinbase.Client.Websocket.Channels;

/// <summary>
/// Channel type
/// </summary>
public enum ChannelType
{
    Heartbeat,
    Ticker,
    Level2,
    User,
    Matches,
    Full
}