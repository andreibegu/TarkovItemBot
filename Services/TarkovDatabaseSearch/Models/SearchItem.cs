using System.Web;
using TarkovItemBot.Services.TarkovDatabase;

namespace TarkovItemBot.Services.TarkovDatabaseSearch
{
    public class SearchItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public ItemKind Kind { get; set; }
        public string IconUrl => $"https://static.tarkov-database.com/image/icon/1-1/{Id}.png";
        public string WikiUrl => $"https://escapefromtarkov.gamepedia.com/{HttpUtility.UrlEncode(Name.Replace(" ", "_"))}";
    }
}
