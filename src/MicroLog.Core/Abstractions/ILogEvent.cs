using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core.Abstractions
{
    /// <summary>
    /// A log event record.
    /// </summary>
    public interface ILogEvent
    {
        /// <summary>
        /// Identification of event log.
        /// </summary>
        ILogEventIdentity Identity { get; }
        /// <summary>
        /// The message describing the event.
        /// </summary>
        string Message { get; set; }
        /// <summary>
        /// The date and time at which the event occurred.
        /// </summary>
        DateTime Timestamp { get; init; }
        /// <summary>
        /// The level of importance of the event.
        /// </summary>
        LogLevel Level { get; set; }
        /// <summary>
        /// An exception associated with the event, or null.
        /// </summary>
        Exception Exception { get; set; }
        /// <summary>
        /// All additional information assosisaated with the event.
        /// </summary>
        IReadOnlyCollection<ILogProperty> Properties { get; }
    }
}
