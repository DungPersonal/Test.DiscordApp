using System.Text.Json;
using System.Text.Json.Serialization;

namespace Test.DiscordApp.Domain.Utility;

public static class JsonUtility
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
    };
    
    public static string ToJson (this object obj)
    {
        return ReferenceEquals(obj, null) ? string.Empty : JsonSerializer.Serialize(obj, JsonSerializerOptions);
    }

    public static T? FromJson<T>(this string? json)
    {
        return string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
    }
}