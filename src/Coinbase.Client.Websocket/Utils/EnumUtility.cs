using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Coinbase.Client.Websocket.Utils
{
    /// <summary>
    ///     Utility to get name from the enum type
    /// </summary>
    public static class EnumUtility
    {
        /// <summary>
        ///     Returns string representation of enum type
        /// </summary>
        public static string GetStringValue(this Enum e)
        {
            return e.GetAttribute<EnumMemberAttribute>().Value;
        }

        public static T GetAttribute<T>(this Enum e) where T : Attribute
        {
            return (T) e.GetType().GetFields().First(x => x.Name == e.ToString()).GetCustomAttributes(typeof(T), true)
                .First();
        }

        public static T GetFieldByStringValue<T>(this T t, string expected)
        {
            var fields = typeof(T).GetFields().ToList();

            foreach (var fieldInfo in fields)
            {
                var stringValueAttribute =
                    (EnumMemberAttribute) fieldInfo.GetCustomAttributes(typeof(EnumMemberAttribute), true)
                        .FirstOrDefault();

                if (stringValueAttribute != null)
                {
                    if (stringValueAttribute.Value == expected) return (T) fieldInfo.GetValue(t);
                }
            }

            return default;
        }
    }
}