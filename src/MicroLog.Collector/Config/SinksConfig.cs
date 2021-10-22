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
}
