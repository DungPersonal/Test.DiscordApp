using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Test.DiscordApp.Infrastructure.Discord.EventHandler;

public class MessageCreatedEventHandler: IEventHandler<MessageCreatedEventArgs>
{
    public Task HandleEventAsync(DiscordClient sender, MessageCreatedEventArgs eventArgs)
    {
        Console.WriteLine($"Message created: {eventArgs.Message.Content}");
        return Task.CompletedTask;
    }
}