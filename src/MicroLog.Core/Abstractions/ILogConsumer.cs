﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core.Abstractions
{
    /// <summary>
    /// Process messages from queue.
    /// Consumes log entries and saves them in the permanent data storage.
    /// </summary>
    public interface ILogConsumer
    {
        void Consume();
    }
}