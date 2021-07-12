namespace Coinbase.Client.Websocket.Responses.Orders
{
    public enum OrderStatus
    {
        Undefined,
        All,
        Pending,
        Active,
        Rejected,
        PartiallyFilled,
        Open,
        Done,
        Settled,
        Executed,
        Canceled
    }
}