﻿using MicroLog.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core
{
    public record LogProperty : ILogProperty
    {
        public string Name { get; init; }
        public string Value { get; init; }
    }
}