using DSharpPlus.Commands;
using Test.DiscordApp.Domain.Interface;

namespace Test.DiscordApp.Infrastructure.Discord.Command;

public class NotifyCommand: IDiscordCommand
{
    [Command("notify")]
    public static async ValueTask NotifyEveryoneAsync(CommandContext context)
    {
        await context.Channel.SendMessageAsync("@everyone Hello, world!");
    }
}