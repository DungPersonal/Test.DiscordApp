namespace Test.DiscordApp.Domain.Proxy.Github.Request;

[Serializable]
public class CommitRequest
{
    private int _perPage = 20;
    private int _page = 1;
    
    public string? Sha { get; set; }
    public string? Path { get; set; }
    public string? Author { get; set; }
    public string? Committer { get; set; }
    public DateTime? Since { get; set; }
    public DateTime? Until { get; set; }
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