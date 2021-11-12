using System;
using System.Text.Json.Serialization;

namespace MicroLog.Core
{
    public class LogException
    {
        public string Message { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public LogException InnerException { get; set; }

        [JsonConstructor]
        public LogException(string message, string type, string source, LogException innerException)
        {
            Message = message;
            Type = type;
            Source = source;
            InnerException = innerException;
        }

        public LogException(Exception exception)
        {
            Message = exception.Message;
            Type = exception.GetType().Name;
            Source = exception.Source;
            if (exception.InnerException is not null)
            {
                InnerException = new LogException(exception.InnerException);
            }
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
}
