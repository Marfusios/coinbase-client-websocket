﻿using System;

namespace Coinbase.Client.Websocket.Utils
{
    public static class UnixTime
    {
        public static readonly DateTime UnixBase = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long NowMs()
        {
            var substracted = DateTime.UtcNow.Subtract(UnixBase);
            return (long) substracted.TotalMilliseconds;
        }

        public static long NowTicks()
        {
            return DateTime.UtcNow.Ticks - UnixBase.Ticks;
        }

        public static DateTime ConvertToTime(long timeInMs)
        {
            return UnixBase.AddMilliseconds(timeInMs);
        }
    }
}