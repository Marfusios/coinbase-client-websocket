using Coinbase.Client.Websocket.Channels;

namespace Coinbase.Client.Websocket.Requests;

/// <summary>
/// Authenticated subscribe request
/// </summary>
public class AuthenticatedSubscribeRequest : SubscribeRequest
{
    /// <inheritdoc />
    public AuthenticatedSubscribeRequest(string[] products, params ChannelType[] channels)
    {
        ProductIds = products;
        Channels = channels;
    }

    /// <inheritdoc />
    public AuthenticatedSubscribeRequest(params Channel[] channels)
    {
        Channels = channels;
    }

    /// <summary>
    /// Sets the authentication properties
    /// </summary>
    public void SetAuthenticationProperties(string signature, string key, string passphrase, string timestamp)
    {
        Signature = signature;
        Key = key;
        Passphrase = passphrase;
        Timestamp = timestamp;
    }

    public string Signature { get; private set; }
    public string Key { get; set; }
    public string Passphrase { get; set; }
    public string Timestamp { get; set; }
}