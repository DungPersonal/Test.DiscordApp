using DSharpPlus.Commands;
using Test.DiscordApp.Domain.Interface;

namespace Test.DiscordApp.Infrastructure.Discord.Command;

[Command("k8s")]
public class KubernetesCommand: IDiscordCommand
{
    [Command("health")]
    public static async ValueTask GetKubernetesHealthAsync(CommandContext context)
    {
        await context.Channel.SendMessageAsync("@everyone Hello, world!");
    }
}