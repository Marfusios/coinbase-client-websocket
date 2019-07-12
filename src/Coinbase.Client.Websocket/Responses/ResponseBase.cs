using System;
using Coinbase.Client.Websocket.Channels;
using Newtonsoft.Json;

namespace Coinbase.Client.Websocket.Responses
{
    /// <summary>
    /// Message which is used as base for every request and response
    /// </summary>
    public class ResponseBase
    {
        /// <summary>
        /// Unique type of the message
        /// </summary>
        public ChannelType Type { get; set; }

        /// <summary>
        /// Sequence numbers are increasing integer values for each product
        /// with every new message being exactly 1 sequence number than the one before it.
        ///
        /// If you see a sequence number that is more than one value from the previous,
        /// it means a message has been dropped.
        /// A sequence number less than one you have seen can be ignored or has arrived out-of-order.
        /// In both situations you may need to perform logic to make sure your system is in the correct state.
        /// </summary>
        public long? Sequence { get; set; }


        /// <summary>
        /// Current server time
        /// </summary>
        public DateTime? Time { get; set; }
    }
}
