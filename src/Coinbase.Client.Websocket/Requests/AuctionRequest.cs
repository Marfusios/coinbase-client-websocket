namespace Coinbase.Client.Websocket.Requests;

/// <summary>
/// Auction request
/// NOTE: The documentation is very ambiguous about how this works,
/// so it could be wrong
/// </summary>
public class AuctionRequest : RequestBase
{
    /// <inheritdoc />
    public override string Type => "auction";

    public object Channels => new[]
    {
        new { name = "auctionfeed" }
    };
}