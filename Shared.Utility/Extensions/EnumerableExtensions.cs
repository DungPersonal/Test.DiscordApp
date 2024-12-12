using System.Diagnostics.CodeAnalysis;

namespace Test.DiscordApp.Utility.Extensions;

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
}