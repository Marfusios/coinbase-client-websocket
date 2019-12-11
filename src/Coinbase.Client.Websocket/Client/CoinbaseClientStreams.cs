using Coinbase.Client.Websocket.Responses;
using Coinbase.Client.Websocket.Responses.Books;
using Coinbase.Client.Websocket.Responses.Tickers;
using Coinbase.Client.Websocket.Responses.Trades;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Coinbase.Client.Websocket.Client
{
    /// <summary>
    ///     All provided streams.
    ///     You need to send subscription request in advance (via method `Send()` on CoinbaseWebsocketClient)
    /// </summary>
    public class CoinbaseClientStreams
    {
        internal readonly Subject<ErrorResponse> ErrorSubject = new Subject<ErrorResponse>();

        //internal readonly Subject<InfoResponse> InfoSubject = new Subject<InfoResponse>();
        internal readonly Subject<HeartbeatResponse> HeartbeatSubject = new Subject<HeartbeatResponse>();

        internal readonly Subject<OrderBookSnapshotResponse> OrderBookSnapshotSubject =
            new Subject<OrderBookSnapshotResponse>();

        internal readonly Subject<OrderBookUpdateResponse> OrderBookUpdateSubject =
            new Subject<OrderBookUpdateResponse>();

        internal readonly Subject<StatusResponse> StatusSubject = new Subject<StatusResponse>();
        internal readonly Subject<SubscribeResponse> SubscribeSubject = new Subject<SubscribeResponse>();
        internal readonly Subject<TickerResponse> TickerSubject = new Subject<TickerResponse>();

        internal readonly Subject<TradeResponse> TradesSubject = new Subject<TradeResponse>();

        // PUBLIC

        /// <summary>
        ///     Server errors stream.
        ///     Error messages: Most failure cases will cause an error message to be emitted.
        ///     This can be helpful for implementing a client or debugging issues.
        /// </summary>
        public IObservable<ErrorResponse> ErrorStream => ErrorSubject.AsObservable();

        /// <summary>
        ///     Response stream to every ping request
        /// </summary>
        public IObservable<HeartbeatResponse> HeartbeatStream => HeartbeatSubject.AsObservable();

        /// <summary>
        ///     Subscription info stream, emits status after sending subscription request
        /// </summary>
        public IObservable<SubscribeResponse> SubscribeStream => SubscribeSubject.AsObservable();

        /// <summary>
        ///     Subscription info stream, emits status after sending subscription request
        /// </summary>
        public IObservable<StatusResponse> StatusStream => StatusSubject.AsObservable();

        /// <summary>
        ///     Trades stream - emits every executed trade on Coinbase
        /// </summary>
        public IObservable<TradeResponse> TradesStream => TradesSubject.AsObservable();


        /// <summary>
        ///     Order book snapshot stream - emits snapshot of the whole order book
        /// </summary>
        public IObservable<OrderBookSnapshotResponse> OrderBookSnapshotStream =>
            OrderBookSnapshotSubject.AsObservable();

        /// <summary>
        ///     Order book updates stream - emits every update to the order book
        /// </summary>
        public IObservable<OrderBookUpdateResponse> OrderBookUpdateStream => OrderBookUpdateSubject.AsObservable();

        /// <summary>
        ///     Quotes stream - emits on every change of top level of order book
        /// </summary>
        public IObservable<TickerResponse> TickerStream => TickerSubject.AsObservable();


        // PRIVATE
    }
}