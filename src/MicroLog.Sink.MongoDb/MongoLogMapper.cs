using MicroLog.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Sink.MongoDb
{
    internal static class MongoLogMapper
    {
        public static MongoLogEntity Map(ILogEvent log) => new MongoLogEntity(
                identity: new MongoLogIdentity(log.Identity.EventId),
                message: log.Message,
                timestamp: log.Timestamp,
                level: log.Level,
                exception: log.Exception,
                properties: log.Properties.Select(prop => new MongoLogProperty(prop))
            );
    }
}
