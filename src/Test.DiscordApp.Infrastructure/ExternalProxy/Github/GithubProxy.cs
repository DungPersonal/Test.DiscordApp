using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Test.DiscordApp.Domain.Config;
using Test.DiscordApp.Domain.Proxy.Github.Request;
using Test.DiscordApp.Domain.Proxy.Github.Response;
using Test.DiscordApp.Infrastructure.ExternalProxy.Base;
using SharedKernel.Utility.Extensions;

namespace Test.DiscordApp.Infrastructure.ExternalProxy.Github;

public class GithubProxy(
    IHttpClientFactory httpClientFactory,
    IOptions<GithubConfig> githubConfig,
    IBaseProxy baseProxy,
    ILogger<GithubProxy> logger
) : IGithubProxy
{
    private HttpClient HttpClient => httpClientFactory.CreateClient(nameof(GithubProxy));

    public async Task<(GithubCommitResponse? Data, string Error)> GetLatestCommit(GithubCommitRequest request,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            new Dictionary<string, object> { ["Request"] = request },
            "GithubProxy.GetLatestCommit - Start"
        );
        var owner = request.Owner ?? githubConfig.Value.User;
        var repo = request.Repo ?? githubConfig.Value.RepositoryName;
        var url = $"repos/{owner}/{repo}/commits/{request.Ref}";
        var response = await baseProxy.GetAsync<GithubCommitResponse>(HttpClient, url, isJsonSnakeCase: true,
            cancellationToken: cancellationToken);

        return response is null ? (null, "Failed to get latest commit") : (response, string.Empty);
    }
}