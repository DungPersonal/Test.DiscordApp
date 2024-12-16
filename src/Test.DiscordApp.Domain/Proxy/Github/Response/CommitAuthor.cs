namespace Test.DiscordApp.Domain.Proxy.Github.Response;

[Serializable]
public record CommitAuthor(string Name, DateTimeOffset Date);