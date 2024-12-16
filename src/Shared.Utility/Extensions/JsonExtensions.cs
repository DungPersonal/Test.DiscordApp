using System.Text.Json;
using System.Text.Json.Serialization;

namespace Test.DiscordApp.Utility.Extensions;

public static class JsonExtensions
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
    };
    
    /// <summary>
    /// Serializes an object to a JSON string.
    /// </summary>
    /// <param name="obj">Object</param>
    /// <returns>JSON string</returns>
    public static string ToJson (this object obj)
    {
        return ReferenceEquals(obj, null) ? string.Empty : JsonSerializer.Serialize(obj, JsonSerializerOptions);
    }

    /// <summary>
    /// Deserializes a JSON string to an object.
    /// </summary>
    /// <param name="json">JSON string</param>
    /// <typeparam name="T">Returned Generic type T</typeparam>
    /// <returns>Object of type T</returns>
    public static T? FromJson<T>(this string? json)
    {
        return string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
    }
}