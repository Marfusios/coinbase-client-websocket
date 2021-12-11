namespace Coinbase.Client.Websocket.Requests;

/// <summary>
/// Subscribe request
/// </summary>
public class StatusUnsubscribeRequest : RequestBase
{
    /// <inheritdoc />
    public override string Type => "unsubscribe";

    public object Channels => new[]
    {
        new
        {
            name = "status"
        }
    };
}