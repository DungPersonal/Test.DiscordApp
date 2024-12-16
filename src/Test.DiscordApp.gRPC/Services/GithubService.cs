using Grpc.Core;

namespace Test.DiscordApp.gRPC.Services;

public class GithubService(
    
): GitHub.GitHubBase
{
    public override Task<GetRepoReply> GetRepo(GetRepoRequest request, ServerCallContext context)
    {
        return base.GetRepo(request, context);
    }
}