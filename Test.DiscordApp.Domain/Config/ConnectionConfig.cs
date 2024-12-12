namespace Test.DiscordApp.Domain.Config;

public record ConnectionConfig
{
    public string Github { get; init; } = null!;
}