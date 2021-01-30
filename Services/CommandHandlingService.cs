using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace TarkovItemBot.Services
{
    public class CommandHandlingService : InitializedService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly IConfiguration _config;

        public CommandHandlingService(IServiceProvider services, DiscordSocketClient client, CommandService commandService, IConfiguration config)
        {
            _commands = commandService;
            _discord = client;
            _config = config;
            _services = services;

            _discord.MessageReceived += MessageReceivedAsync;
            _commands.CommandExecuted += CommandExecutedAsync;
        }
        public override async Task InitializeAsync(CancellationToken cancellationToken)
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(),_services);
        }

        private async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            var argPos = 0;
            if (!message.HasStringPrefix(_config["prefix"], ref argPos) && !message.HasMentionPrefix(_discord.CurrentUser, ref argPos)) return;

            var context = new SocketCommandContext(_discord, message);
            await _commands.ExecuteAsync(context, argPos, _services);
        }

        private async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!command.IsSpecified || result.IsSuccess) return;

            await context.Channel.SendMessageAsync($"Error: {result}");
        }
    }
}
