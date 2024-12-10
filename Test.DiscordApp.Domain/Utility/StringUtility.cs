using System.Diagnostics.CodeAnalysis;

namespace Test.DiscordApp.Domain.Utility;

public static class StringUtility
{
    public static bool IsNullOrEmpty([NotNullWhen(false)]this string str) => string.IsNullOrEmpty(str);

    public static bool IsNotEmpty([NotNullWhen(true)] this string str) => !string.IsNullOrEmpty(str);
}