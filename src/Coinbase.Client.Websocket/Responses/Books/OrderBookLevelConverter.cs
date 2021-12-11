using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Coinbase.Client.Websocket.Responses.Books;

class OrderBookLevelConverter : JsonConverter
{
    readonly OrderBookSide _side;

    public OrderBookLevelConverter() { }

    public OrderBookLevelConverter(OrderBookSide side)
    {
        _side = side;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(double[][]);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
        JsonSerializer serializer)
    {
        var array = JArray.Load(reader);
        return JArrayToTradingTicker(array);
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    OrderBookLevel[] JArrayToTradingTicker(JArray data)
    {
        var result = new List<OrderBookLevel>();
        foreach (var item in data)
        {
            var array = item.ToArray();

            var level = new OrderBookLevel();

            if (array.Length == 2)
            {
                level.Side = _side;
                level.Price = (double) array[0];
                level.Amount = (double) array[1];
            }
            else
            {
                var side = (string) array[0];
                level.Side = string.IsNullOrWhiteSpace(side) ? OrderBookSide.Undefined : 
                    side == "buy" ? OrderBookSide.Buy : OrderBookSide.Sell;
                level.Price = (double) array[1];
                level.Amount = (double) array[2];
            }

            result.Add(level);
        }

        return result.ToArray();
    }
}