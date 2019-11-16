using Coinbase.Client.Websocket.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Reactive.Subjects;

namespace Coinbase.Client.Websocket.Responses
{
    public partial class StatusResponse : ResponseBase
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

    public partial class Currency
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("min_size")]
        public string MinSize { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("funding_account_id")]
        public Guid FundingAccountId { get; set; }

        [JsonProperty("status_message")]
        public string StatusMessage { get; set; }

        [JsonProperty("max_precision")]
        public string MaxPrecision { get; set; }

        [JsonProperty("convertible_to")]
        public QuoteCurrency[] ConvertibleTo { get; set; }

        [JsonProperty("details")]
        public Details Details { get; set; }
    }

    public partial class Details
    {
        [JsonProperty("type")]
        public DetailsType Type { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("network_confirmations")]
        public long NetworkConfirmations { get; set; }

        [JsonProperty("sort_order")]
        public long SortOrder { get; set; }

        [JsonProperty("crypto_address_link")]
        public string CryptoAddressLink { get; set; }

        [JsonProperty("crypto_transaction_link")]
        public string CryptoTransactionLink { get; set; }

        [JsonProperty("push_payment_methods")]
        public string[] PushPaymentMethods { get; set; }

        [JsonProperty("processing_time_seconds", NullValueHandling = NullValueHandling.Ignore)]
        public long? ProcessingTimeSeconds { get; set; }

        [JsonProperty("min_withdrawal_amount", NullValueHandling = NullValueHandling.Ignore)]
        public double? MinWithdrawalAmount { get; set; }

        [JsonProperty("group_types", NullValueHandling = NullValueHandling.Ignore)]
        public string[] GroupTypes { get; set; }

        [JsonProperty("display_name", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; }
    }

    public partial class Product
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("base_currency")]
        public string BaseCurrency { get; set; }

        [JsonProperty("quote_currency")]
        public QuoteCurrency QuoteCurrency { get; set; }

        [JsonProperty("base_min_size")]
        public string BaseMinSize { get; set; }

        [JsonProperty("base_max_size")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long BaseMaxSize { get; set; }

        [JsonProperty("base_increment")]
        public string BaseIncrement { get; set; }

        [JsonProperty("quote_increment")]
        public string QuoteIncrement { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("margin_enabled")]
        public bool MarginEnabled { get; set; }

        [JsonProperty("status_message")]
        public string StatusMessage { get; set; }

        [JsonProperty("min_market_funds")]
        public string MinMarketFunds { get; set; }

        [JsonProperty("max_market_funds")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long MaxMarketFunds { get; set; }

        [JsonProperty("post_only")]
        public bool PostOnly { get; set; }

        [JsonProperty("limit_only")]
        public bool LimitOnly { get; set; }

        [JsonProperty("cancel_only")]
        public bool CancelOnly { get; set; }

        [JsonProperty("type")]
        public ProductType Type { get; set; }
    }

    public enum QuoteCurrency { Btc, Dai, Eth, Eur, Gbp, Usd, Usdc };

    public enum DetailsType { Crypto, Fiat };

    public enum Status { Online };

    public enum ProductType { Spot };

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                QuoteCurrencyConverter.Singleton,
                DetailsTypeConverter.Singleton,
                StatusConverter.Singleton,
                ProductTypeConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class QuoteCurrencyConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(QuoteCurrency) || t == typeof(QuoteCurrency?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "BTC":
                    return QuoteCurrency.Btc;
                case "DAI":
                    return QuoteCurrency.Dai;
                case "ETH":
                    return QuoteCurrency.Eth;
                case "EUR":
                    return QuoteCurrency.Eur;
                case "GBP":
                    return QuoteCurrency.Gbp;
                case "USD":
                    return QuoteCurrency.Usd;
                case "USDC":
                    return QuoteCurrency.Usdc;
            }
            throw new Exception("Cannot unmarshal type QuoteCurrency");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (QuoteCurrency)untypedValue;
            switch (value)
            {
                case QuoteCurrency.Btc:
                    serializer.Serialize(writer, "BTC");
                    return;
                case QuoteCurrency.Dai:
                    serializer.Serialize(writer, "DAI");
                    return;
                case QuoteCurrency.Eth:
                    serializer.Serialize(writer, "ETH");
                    return;
                case QuoteCurrency.Eur:
                    serializer.Serialize(writer, "EUR");
                    return;
                case QuoteCurrency.Gbp:
                    serializer.Serialize(writer, "GBP");
                    return;
                case QuoteCurrency.Usd:
                    serializer.Serialize(writer, "USD");
                    return;
                case QuoteCurrency.Usdc:
                    serializer.Serialize(writer, "USDC");
                    return;
            }
            throw new Exception("Cannot marshal type QuoteCurrency");
        }

        public static readonly QuoteCurrencyConverter Singleton = new QuoteCurrencyConverter();
    }

    internal class DetailsTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(DetailsType) || t == typeof(DetailsType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "crypto":
                    return DetailsType.Crypto;
                case "fiat":
                    return DetailsType.Fiat;
            }
            throw new Exception("Cannot unmarshal type DetailsType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (DetailsType)untypedValue;
            switch (value)
            {
                case DetailsType.Crypto:
                    serializer.Serialize(writer, "crypto");
                    return;
                case DetailsType.Fiat:
                    serializer.Serialize(writer, "fiat");
                    return;
            }
            throw new Exception("Cannot marshal type DetailsType");
        }

        public static readonly DetailsTypeConverter Singleton = new DetailsTypeConverter();
    }

    internal class StatusConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Status) || t == typeof(Status?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "online")
            {
                return Status.Online;
            }
            throw new Exception("Cannot unmarshal type Status");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Status)untypedValue;
            if (value == Status.Online)
            {
                serializer.Serialize(writer, "online");
                return;
            }
            throw new Exception("Cannot marshal type Status");
        }

        public static readonly StatusConverter Singleton = new StatusConverter();
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

    internal class ProductTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ProductType) || t == typeof(ProductType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "spot")
            {
                return ProductType.Spot;
            }
            throw new Exception("Cannot unmarshal type ProductType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ProductType)untypedValue;
            if (value == ProductType.Spot)
            {
                serializer.Serialize(writer, "spot");
                return;
            }
            throw new Exception("Cannot marshal type ProductType");
        }

        public static readonly ProductTypeConverter Singleton = new ProductTypeConverter();
    }
}
