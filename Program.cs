using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Threading.Tasks;
using TarkovItemBot.Options;
using TarkovItemBot.Services;
using TarkovItemBot.Services.TarkovDatabase;
using TarkovItemBot.Services.TarkovDatabaseSearch;

namespace TarkovItemBot
{
    class Program
    {
        static async Task Main()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(theme: ConsoleTheme.None)
                .CreateLogger();

            var hostBuilder = Host.CreateDefaultBuilder()
                .UseSerilog()
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


                    // TODO: Ratelimit from config
                    services.AddHttpClient<TarkovDatabaseClient>().AddHttpMessageHandler(_ => new RateLimitHandler(350, TimeSpan.FromMinutes(1)))
                        .AddHttpMessageHandler<TarkovDatabaseTokenHandler>();

                    // Tarkov Database Search
                    services.AddHttpClient<TarkovSearchAuthClient>();

                    services.AddScoped<TarkovSearchTokenCache>();
                    services.AddTransient<TarkovSearchTokenHandler>();

                    // TODO: Ratelimit from config
                    services.AddHttpClient<TarkovSearchClient>().AddHttpMessageHandler(_ => new RateLimitHandler(400, TimeSpan.FromMinutes(1)))
                        .AddHttpMessageHandler<TarkovSearchTokenHandler>();

                    services.AddHostedService<CommandHandlingService>();
                    services.AddHostedService<PresenceService>();
                });

            await hostBuilder.RunConsoleAsync();
        }
    }
}
