using System;
using System.Globalization;
using System.Reactive.Subjects;
using Coinbase.Client.Websocket.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Coinbase.Client.Websocket.Responses
{
    public class StatusResponse : ResponseBase
    {
        [JsonProperty("products")] public Product[] Products { get; set; }

        [JsonProperty("currencies")] public Currency[] Currencies { get; set; }


        internal static bool TryHandle(JObject response, ISubject<StatusResponse> subject)
        {
            if (response?["type"].Value<string>() != "status") return false;

            var parsed = response.ToObject<StatusResponse>(CoinbaseJsonSerializer.Serializer);
            subject.OnNext(parsed);
            return true;
        }
    }

    public class Currency
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("min_size")] public string MinSize { get; set; }

        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("funding_account_id")] public string FundingAccountId { get; set; }

        [JsonProperty("status_message")] public string StatusMessage { get; set; }

        [JsonProperty("max_precision")] public string MaxPrecision { get; set; }

        [JsonProperty("convertible_to")] public string[] ConvertibleTo { get; set; }

        [JsonProperty("details")] public Details Details { get; set; }
    }

    public class Details
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("symbol")] public string Symbol { get; set; }

        [JsonProperty("network_confirmations")]
        public long NetworkConfirmations { get; set; }

        [JsonProperty("sort_order")] public long SortOrder { get; set; }

        [JsonProperty("crypto_address_link")] public string CryptoAddressLink { get; set; }

        [JsonProperty("crypto_transaction_link")]
        public string CryptoTransactionLink { get; set; }

        [JsonProperty("push_payment_methods")] public string[] PushPaymentMethods { get; set; }

        [JsonProperty("processing_time_seconds", NullValueHandling = NullValueHandling.Ignore)]
        public long? ProcessingTimeSeconds { get; set; }

        [JsonProperty("min_withdrawal_amount", NullValueHandling = NullValueHandling.Ignore)]
        public double? MinWithdrawalAmount { get; set; }

        [JsonProperty("group_types", NullValueHandling = NullValueHandling.Ignore)]
        public string[] GroupTypes { get; set; }

        [JsonProperty("display_name", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; }
    }

    public class Product
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("base_currency")] public string BaseCurrency { get; set; }

        [JsonProperty("quote_currency")] public string QuoteCurrency { get; set; }

        [JsonProperty("base_min_size")] public double BaseMinSize { get; set; }

        [JsonProperty("base_max_size")] public double BaseMaxSize { get; set; }

        [JsonProperty("base_increment")] public double BaseIncrement { get; set; }

        [JsonProperty("quote_increment")] public double QuoteIncrement { get; set; }

        [JsonProperty("display_name")] public string DisplayName { get; set; }

        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("margin_enabled")] public bool MarginEnabled { get; set; }

        [JsonProperty("status_message")] public string StatusMessage { get; set; }

        [JsonProperty("min_market_funds")] public double MinMarketFunds { get; set; }

        [JsonProperty("max_market_funds")] public double MaxMarketFunds { get; set; }

        [JsonProperty("post_only")] public bool PostOnly { get; set; }

        [JsonProperty("limit_only")] public bool LimitOnly { get; set; }

        [JsonProperty("cancel_only")] public bool CancelOnly { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    
    }
}