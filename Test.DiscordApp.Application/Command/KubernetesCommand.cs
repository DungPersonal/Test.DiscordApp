using DSharpPlus.Commands;

namespace Test.DiscordApp.Application.Command;

[Command("k8s")]
public class KubernetesCommand
{
    [Command("health")]
    public static async ValueTask ExecuteAsync(CommandContext context)
    {
        await context.Channel.SendMessageAsync("@everyone Hello, world!");
    }
}