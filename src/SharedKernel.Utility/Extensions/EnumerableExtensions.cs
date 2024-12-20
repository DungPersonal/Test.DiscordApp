using System.Diagnostics.CodeAnalysis;

namespace SharedKernel.Utility.Extensions;

public static class EnumerableExtensions
{
    /// <summary>
    /// Determines whether an enumerable is null or empty.
    /// </summary>
    /// <param name="enumerable"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? enumerable)
        where T : IEnumerable<T> => enumerable is null || !enumerable.Any();

    /// <summary>
    /// Determines whether an enumerable is not null and not empty.
    /// </summary>
    /// <param name="enumerable"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool IsNotEmpty<T>([NotNullWhen(true)] this IEnumerable<T>? enumerable) where T : IEnumerable<T> =>
        enumerable is not null && enumerable.Any();
    
    /// <summary>
    /// Converts an Enumerable to a string with a separator.
    /// </summary>
    /// <param name="enumerable">An Enumerable of type T</param>
    /// <param name="separator">A string separator for each element</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>A string represent a list of separated elements</returns>
    public static string ToJoinString<T>(this IEnumerable<T>? enumerable, string separator = ",") => 
        enumerable is not null ? string.Join(separator, enumerable): string.Empty;
}