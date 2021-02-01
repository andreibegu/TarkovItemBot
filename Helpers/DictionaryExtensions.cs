using System.Collections.Generic;
using System.Linq;

namespace TarkovItemBot.Helpers
{
    public static class DictionaryExtensions
    {
        public static string AsQueryString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary.Count == 0)
                return "";
            else
                return $"?{string.Join("&", dictionary.Select(x => $"{x.Key}={x.Value}"))}";
        }
    }
}
