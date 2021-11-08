using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core.Abstractions
{
    /// <summary>
    /// Sends log events to the Message Queue.
    /// </summary>
    public interface ILogPublisher
    {
        /// <summary>
        /// Configuration of the publisher.
        /// </summary>
        IPublisherConfig Config { get; }
        /// <summary>
        /// Publishes given log to queue.
        /// </summary>
        /// <param name="logEvent">Log event to send.</param>
        /// <returns>An operation task.</returns>
        Task PublishAsync(ILogEvent logEvent);
        /// <summary>
        /// Sends each log from batch separately to queue.
        /// </summary>
        /// <param name="logEvents">Log events to send.</param>
        /// <returns>An operation task.</returns>
        Task PublishAsync(IEnumerable<ILogEvent> logEvents);
    }
}
