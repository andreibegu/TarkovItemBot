using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TarkovItemBot.Helpers
{
    public static class DictionaryExtensions
    {
        public static string AsQueryString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary.Count == 0)
                return "";
            else
                return $"?{string.Join("&", dictionary.Select(x => $"{HttpUtility.UrlEncode(x.Key.ToString())}={HttpUtility.UrlEncode(x.Value.ToString())}"))}";
        }
    }
}
