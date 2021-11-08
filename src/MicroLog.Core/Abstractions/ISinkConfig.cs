using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core.Abstractions
{
    /// <summary>
    /// Configuration of ILogSink.
    /// </summary>
    public interface ISinkConfig
    {
        /// <summary>
        /// Unique name, identification of the sink.
        /// </summary>
        public string Name { get; set; }
    }
}
