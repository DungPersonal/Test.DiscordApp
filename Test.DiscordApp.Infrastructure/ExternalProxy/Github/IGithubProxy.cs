using Test.DiscordApp.Domain.Proxy.Github.Response;

namespace Test.DiscordApp.Infrastructure.ExternalProxy.Github;

public interface IGithubProxy
{
    Task<(GithubCommit? Data, string Error)> GetLatestCommit(string branch = "", string trackId = "",
        CancellationToken cancellationToken = default);
}