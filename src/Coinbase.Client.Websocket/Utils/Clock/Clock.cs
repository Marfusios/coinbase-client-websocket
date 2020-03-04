using System;

namespace Coinbase.Client.Websocket.Utils.Clock
{
    public class Clock : IClock
    {
        public DateTime GetTime()
        {
            return DateTime.UtcNow;
        }
    }
}