using DSharpPlus.Commands;
using Microsoft.Extensions.Logging;
using Test.DiscordApp.Domain.Interface;
using Test.DiscordApp.Infrastructure.ExternalProxy.Github;

namespace Test.DiscordApp.Infrastructure.Discord.Command;

[Command("github")]
public class GithubCommand(
    ILogger<GithubCommand> logger,
    IGithubProxy githubProxy
): IDiscordCommand
{
    [Command("latest-commit")]
    public static async ValueTask GetLatestCommitAsync(CommandContext context)
    {
        await context.Channel.SendMessageAsync("@everyone Hello, world!");
    }
}