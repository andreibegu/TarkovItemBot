using System.Collections.Generic;

namespace TarkovItemBot.Services
{
    public record ItemsInfo(int Total, int Modified, Dictionary<ItemKind, KindInfo> Kinds);

    public record KindInfo(int Count, int Modified);

}
