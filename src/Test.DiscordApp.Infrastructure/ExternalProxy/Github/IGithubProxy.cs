using Test.DiscordApp.Domain.Proxy.Github.Request;
using Test.DiscordApp.Domain.Proxy.Github.Response;

namespace Test.DiscordApp.Infrastructure.ExternalProxy.Github;

public interface IGithubProxy
{
    Task<(GithubCommitResponse? Data, string Error)> GetLatestCommit(GithubCommitRequest request,
        CancellationToken cancellationToken = default);
}