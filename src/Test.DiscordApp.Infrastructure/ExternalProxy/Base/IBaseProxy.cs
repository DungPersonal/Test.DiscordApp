namespace Test.DiscordApp.Infrastructure.ExternalProxy.Base;

public interface IBaseProxy
{
    Task<T?> GetAsync<T>(
        HttpClient httpClient, string url, Dictionary<string, string>? headers = null, 
        bool isJsonSnakeCase = false, CancellationToken cancellationToken = default
    );
}