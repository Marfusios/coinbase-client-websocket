using System;

namespace Coinbase.Client.Websocket.Exceptions
{
    /// <summary>
    /// Base Coinbase exception
    /// </summary>
    public class CoinbaseException : Exception
    {
        /// <inheritdoc />
        public CoinbaseException()
        {
        }

        /// <inheritdoc />
        public CoinbaseException(string message)
            : base(message)
        {
        }

        /// <inheritdoc />
        public CoinbaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
