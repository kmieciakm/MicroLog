using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core
{
    public interface ILogger
    {
        void LogTrace();
        void LogDebug();
        void LogInformation();
        void LogWarning();
        void LogError();
        void LogCritical();
    }
}
