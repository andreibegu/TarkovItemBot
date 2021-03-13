using Qmmands.Delegates;
using System;

namespace TarkovItemBot.Services.Commands
{
    public static class CooldownBucketKeyGenerators
    {
        public static CooldownBucketKeyGeneratorDelegate DiscordDefault => (t, ctx) =>
        {
            var context = ctx as DiscordCommandContext;
            return t switch
            {
                CooldownType.Server => context.Guild.Id,
                CooldownType.Channel => context.Channel.Id,
                CooldownType.User => context.User.Id,
                CooldownType.Global => -1,
                _ => throw new NotImplementedException(),
            };
        };
    }

    public enum CooldownType
    {
        Server,
        Channel,
        User,
        Global
    }
}
