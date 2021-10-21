using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Driver.MongoDb.Config
{
    public class MongoDbLogConfig
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
