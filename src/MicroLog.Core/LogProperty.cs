using MicroLog.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core
{
    public class LogProperty : ILogProperty
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
