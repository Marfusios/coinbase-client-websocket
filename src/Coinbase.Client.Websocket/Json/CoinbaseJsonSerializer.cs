using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Coinbase.Client.Websocket.Json;

/// <summary>
/// Helper class for JSON serialization
/// </summary>
public static class CoinbaseJsonSerializer
{
    /// <summary>
    /// Custom JSON settings
    /// </summary>
    public static readonly JsonSerializerSettings Settings = new()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        Formatting = Formatting.None,
        Converters = new List<JsonConverter>
        {
            new CoinbaseStringEnumConverter
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        },
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        }
    };

    /// <summary>
    /// Custom preconfigured JSON serializer
    /// </summary>
    public static readonly JsonSerializer Serializer = JsonSerializer.Create(Settings);

    /// <summary>
    /// Deserialize JSON string data by our configuration
    /// </summary>
    public static T Deserialize<T>(string data)
    {
        return JsonConvert.DeserializeObject<T>(data, Settings);
    }

    /// <summary>
    /// Serialize object into JSON by our configuration
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string Serialize(object data)
    {
        return JsonConvert.SerializeObject(data, Settings);
    }

    internal static ILogger Logger = NullLogger.Instance;
}