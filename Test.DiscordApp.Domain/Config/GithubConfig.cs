namespace Test.DiscordApp.Domain.Config;

public class GithubConfig
{
    public string User { get; set; } = null!;
    public string RepositoryName { get; set; } = null!;
    public string DefaultBranch { get; set; } = null!;
    public string Token { get; set; } = null!;
}