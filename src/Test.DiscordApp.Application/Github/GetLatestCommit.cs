using Mapster;
using Microsoft.Extensions.Logging;
using SharedKernel.Model.Abstraction.Messaging.Query;
using SharedKernel.Model.Base;
using Test.DiscordApp.Domain.Proxy.Github.Request;
using Test.DiscordApp.Infrastructure.ExternalProxy.Github;
using SharedKernel.Utility.Extensions;

namespace Test.DiscordApp.Application.Github;

public class GetLatestCommitHandler(
    ILogger<GetLatestCommitHandler> logger,
    IGithubProxy githubProxy
) : IQueryHandler<GetLatestCommitRequest, GetLatestCommitResponse>
{
    public async Task<Result<GetLatestCommitResponse>> Handle(GetLatestCommitRequest request, CancellationToken cancellationToken)
    {
        var commitRequest = new GithubCommitRequest
        {
            Owner = request.Owner,
            Repo = request.Repo,
            Ref = request.Ref,
            Branch = request.Branch
        };
        var (data, error) = await githubProxy.GetLatestCommit(commitRequest, cancellationToken);
        if (error.IsNotEmpty())
        {
            logger.LogInformation(
                new Dictionary<string, object> { ["Error"] = error },
                "GithubService.GetRepo - Failed to get latest commit"
            );
            return Result.Failure<GetLatestCommitResponse>(Error.Failure(description: error));
        }

        if (data is null)
        {
            logger.LogInformation("GithubService.GetRepo - No data found");
            return Result.Failure<GetLatestCommitResponse>(Error.NotFound(description: "No data found"));
        }
        
        return Result.Success(data.Adapt<GetLatestCommitResponse>());
    }
}

public record GetLatestCommitRequest(
    string? Owner,
    string? Repo,
    string Ref,
    string? Branch
) : IQuery<GetLatestCommitResponse>;

[Serializable]
public record GetLatestCommitResponse(
    string? Sha,
    CommitDetailsDto? Commit,
    string? Url,
    string? HtmlUrl
);

[Serializable]
public record CommitDetailsDto(
    CommitAuthorDto? Author,
    string? Message,
    string? Url
);

[Serializable]
public record CommitAuthorDto(
    string? Name,
    string? Email,
    DateTimeOffset? Date
);