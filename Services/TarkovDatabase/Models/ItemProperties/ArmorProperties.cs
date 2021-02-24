using System.Collections.Generic;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public record ArmorProperties(int Class, float Durability, MaterialProperties Material, float BluntThroughput, IReadOnlyCollection<string> Zones);
}
