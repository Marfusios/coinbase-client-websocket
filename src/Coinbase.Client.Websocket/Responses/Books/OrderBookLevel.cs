namespace Coinbase.Client.Websocket.Responses.Books
{
    /// <summary>
    /// One order book level
    /// </summary>
    public class OrderBookLevel
    {
        public OrderBookSide Side { get; set; }

        public double Price { get; set; }

        public double Amount { get; set; }
    }
}
