namespace Test.DiscordApp.Domain.Proxy.Github.Response;

[Serializable]
public class GithubCommitResponse
{
    public string? Sha { get; set; }
    public CommitDetails? Commit { get; set; }
    public string? Url { get; set; }
    public string? HtmlUrl { get; set; }
}

[Serializable]
public class CommitDetails
{
    public CommitAuthor? Author { get; set; }
    public string? Message { get; set; }
    public string? Url { get; set; }
}

[Serializable]
public class CommitAuthor
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public DateTimeOffset? Date { get; set; }   
}