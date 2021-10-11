using MicroLog.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core
{
    public class MicroLogger : ILogger
    {
        private ILogSink _LogSink { get; set; }
        private MicroLogConfig _Config { get; set; }

        public MicroLogger(ILogSink logSink)
        {
            _LogSink = logSink;
        }

        public MicroLogger(ILogSink logSink, MicroLogConfig config) : this(logSink)
        {
            _Config = config;
        }

        public void LogCritical()
        {
            throw new NotImplementedException();
        }

        public void LogDebug()
        {
            throw new NotImplementedException();
        }

        public void LogError()
        {
            throw new NotImplementedException();
        }

        public void LogInformation()
        {
            throw new NotImplementedException();
        }

        public void LogTrace()
        {
            throw new NotImplementedException();
        }

        public void LogWarning()
        {
            throw new NotImplementedException();
        }
    }
}
