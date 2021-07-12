using System;
using System.Globalization;
using Coinbase.Client.Websocket.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Coinbase.Client.Websocket.Responses.Orders
{
    public partial class Order
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("price")] public double? Price { get; set; }

        [JsonProperty("size")] public string Size { get; set; }

        [JsonProperty("product_id")] public string ProductId { get; set; }

        [JsonProperty("profile_id")] public string ProfileId { get; set; }

        [JsonProperty("side")] public string Side { get; set; }

        [JsonProperty("type")] public OrderType Type { get; set; }

        [JsonProperty("time_in_force")] public string TimeInForce { get; set; }

        [JsonProperty("post_only")] public bool PostOnly { get; set; }

        [JsonProperty("created_at")] public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("fill_fees")] public string FillFees { get; set; }

        [JsonProperty("filled_size")] public string FilledSize { get; set; }

        [JsonProperty("executed_value")] public string ExecutedValue { get; set; }

        [JsonProperty("status")] public OrderStatus Status { get; set; }

        [JsonProperty("settled")] public bool Settled { get; set; }
    }

    public partial class Order
    {
        public static Order[] FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Order[]>(json, CoinbaseJsonSerializer.Settings);
        }
    }

    public static partial class Serialize
    {
        public static string ToJson(this Order[] self)
        {
            return JsonConvert.SerializeObject(self, CoinbaseJsonSerializer.Settings);
        }
    }
}