using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Interactivity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using TarkovItemBot.Options;
using TarkovItemBot.Services;

namespace TarkovItemBot
{
    class Program
    {
        static async Task Main()
        {
            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddEnvironmentVariables("TarkovItemBot_");
                })
                .ConfigureDiscordHost<DiscordSocketClient>((context, config) =>
                {
                    config.Token = context.Configuration["Bot:Token"];
                })
                .UseCommandService((context, config) =>
                {
                    config.DefaultRunMode = RunMode.Async;
                    config.CaseSensitiveCommands = false;
                })
                .ConfigureServices((context, services) =>
                {
                    //Config
                    services.Configure<BotOptions>(context.Configuration.GetSection("Bot"));
                    services.Configure<TarkovDatabaseOptions>(context.Configuration.GetSection("TarkovDatabase"));

                    // Cache
                    services.AddMemoryCache();

                    // Tarkov Database
                    services.AddHttpClient<TarkovDatabaseAuthClient>();

                    services.AddScoped<TarkovDatabaseTokenCache>();
                    services.AddTransient<TarkovDatabaseTokenHandler>();

                    services.AddHttpClient<TarkovDatabaseClient>().AddHttpMessageHandler<TarkovDatabaseTokenHandler>();

                    // Tarkov Database Search
                    services.AddHttpClient<TarkovSearchAuthClient>();

                    services.AddScoped<TarkovSearchTokenCache>();
                    services.AddTransient<TarkovSearchTokenHandler>();

                    services.AddHttpClient<TarkovSearchClient>().AddHttpMessageHandler<TarkovSearchTokenHandler>();

                    services.AddHostedService<CommandHandlingService>();

                    // Interactive
                    services.AddSingleton<InteractivityService>();
                });

            await hostBuilder.RunConsoleAsync();
        }
    }
}
