namespace TarkovItemBot.Options
{
    public class TarkovDatabaseOptions
    {
        public string BaseUri { get; set; } = "https://api.tarkov-database.com/v2/";
        public string SearchBaseUri { get; set; } = "https://search.tarkov-database.com/";
        public string Token { get; set; }
        public string SearchToken { get; set; }
    }
}
