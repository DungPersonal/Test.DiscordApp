using DSharpPlus.Commands;

namespace Test.DiscordApp.Application.Command;

public class NotifyCommand
{
    [Command("notify")]
    public static async ValueTask ExecuteAsync(CommandContext context)
    {
        await context.Channel.SendMessageAsync("@everyone Hello, world!");
    }
}