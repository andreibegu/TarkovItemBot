using System.ComponentModel;
using System.Text.Json.Serialization;

namespace TarkovItemBot.Services.TarkovTools
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Currency
    {
        [Description("₽")]
        RUB,
        [Description("$")]
        USD,
        [Description("€")]
        EUR
    }
}
