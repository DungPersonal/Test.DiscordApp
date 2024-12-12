namespace Test.DiscordApp.Domain.Config;

public record GithubConfig
{
    public string User { get; init; } = null!;
    public string RepositoryName { get; init; } = null!;
    public string DefaultBranch { get; init; } = null!;
    public string Token { get; init; } = null!;
}