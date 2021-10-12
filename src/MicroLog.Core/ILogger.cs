using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core
{
    public interface ILogger
    {
        public bool ShouldLog(LogLevel level);
        Task LogTraceAsync(string message);
        Task LogDebugAsync(string message);
        Task LogInformationAsync(string message);
        Task LogWarningAsync(string message, Exception exception = null);
        Task LogErrorAsync(string message, Exception exception = null);
        Task LogCriticalAsync(string message, Exception exception = null);
    }
}
