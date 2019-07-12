using System;

namespace Coinbase.Client.Websocket.Exceptions
{
    /// <summary>
    /// Exception that indicates bad user input
    /// </summary>
    public class CoinbaseBadInputException : CoinbaseException
    {
        /// <inheritdoc />
        public CoinbaseBadInputException()
        {
        }

        /// <inheritdoc />
        public CoinbaseBadInputException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public CoinbaseBadInputException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
