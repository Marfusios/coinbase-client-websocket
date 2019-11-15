using System;
using System.Collections.Generic;

using System.Globalization;
using System.Reactive.Subjects;
using Coinbase.Client.Websocket.Json;
using Coinbase.Client.Websocket.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Coinbase.Client.Websocket.Responses
{
    public class StatusResponse : ResponseBase
    {
        [JsonProperty("products")]
        public Product[] Products { get; set; }

        [JsonProperty("currencies")]
        public Currency[] Currencies { get; set; }

        internal static bool TryHandle(JObject response, ISubject<StatusResponse> subject)
        {
            if (response?["type"].Value<string>() != "status")
            {
                return false;
            }

            StatusResponse parsed = response.ToObject<StatusResponse>(CoinbaseJsonSerializer.Serializer);
            subject.OnNext(parsed);
            return true;
        }
    }

    public class Currency
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("min_size")]
        public string MinSize { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("status_message")] public object StatusMessage { get; set; }

        [JsonProperty("max_precision")]
        public string MaxPrecision { get; set; }

        [JsonProperty("convertible_to")]
        public string[] ConvertibleTo { get; set; }

        [JsonProperty("details")]
        public Details Details { get; set; }
    }

    public class Details
    {
    }

    public class Product
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("base_currency")]
        public string BaseCurrency { get; set; }

        [JsonProperty("quote_currency")]
        public string QuoteCurrency { get; set; }

        [JsonProperty("base_min_size")]
        public string BaseMinSize { get; set; }

        [JsonProperty("base_max_size")]
        public long? BaseMaxSize { get; set; }

        [JsonProperty("base_increment")]
        public string BaseIncrement { get; set; }

        [JsonProperty("quote_increment")]
        public string QuoteIncrement { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("status_message")] public object StatusMessage { get; set; }

        [JsonProperty("min_market_funds")]
        public long? MinMarketFunds { get; set; }

        [JsonProperty("max_market_funds")]
        public long? MaxMarketFunds { get; set; }

        [JsonProperty("post_only")]
        public bool? PostOnly { get; set; }

        [JsonProperty("limit_only")]
        public bool? LimitOnly { get; set; }

        [JsonProperty("cancel_only")]
        public bool? CancelOnly { get; set; }
    }
}