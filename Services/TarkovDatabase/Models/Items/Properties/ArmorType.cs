using System.Text.Json.Serialization;

namespace TarkovItemBot.Services.TarkovDatabase
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ArmorType
    {
        Body,
        Helmet,
        Attachment,
        Visor,
        FaceCover
    }
}
