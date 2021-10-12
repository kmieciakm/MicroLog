using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core
{
    public interface ILogEventIdentity
    {
        string EventId { get; init; }
        string ServiceId { get; init; }
    }
}
