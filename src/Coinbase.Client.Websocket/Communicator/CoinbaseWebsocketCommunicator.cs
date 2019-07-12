using System;
using System.Net.WebSockets;
using Websocket.Client;

namespace Coinbase.Client.Websocket.Communicator
{
    /// <inheritdoc cref="WebsocketClient" />
    public class CoinbaseWebsocketCommunicator : WebsocketClient, ICoinbaseCommunicator
    {
        /// <inheritdoc />
        public CoinbaseWebsocketCommunicator(Uri url, Func<ClientWebSocket> clientFactory = null) 
            : base(url, clientFactory)
        {
        }
    }
}
