using DSharpPlus.Commands;
using Test.DiscordApp.Domain.Interface;

namespace Test.DiscordApp.Infrastructure.Discord.Command;

public class PingCommand: IDiscordCommand
{
    [Command("ping")]
    public static async ValueTask ExecuteAsync(CommandContext context) =>
        await context.RespondAsync($"Pong!");
}