using System;

namespace Coinbase.Client.Websocket.Utils.Clock
{
    public interface IClock
    {
        DateTime GetTime();
    }
}