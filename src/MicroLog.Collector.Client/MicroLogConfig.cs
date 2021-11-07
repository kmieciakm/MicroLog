using MicroLog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Collector.Client
{
    public class MicroLogConfig
    {
        public string Url { get; set; }
        public LogLevel MinimumLevel { get; set; }
    }
}
