namespace Test.DiscordApp.Domain.Config;

public class DiscordConfig
{
    public string Prefix { get; init; } = "!";
    public bool AllowMention { get; init; } = true;
    public string Token { get; init; } = null!;
    public string ChannelId { get; init; } = null!;
}