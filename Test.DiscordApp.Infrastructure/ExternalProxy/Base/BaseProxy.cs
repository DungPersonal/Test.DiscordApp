using Test.DiscordApp.Domain.Utility;

namespace Test.DiscordApp.Infrastructure.ExternalProxy.Base;

public class BaseProxy: IBaseProxy
{
    public async Task<T?> GetAsync<T>(HttpClient httpClient, string url, Dictionary<string, string>? headers = null,
        string trackId = "", CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
        return responseString.FromJson<T>();
    }
}