using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Collector.Client
{
    class MicroLogRoutes
    {
        private string _Base { get; init; }
        public string Insert
        {
            get
            {
                return $"{_Base}/insert";
            }
        }

        public MicroLogRoutes(string url)
        {
            _Base = $"{url}/api/collector";
        }
    }
}
