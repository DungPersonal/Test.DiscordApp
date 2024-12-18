using Grpc.Core;
using MediatR;
using Test.DiscordApp.Application.Github;
using Google.Protobuf.WellKnownTypes;
using Shared.Protos;

namespace Test.DiscordApp.gRPC.Services;

public class GithubService(
    ISender sender
): GitHub.GitHubBase
{
    public override async Task<GetRepoReply> GetRepo(GetRepoRequest request, ServerCallContext context)
    {
        var getLatestCommitRequest = new GetLatestCommitRequest(
            Owner: request.Owner,
            Repo: request.Repo,
            Ref: request.Ref,
            Branch: request.Branch
        );
        var result = await sender.Send(getLatestCommitRequest, context.CancellationToken);
        if (result.IsFailure)
            return new GetRepoReply();

        var data = result.Value;
        return new GetRepoReply
        {
            Sha = data.Sha,
            Commit = new CommitDetails
            {
                Author = new CommitAuthor
                {
                    Name = data.Commit?.Author?.Name,
                    Email = data.Commit?.Author?.Email,
                    Date = data.Commit?.Author?.Date?.ToTimestamp() ?? new Timestamp()
                },
                Message = data.Commit?.Message,
                Url = data.Commit?.Url
            },
            Url = data.Url,
            HtmlUrl = data.HtmlUrl
        };
    }
}