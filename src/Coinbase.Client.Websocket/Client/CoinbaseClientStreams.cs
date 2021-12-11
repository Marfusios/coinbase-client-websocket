using System.Reactive.Subjects;
using Coinbase.Client.Websocket.Responses;
using Coinbase.Client.Websocket.Responses.Books;
using Coinbase.Client.Websocket.Responses.Full;
using Coinbase.Client.Websocket.Responses.Status;
using Coinbase.Client.Websocket.Responses.Tickers;

namespace Coinbase.Client.Websocket.Client;

/// <summary>
/// All provided streams.
/// You need to send subscription request in advance (via method `Send()` on CoinbaseWebsocketClient)
/// </summary>
public class CoinbaseClientStreams
{
    /// <summary>
    /// Server errors stream.
    /// Error messages: Most failure cases will cause an error message to be emitted.
    /// This can be helpful for implementing a client or debugging issues.
    /// </summary>
    public readonly Subject<ErrorResponse> ErrorStream = new();

    /// <summary>
    /// Response stream to every ping request
    /// </summary>
    public readonly Subject<HeartbeatResponse> HeartbeatStream = new();

    /// <summary>
    /// Subscription info stream, emits status after sending subscription request
    /// </summary>
    public readonly Subject<SubscribeResponse> SubscribeStream = new();

    /// <summary>
    /// Trades stream - emits every executed trade on Coinbase
    /// </summary>
    public readonly Subject<MatchResponse> MatchesStream = new();

    /// <summary>
    /// Order book snapshot stream - emits snapshot of the whole order book
    /// </summary>
    public readonly Subject<OrderBookSnapshotResponse> OrderBookSnapshotStream = new();

    /// <summary>
    /// Order book updates stream - emits every update to the order book
    /// </summary>
    public readonly Subject<OrderBookUpdateResponse> OrderBookUpdateStream = new();

    /// <summary>
    /// Quotes stream - emits on every change of top level of order book
    /// </summary>
    public readonly Subject<TickerResponse> TickerStream = new();

    /// <summary>
    /// An activate message is sent when a stop order is placed
    /// </summary>
    public readonly Subject<ActivateResponse> ActivateStream = new();

    /// <summary>
    /// An order has changed.
    /// This is the result of self-trade prevention adjusting the order size or available funds.
    /// Orders can only decrease in size or funds.
    /// </summary>
    public readonly Subject<ChangeResponse> ChangeStream = new();

    /// <summary>
    /// The order is no longer on the order book.
    /// Sent for all orders for which there was a received message.
    /// This message can result from an order being canceled or filled.
    /// </summary>
    public readonly Subject<DoneResponse> DoneStream = new();

    /// <summary>
    /// The order is now open on the order book.
    /// This message will only be sent for orders which are not fully filled immediately.
    /// </summary>
    public readonly Subject<OpenResponse> OpenStream = new();

    /// <summary>
    /// A valid order has been received and is now active.
    /// This message is emitted for every single valid order as soon as the matching engine receives it whether it fills immediately or not.
    /// </summary>
    public readonly Subject<ReceivedResponse> ReceivedStream = new();

    /// <summary>
    /// The status channel will send all products and currencies on a preset interval.
    /// </summary>
    public readonly Subject<StatusResponse> StatusStream = new();

    /// <summary>
    /// The auction channel will send information about the auction while the product is in auction mode.
    /// </summary>
    public readonly Subject<AuctionResponse> AuctionStream = new();
}