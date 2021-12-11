namespace Coinbase.Client.Websocket.Requests;

/// <summary>
/// Status subscribe request
/// </summary>
public class StatusSubscribeRequest : RequestBase
{
    /// <inheritdoc />
    public override string Type => "subscribe";

    public object Channels => new[]
    {
        new
        {
            name = "status"
        }
    };
}