namespace TarkovItemBot.Options
{
    public class BotOptions
    {
        public string Token { get; set; }
        public string Prefix { get; set; } = "t!";
        public bool EnableReactionChoice { get; set; } = true;
    }
}
