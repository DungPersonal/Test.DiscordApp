using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedKernel.Utility.Extensions;

public static class JsonExtensions
{
    private static readonly JsonSerializerOptions CamelCaseJsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
    };

    private static readonly JsonSerializerOptions SnakeCaseJsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
    };

    /// <summary>
    /// Serializes an object to a JSON string.
    /// </summary>
    /// <param name="obj">Object</param>
    /// <param name="isSnakeCase">A Boolean determine if to use snakeCase option</param>
    /// <returns>JSON string</returns>
    public static string ToJson(this object obj, bool isSnakeCase = false)
    {
        return ReferenceEquals(obj, null)
            ? string.Empty
            : JsonSerializer.Serialize(obj, GetJsonSerializerOptions(isSnakeCase));
    }

    /// <summary>
    /// Deserializes a JSON string to an object.
    /// </summary>
    /// <param name="json">JSON string</param>
    /// <param name="isSnakeCase">A Boolean determine if to use snakeCase option</param>
    /// <typeparam name="T">Returned Generic type T</typeparam>
    /// <returns>Object of type T</returns>
    public static T? FromJson<T>(this string? json, bool isSnakeCase = false)
    {
        return string.IsNullOrEmpty(json)
            ? default
            : JsonSerializer.Deserialize<T>(json, GetJsonSerializerOptions(isSnakeCase));
    }
    
    private static JsonSerializerOptions GetJsonSerializerOptions(bool isSnakeCase) => isSnakeCase
        ? SnakeCaseJsonSerializerOptions
        : CamelCaseJsonSerializerOptions;
}