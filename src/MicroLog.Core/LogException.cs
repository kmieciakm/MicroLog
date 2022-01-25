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
    /// A message that describes the immediate frames of the call stack.
    /// </summary>
    public string StackTrace { get; set; }
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

    /// <summary>
    /// Initializes a new instance of the <see cref="LogException"/> based on the specified error.
    /// </summary>
    /// <param name="exception">Exception prototype.</param>
    /// <returns><see cref="LogException"/> instance.</returns>
    public static LogException Parse(Exception exception)
    {
        if (exception is null) return null;

        return new LogException()
        {
            Message = exception.Message,
            StackTrace = exception.StackTrace,
            Type = exception.GetType().Name,
            Source = exception.Source,
            InnerException = Parse(exception.InnerException)
        };
    }

    public override bool Equals(object obj)
    {
        return obj is LogException exception &&
               Message == exception.Message &&
               StackTrace == exception.StackTrace &&
               Type == exception.Type &&
               Source == exception.Source &&
               InnerException is not null ? InnerException.Equals(exception.InnerException) : true;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Message, StackTrace, Type, Source, InnerException);
    }
}
