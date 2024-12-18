namespace Test.DiscordApp.Domain.Proxy.Github.Request;

[Serializable]
public class GithubCommitRequest
{
    private int _perPage = 20;
    private int _page = 1;
    private string? _ref;

    public string? Owner { get; set; }
    public string? Repo { get; set; }
    public string? Sha { get; set; }
    public string? Path { get; set; }
    public string? Author { get; set; }
    public string? Committer { get; set; }
    public DateTime? Since { get; set; }
    public DateTime? Until { get; set; }
    public string? Branch { get; set; }

    public string? Ref
    {
        get => _ref ?? (Branch is null ? null : $"refs/heads/{Branch}");
        set => _ref = value;
    }

    public int PerPage
    {
        get => _perPage;
        set => _perPage = value < 1 ? 1 : value;
    }

    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }
}