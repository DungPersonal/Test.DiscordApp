namespace Test.DiscordApp.Domain.Proxy.Github.Response;

[Serializable]
public record GithubCommit(
    string Sha, 
    CommitDetails Commit
);