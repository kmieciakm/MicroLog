using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core.Abstractions
{
    /// <summary>
    /// Unique identification of event log.
    /// </summary>
    public interface ILogEventIdentity
    {
        /// <summary>
        /// Id of event log.
        /// </summary>
        string EventId { get; init; }
    }
}
