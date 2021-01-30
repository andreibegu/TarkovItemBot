using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using TarkovItemBot.Services;

namespace TarkovItemBot
{
    class Program
    {
		static async Task Main()
        {
            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureDiscordHost<DiscordSocketClient>((context, config) =>
                {
                    config.Token = context.Configuration["token"];
                })
                .UseCommandService((context, config) =>
                {
                    config.DefaultRunMode = RunMode.Async;
                    config.CaseSensitiveCommands = false;
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddHostedService<CommandHandlingService>();
                });
            
            await hostBuilder.RunConsoleAsync();
        }
    }
}
