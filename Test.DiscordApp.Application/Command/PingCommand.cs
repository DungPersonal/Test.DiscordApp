using DSharpPlus.Commands;

namespace Test.DiscordApp.Application.Command;

public class PingCommand
{
    [Command("ping")]
    public static async ValueTask ExecuteAsync(CommandContext context) =>
        await context.RespondAsync($"Pong!");
}