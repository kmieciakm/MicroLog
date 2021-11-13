using System.Text.Json.Serialization;

namespace MicroLog.Core;

/// <summary>
/// Base exception model.
/// </summary>
public class LogException
{
    /// <summary>
    /// A message that describes the current exception.
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// The type of the exception instance.
    /// </summary>
    public string Type { get; set; }
    /// <summary>
    /// The name of the application or the object that causes the error.
    /// </summary>
    public string Source { get; set; }
    /// <summary>
    /// An exception that caused the current exception.
    /// </summary>
    public LogException InnerException { get; set; }

    [JsonConstructor]
    public LogException(string message, string type, string source, LogException innerException)
    {
        Message = message;
        Type = type;
        Source = source;
        InnerException = innerException;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogException"/> based on the specified error.
    /// </summary>
    /// <param name="exception">Exception prototype.</param>
    /// <returns><see cref="LogException"/> instance.</returns>
    public static LogException Parse(Exception exception)
    {
        if (exception is null) return null;

        return new LogException(
            exception.Message,
            exception.GetType().Name,
            exception.Source,
            Parse(exception.InnerException));
    }

    public override bool Equals(object obj)
    {
        return obj is LogException exception &&
               Message == exception.Message &&
               Type == exception.Type &&
               Source == exception.Source &&
               InnerException is not null ? InnerException.Equals(exception.InnerException) : true;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Message, Type, Source, InnerException);
    }
}
