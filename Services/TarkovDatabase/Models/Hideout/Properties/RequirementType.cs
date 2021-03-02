using System.Text.Json.Serialization;

namespace TarkovItemBot.Services.TarkovDatabase
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RequirementType
    {
        Loyalty,
        Skill
    }
}
