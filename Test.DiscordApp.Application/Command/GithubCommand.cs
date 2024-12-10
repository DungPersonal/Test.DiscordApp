using DSharpPlus.Commands;

namespace Test.DiscordApp.Application.Command;

[Command("github")]
public class GithubCommand
{
    [Command("latest-commit")]
    public static async ValueTask ExecuteAsync(CommandContext context)
    {
        await context.Channel.SendMessageAsync("@everyone Hello, world!");
    }
}