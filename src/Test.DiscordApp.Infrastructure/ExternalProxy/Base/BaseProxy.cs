using Microsoft.Extensions.Logging;
using SharedKernel.Utility.Extensions;

namespace Test.DiscordApp.Infrastructure.ExternalProxy.Base;

public class BaseProxy(
    ILogger<BaseProxy> logger
): IBaseProxy
{
    public async Task<T?> GetAsync<T>(
        HttpClient httpClient, string url, Dictionary<string, string>? headers = null,
        bool isJsonSnakeCase = false, CancellationToken cancellationToken = default)
    {
        try
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
            logger.LogInformation("GET {URL} - {STATUSCODE}", url, response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
            return responseString.FromJson<T>(isJsonSnakeCase);
        }
        catch (OperationCanceledException ex)
        {
            logger.LogWarning(ex,"GET {URL} - OperationCanceled {EXCEPTION}", url, ex.Message);
            return default;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GET {URL} - Failed to get response", url);
            return default;
        }
    }
}