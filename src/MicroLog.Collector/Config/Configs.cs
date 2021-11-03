using MicroLog.Collector.RabbitMq.Config;
using MicroLog.Sink.MongoDb.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroLog.Collector.Config
{
    public class SinksConfig
    {
        public IEnumerable<MongoSinkConfig> Mongo { get; set; }
    }

    public class PublishersConfig
    {
        public RabbitPublisherConfig RabbitMq { get; set; }
    }
}
