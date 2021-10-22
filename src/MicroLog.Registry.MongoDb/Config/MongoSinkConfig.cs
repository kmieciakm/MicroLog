using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Sink.MongoDb.Config
{
    public class MongoSinkConfig
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
