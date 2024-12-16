using System.Net.Http.Headers;
using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Test.DiscordApp.Domain.Config;
using Test.DiscordApp.Domain.Interface;
using Test.DiscordApp.Infrastructure.Discord.EventHandler;
using Test.DiscordApp.Infrastructure.ExternalProxy.Base;
using Test.DiscordApp.Infrastructure.ExternalProxy.Github;

namespace Test.DiscordApp.Infrastructure;

public static class ServicesExtensions
{
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder,
        IConfiguration configuration)
    {
        var connectionConfig = configuration.GetSection("ConnectionStrings").Get<ConnectionConfig>();
        var githubConfig = configuration.GetSection("Github").Get<GithubConfig>();
        if (connectionConfig is null) throw new InvalidOperationException("Connection configuration is missing");
        if (githubConfig is null) throw new InvalidOperationException("Github configuration is missing");

        builder.Services
            .AddSingleton<IBaseProxy, BaseProxy>()
            .AddSingleton<IGithubProxy, GithubProxy>()
            .AddHttpClient(connectionConfig, githubConfig);

        return builder;
    }

    private static void AddHttpClient(this IServiceCollection services, ConnectionConfig connectionConfig,
        GithubConfig githubConfig)
    {
        // Configure Default HttpClient for every proxy
        services.AddHttpClient();

        // Configure Default HttpClient for GithubProxy
        services.AddHttpClient(
            nameof(GithubProxy),
            client =>
            {
                client.BaseAddress = new Uri(connectionConfig.Github);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", githubConfig.Token);
                client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", githubConfig.ApiVersion);
            }
        );
    }

    public static void ConfigureDiscordClient(this IHostBuilder builder, IConfiguration configuration)
    {
        builder.ConfigureServices((_, services) =>
        {
            var config = configuration.GetSection("Discord").Get<DiscordConfig>();
            if (config is null) throw new InvalidOperationException("Discord configuration is missing");

            var discordBuilder = DiscordClientBuilder.CreateDefault(config.Token, DiscordIntents.All);

            discordBuilder
                .AddEventHandlers()
                .AddCommands(config);

            discordBuilder.ConfigureLogging(c => c.AddSerilog());

            var client = discordBuilder.Build();

            services.AddSingleton(client);
            Task.Run(async () =>
            {
                await client.ConnectAsync();
                await Task.Delay(-1);
            });
        });
    }

    private static DiscordClientBuilder AddEventHandlers(this DiscordClientBuilder builder)
    {
        return builder.ConfigureEventHandlers(b =>
            b.AddEventHandlers<MessageCreatedEventHandler>(ServiceLifetime.Singleton));
    }

    private static void AddCommands(this DiscordClientBuilder builder, DiscordConfig config)
    {
        builder.UseCommands((_, extension) =>
        {
            // Add all command types from the current assembly
            var currAssembly = typeof(ServicesExtensions).Assembly;
            var commandTypes = currAssembly.GetTypes()
                .Where(t => typeof(IDiscordCommand).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false })
                .ToArray();
            extension.AddCommands(commandTypes);

            // Add Prefix resolver
            var textCommandProcessor = new TextCommandProcessor(new TextCommandConfiguration
            {
                PrefixResolver = new DefaultPrefixResolver(config.AllowMention, config.Prefix.Split(','))
                    .ResolvePrefixAsync
            });
            extension.AddProcessor(textCommandProcessor);
        });
    }
}