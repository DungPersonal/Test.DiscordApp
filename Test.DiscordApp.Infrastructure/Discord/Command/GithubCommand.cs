using DSharpPlus.Commands;
using Test.DiscordApp.Domain.Interface;

namespace Test.DiscordApp.Infrastructure.Discord.Command;

[Command("github")]
public class GithubCommand: IDiscordCommand
{
    [Command("latest-commit")]
    public static async ValueTask GetLatestCommitAsync(CommandContext context)
    {
        await context.Channel.SendMessageAsync("@everyone Hello, world!");
    }
}