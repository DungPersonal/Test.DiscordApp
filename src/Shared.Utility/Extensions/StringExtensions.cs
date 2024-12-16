using System.Diagnostics.CodeAnalysis;

namespace Test.DiscordApp.Utility.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Determines whether a string is null or empty.
    /// </summary>
    /// <param name="str">string</param>
    /// <returns>bool</returns>
    public static bool IsNullOrEmpty([NotNullWhen(false)]this string str) => string.IsNullOrEmpty(str);

    /// <summary>
    /// Determines whether a string is not null and not empty.
    /// </summary>
    /// <param name="str">string</param>
    /// <returns>bool</returns>
    public static bool IsNotEmpty([NotNullWhen(true)] this string str) => !string.IsNullOrEmpty(str);
}