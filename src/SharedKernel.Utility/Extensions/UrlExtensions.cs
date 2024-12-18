using System.Collections;
using Microsoft.AspNetCore.WebUtilities;

namespace SharedKernel.Utility.Extensions;

public static class UrlExtensions
{
    public static Uri AddUrlQueryParam(this Uri baseUrl, params (string Key, object? Value)[] queryParam)
    {
        
        var filteredParams = queryParam
            .Where(pair => !string.IsNullOrWhiteSpace(pair.Key)
                           && pair.Value is not null
                           && !string.IsNullOrWhiteSpace(pair.Value?.ToString()))
            .ToDictionary(pair => pair.Key, pair => pair.Value.ConvertObjectToString());

        var urlWithQuery = QueryHelpers.AddQueryString(baseUrl.ToString(), filteredParams);

        return ToUri(urlWithQuery);
    }

    public static string? ConvertObjectToString(this object? value)
    {
        return value switch {
            null => string.Empty,
            string str => str,
            IList list => list.Count > 0 ? string.Join(",", list) : string.Empty,
            IEnumerable enumerable => string.Join(",", enumerable),
            _ => value.ToString() ?? string.Empty
        };
    }
    
    public static Uri ToUri(this string? str) => new(str ?? string.Empty);
}