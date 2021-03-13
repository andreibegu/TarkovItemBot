using Discord;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using Humanizer;
using Microsoft.Extensions.Options;
using Qmmands;
using Serilog;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TarkovItemBot.Helpers;
using TarkovItemBot.Options;

namespace TarkovItemBot.Services.Commands
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
            _commands.CommandExecutionFailed += CommandExecutionFailedAsync;
        }

        public override Task InitializeAsync(CancellationToken cancellationToken)
        {
            _commands.AddModules(Assembly.GetEntryAssembly());
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            if (rawMessage is not SocketUserMessage message) return;
            if (message.Source != MessageSource.User) return;

            var prefixes = new string[] { _config.Prefix, _discord.CurrentUser.Mention };
            if (!CommandUtilities.HasAnyPrefix(message.Content, prefixes, out _, out var output)) return;

            var context = new DiscordCommandContext(_discord, message, _services);
            var result = await _commands.ExecuteAsync(output, context);

            await HandleResultAsync(result, context);
        }
        
        private static async Task HandleResultAsync(IResult result, DiscordCommandContext context)
        {
            if (result is FailedResult failedResult)
            {
                if (failedResult is CommandNotFoundResult) return;

                var reason = failedResult switch
                {
                    TypeParseFailedResult parseResult =>
                        $"Parameter `{parseResult.Parameter.Name}` has been wrongly provided! " +
                        $"(command usage: `{parseResult.Parameter.Command.GetUsage()}`)",
                    CommandOnCooldownResult cooldownResult =>
                        $"Command is on a per " +
                        $"{cooldownResult.Cooldowns[0].Cooldown.BucketType.Humanize(LetterCasing.LowerCase)} cooldown! " +
                        $"Retry after {cooldownResult.Cooldowns[0].RetryAfter.Humanize()}.",
                    ParameterChecksFailedResult parameterChecksResult =>
                        parameterChecksResult.FailedChecks[0].Result.Reason,
                    ChecksFailedResult checksResult =>
                        checksResult.FailedChecks[0].Result.Reason,
                    _ => failedResult.Reason
                };

                await context.Message.ReplyAsync($"An error occured! {reason}",
                    allowedMentions: AllowedMentions.None);
            }
        }

        private async Task CommandExecutionFailedAsync(CommandExecutionFailedEventArgs args)
        {
            var context = args.Context as DiscordCommandContext;
            var result = args.Result;
            Log.Logger.Error(result.Exception, result.Reason);

            await context.Message.ReplyAsync($"An error occured! {result.Exception.Message}",
                allowedMentions: AllowedMentions.None);
        }
    }
}
