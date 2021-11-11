using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core.Abstractions
{
    /// <summary>
    /// Representation of additional data added to the event log.
    /// </summary>
    public interface ILogProperty
    {
        /// <summary>
        /// Name of the property.
        /// </summary>
        string Name { get; init; }
        /// <summary>
        /// Property value serialized to JSON format.
        /// </summary>
        string Value { get; init; }
    }
}
