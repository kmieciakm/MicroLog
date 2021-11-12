using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core.Abstractions
{
    /// <summary>
    /// Asynchronous logger. 
    /// </summary>
    public interface ILogger
    {
        bool ShouldLog(LogLevel level);
        Task LogAsync(LogLevel level, string message, LogException exception = null);
        Task LogTraceAsync(string message);
        Task LogDebugAsync(string message);
        Task LogInformationAsync(string message);
        Task LogWarningAsync(string message, LogException exception = null);
        Task LogErrorAsync(string message, LogException exception = null);
        Task LogCriticalAsync(string message, LogException exception = null);
    }
}
