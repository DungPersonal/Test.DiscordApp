using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;
using Test.DiscordApp.Application.Command;
using Test.DiscordApp.Application.EventHandler;
using Test.DiscordApp.Application.Services;
using Test.DiscordApp.Domain.Config;
using Test.DiscordApp.Infrastructure;

namespace Test.DiscordApp.Application;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("Program");
        logger.LogInformation("init TestDiscordApp.Application Program");
        await CreateGrpcClient(args, logger);
    }

    private static async Task CreateGrpcClient(string[] args, ILogger logger)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets(typeof(Program).Assembly, optional: false)
            .AddEnvironmentVariables();
        
        builder.Services
            .Configure<DiscordConfig>(builder.Configuration.GetSection("Discord"))
            .Configure<GithubConfig>(builder.Configuration.GetSection("Github"))
            .AddSingleton(TimeProvider.System)
            .AddInfrastructureServices(builder.Configuration)
            .AddGrpc();

        await builder.Host.CreateDiscordClient(builder.Configuration, logger);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<GreeterService>();
        app.MapGet("/",
            () =>
                "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        await app.RunAsync();
    }

    private static async Task CreateDiscordClient(this IHostBuilder _, IConfiguration configuration, ILogger logger)
    {
        try
        {
            var config = configuration.GetSection("Discord").Get<DiscordConfig>();
            if (config == null)
            {
                logger.LogError("Discord configuration is missing");
                throw new ArgumentNullException(nameof(config));
            }

            var discordBuilder = DiscordClientBuilder.CreateDefault(config.Token, DiscordIntents.All);

            discordBuilder
                .AddEventHandlers()
                .AddCommands();
            
            var client = discordBuilder.Build();
            
            await client.ConnectAsync();
            await Task.Delay(-1);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to create Discord client");
            throw;
        }
    }

    private static DiscordClientBuilder AddEventHandlers(this DiscordClientBuilder builder)
    {
        builder.ConfigureEventHandlers(b => b.AddEventHandlers<MessageCreatedEventHandler>(ServiceLifetime.Singleton));
        return builder;
    }

    private static void AddCommands(this DiscordClientBuilder builder)
    {
        builder.UseCommands((_, extension) =>
        {
            extension.AddCommands([typeof(PingCommand), typeof(NotifyCommand)]);

            var textCommandProcessor = new TextCommandProcessor(new TextCommandConfiguration
            {
                PrefixResolver = new DefaultPrefixResolver(true, "!").ResolvePrefixAsync
            });
            extension.AddProcessor(textCommandProcessor);
        });
    }
}