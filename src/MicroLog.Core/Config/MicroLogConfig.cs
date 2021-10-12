using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core.Config
{
    public class MicroLogConfig
    {
        public string ServiceName { get; set; }
        public LogLevel MinimumLevel { get; set; }
    }
}
