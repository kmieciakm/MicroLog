using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Collector.RabbitMq.Config
{
    public class RabbitPublisherConfig
    {
        public string Queues { get; set; }

        public IEnumerable<string> GetQueues()
        {
            return Queues.Split(',');
        }
    }
}
