using Microsoft.Extensions.Logging;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;

namespace SharedKernel.Utility.Extensions;

public static class LoggerExtensions
{
    #region LogMessage Template

    private static readonly Action<ILogger, string, Exception?> LogInformationMessage =
        LoggerMessage.Define<string>(LogLevel.Information, new EventId(0, nameof(LogInformationMessage)), "{Message}");

    private static readonly Action<ILogger, string, Exception?> LogWarningMessage =
        LoggerMessage.Define<string>(LogLevel.Warning, new EventId(0, nameof(LogWarningMessage)), "{Message}");

    private static readonly Action<ILogger, string, Exception?> LogErrorMessage =
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(0, nameof(LogErrorMessage)), "{Message}");

    private static readonly Action<ILogger, string, Exception?> LogCriticalMessage =
        LoggerMessage.Define<string>(LogLevel.Critical, new EventId(0, nameof(LogCriticalMessage)), "{Message}");

    #endregion

    public static void LogInformation(
        this ILogger logger, Dictionary<string, object>? enrichedProperties, string? message, params object?[] args
    )
    {
        var eventEnrichers = enrichedProperties?
            .Select(p => new PropertyEnricher(p.Key, p.Value))
            .ToArray<ILogEventEnricher>() ?? [];
        using (LogContext.Push(eventEnrichers))
        {
            LogInformationMessage(logger, message ?? string.Empty, null);
        }
    }

    public static void LogWarning(
        this ILogger logger, Dictionary<string, object>? enrichedProperties, string? message, params object?[] args
    )
    {
        var eventEnrichers = enrichedProperties?
            .Select(p => new PropertyEnricher(p.Key, p.Value))
            .ToArray<ILogEventEnricher>() ?? [];
        using (LogContext.Push(eventEnrichers))
        {
            LogWarningMessage(logger, message ?? string.Empty, null);
        }
    }

    public static void LogError(
        this ILogger logger, Dictionary<string, object>? enrichedProperties, Exception? exception, string? message,
        params object?[] args
    )
    {
        ILogEventEnricher[] exceptionEnrichers =
        [
            new PropertyEnricher("Exception", exception?.Message),
            new PropertyEnricher("StackTrace", exception?.StackTrace)
        ];
        var eventEnrichers = enrichedProperties?
            .Select(p => new PropertyEnricher(p.Key, p.Value))
            .Concat(exceptionEnrichers)
            .ToArray() ?? [];
        using (LogContext.Push(eventEnrichers))
        {
            LogErrorMessage(logger, message ?? string.Empty, exception);
        }
    }

    public static void LogError(
        this ILogger logger, Dictionary<string, object>? enrichedProperties, string? message, params object?[] args
    )
    {
        var eventEnrichers = enrichedProperties?
            .Select(p => new PropertyEnricher(p.Key, p.Value))
            .ToArray<ILogEventEnricher>() ?? [];
        using (LogContext.Push(eventEnrichers))
        {
            LogErrorMessage(logger, message ?? string.Empty, null);
        }
    }

    public static void LogCritical(
        this ILogger logger, Dictionary<string, object>? enrichedProperties, Exception? exception, string? message,
        params object?[] args
    )
    {
        ILogEventEnricher[] exceptionEnrichers =
        [
            new PropertyEnricher("Exception", exception?.Message),
            new PropertyEnricher("StackTrace", exception?.StackTrace)
        ];
        var eventEnrichers = enrichedProperties?
            .Select(p => new PropertyEnricher(p.Key, p.Value))
            .Concat(exceptionEnrichers)
            .ToArray() ?? [];
        using (LogContext.Push(eventEnrichers))
        {
            LogCriticalMessage(logger, message ?? string.Empty, exception);
        }
    }

    public static void LogCritical(
        this ILogger logger, Dictionary<string, object>? enrichedProperties, string? message, params object?[] args
    )
    {
        var eventEnrichers = enrichedProperties?
            .Select(p => new PropertyEnricher(p.Key, p.Value))
            .ToArray<ILogEventEnricher>() ?? [];
        using (LogContext.Push(eventEnrichers))
        {
            LogCriticalMessage(logger, message ?? string.Empty, null);
        }
    }
}