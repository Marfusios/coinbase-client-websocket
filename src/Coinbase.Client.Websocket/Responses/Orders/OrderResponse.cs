﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Coinbase.Client.Websocket.Channels;
using Coinbase.Client.Websocket.Client;
using Coinbase.Client.Websocket.Communicator;
using Coinbase.Client.Websocket.Json;
using Coinbase.Client.Websocket.Network;
using Coinbase.Client.Websocket.Responses.Trades;
using Coinbase.Client.Websocket.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace Coinbase.Client.Websocket.Responses.Orders
{
    public partial class OrderResponse
    {
        public string Id { get; set; }

        public long? Cid { get; set; }

        public long? Gid { get; set; }

        [JsonProperty("orderType")] 
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderType OrderType { get; set; }

        [JsonProperty("status")] public OrderStatus OrderStatus { get; set; }

        /// <summary>
        /// Pair (BTCUSD)
        /// </summary>
        public string Pair => CoinbaseSymbolUtils.FormatPairToLower(ProductId);

        public double? Size { get; set; }

        /// <summary>
        /// Positive means buy, negative means sell.
        /// </summary>
        public double? Amount => OrderUtils.OrderSideToAmount(Side, Size ?? 0);

        public double? Funds { get; set; }

        public double? TakerFeeRate { get; set; }

        public bool? Private { get; set; }

        public double? StopPrice { get; set; }

        public string UserId { get; set; }

        public string ProfileId { get; set; }

        [JsonProperty("side")] public TradeSide Side { get; set; }

        [JsonProperty("product_id")] public string ProductId { get; set; }

        public DateTimeOffset? TimeStamp { get; set; }

        public double? RemainingSize { get; set; }

        public double? NewSize { get; set; }

        public double? OldSize { get; set; }

        public double? OldFunds { get; set; }

        public double? NewFunds { get; set; }

        public double? Price { get; set; }

        public long? Sequence { get; set; }

        [JsonProperty("time_in_force")] public string TimeInForce { get; set; }

        public DateTime? Time { get; set; }

        [JsonProperty("created_at")] public DateTimeOffset MtsCreate { get; set; }

        public DoneReason? Reason { get; set; }

        public string ClientOid { get; set; }

        [JsonProperty("fill_fees")] public string FillFees { get; set; }

        [JsonProperty("filled_size")] public string FilledSize { get; set; }

        public string ExecutedValue { get; set; }

        public bool? Settled { get; set; }

        [JsonProperty("post_only")] public string PostOnly { get; set; }

        internal static bool TryHandle(JObject response, Subject<OrderResponse> subject)
        {
            if (response?["type"].Value<string>() == "received" ||
                response?["type"].Value<string>() == "open" ||
                response?["type"].Value<string>() == "done" ||
                response?["type"].Value<string>() == "match" ||
                response?["type"].Value<string>() == "change" ||
                response?["type"].Value<string>() == "activate" ||
                response?["type"].Value<string>() == "order" ||
                response?["type"].Value<string>() == "order")
            {
                var parsed = response.ToObject<OrderResponse>(CoinbaseJsonSerializer.Serializer);
                subject.OnNext(parsed);
                return true;
            }

            return false;
        }
    }

    public partial class OrderResponse
    {
        public static OrderResponse[] FromJson(string json) =>
            JsonConvert.DeserializeObject<OrderResponse[]>(json, CoinbaseJsonSerializer.Settings);
    }

    public static partial class Serialize
    {
        public static string ToJson(this OrderResponse[] self) =>
            JsonConvert.SerializeObject(self, CoinbaseJsonSerializer.Settings);
    }
}