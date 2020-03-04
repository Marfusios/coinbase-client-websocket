﻿using System;
using Coinbase.Client.Websocket.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Coinbase.Client.Websocket.Json
{
    /// <summary>
    /// Extended StringEnumConverter to use first value ('Unknown') as default and log unexpected type
    /// </summary>
    public class CoinbaseStringEnumConverter : StringEnumConverter
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            try
            {
                var val = reader.Value;
                if (val is string valS && string.IsNullOrWhiteSpace(valS))
                {
                    // received empty string, can't parse to enum, use default enum value (first)
                    return existingValue;
                }

                return base.ReadJson(reader, objectType, existingValue, serializer);
            }
            catch
            {
                Log.Warn(
                    $"Can't parse enum, value: {reader.Value}, target type: {objectType}, using default '{existingValue}'");
                return existingValue;
            }
        }
    }
}