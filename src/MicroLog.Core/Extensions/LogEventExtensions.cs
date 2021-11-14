using MicroLog.Core.Abstractions;

namespace MicroLog.Core.Extensions;

public static class LogEventExtensions
{
    /// <summary>
    /// Validates given <see cref="LogEvent"/> record.
    /// </summary>
    /// <param name="logEvent">Log event to validate.</param>
    /// <param name="error">If any error occurs supplies description of an error, otherwise empty string.</param>
    /// <returns>True if log event is valid, otherwise false.</returns>
    public static bool IsValid(this ILogEvent logEvent, out string error)
    {
        if (logEvent.Properties.Any(property => !IsValidJson(property.Value)))
        {
            error = "LogEvent invalid - not all properties values are in JSON format.";
            return false;
        }

        error = string.Empty;
        return true;
    }

    private static bool IsValidJson(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        value = value.Trim();

        if (value.StartsWith("{") && value.EndsWith("}") ||
            value.StartsWith("[") && value.EndsWith("]"))
        {
            try
            {
                var obj = JsonDocument.Parse(value);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        return false;
    }
}
