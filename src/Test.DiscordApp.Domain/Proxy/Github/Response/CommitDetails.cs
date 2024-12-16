namespace Test.DiscordApp.Domain.Proxy.Github.Response;

[Serializable]
public record CommitDetails(CommitAuthor Author, string Message);