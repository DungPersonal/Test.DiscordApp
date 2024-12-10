using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Test.DiscordApp.Domain.Config;
using Test.DiscordApp.Infrastructure.ExternalProxy.Github;

namespace Test.DiscordApp.Infrastructure;

public static class ServicesConfiguration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionConfig = configuration.GetSection("ConnectionStrings").Get<ConnectionConfig>();
        var githubConfig = configuration.GetSection("Github").Get<GithubConfig>();
        if (connectionConfig == null)
        {
            throw new ArgumentNullException(nameof(connectionConfig));
        }

        if (githubConfig == null)
        {
            throw new ArgumentNullException(nameof(githubConfig));
        }

        services.AddSingleton<IGithubProxy, GithubProxy>();

        services.AddHttpClient(connectionConfig, githubConfig);

        return services;
    }

    private static void AddHttpClient(this IServiceCollection services, ConnectionConfig connectionConfig,
        GithubConfig githubConfig)
    {
        services.AddHttpClient(
            nameof(GithubProxy),
            client =>
            {
                client.BaseAddress = new Uri(connectionConfig.Github);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", githubConfig.Token);
            }
        );
    }
}