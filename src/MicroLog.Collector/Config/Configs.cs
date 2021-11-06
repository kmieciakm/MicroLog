using MicroLog.Core.Abstractions;
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

    public class PublisherConfig : IPublisherConfig
    {
        public string Queues { get; set; } = string.Empty;

        public bool IsPublisherSpecified
        {
            get
            {
                return !string.IsNullOrEmpty(Queues);
            }
        }

        public IEnumerable<string> GetQueues()
        {
            return Queues.Split(',');
        }
    }
}
