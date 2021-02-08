using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TarkovItemBot.Helpers;
using TarkovItemBot.Options;

namespace TarkovItemBot.Services
{
    public class CommandHandlingService : InitializedService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly BotOptions _config;

        public CommandHandlingService(IServiceProvider services, DiscordSocketClient client, CommandService commandService, IOptions<BotOptions> config)
        {
            _commands = commandService;
            _discord = client;
            _config = config.Value;
            _services = services;

            _discord.MessageReceived += MessageReceivedAsync;
            _commands.CommandExecuted += CommandExecutedAsync;
        }
        public override async Task InitializeAsync(CancellationToken cancellationToken)
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            if (rawMessage is not SocketUserMessage message) return;
            if (message.Source != MessageSource.User) return;

            var argPos = 0;
            if (!message.HasStringPrefix(_config.Prefix, ref argPos) && !message.HasMentionPrefix(_discord.CurrentUser, ref argPos)) return;

            var context = new SocketCommandContext(_discord, message);
            await _commands.ExecuteAsync(context, argPos, _services);
        }

        private async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (result.IsSuccess) return;
            if (result.Error == CommandError.UnknownCommand) return;

            string message = result switch
            {
                ParseResult parseResult => $"Parameter `{parseResult.ErrorParameter.Name}` has been wrongly provided! " +
                    $"(command usage: `{_config.Prefix}{parseResult.ErrorParameter.Command.GetUsage()}`)",
                _ => result.ErrorReason
            };

            await context.Message.ReplyAsync($"An error occured! {message}", allowedMentions: AllowedMentions.None);
        }
    }
}
