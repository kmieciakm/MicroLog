global using System;
global using System.Linq;
global using System.Collections.Generic;
global using System.Threading.Tasks;

global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Http.Extensions;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;

global using MicroLog.Core.Enrichers;
global using MicroLog.Collector.Client;
global using MicroLog.Provider.AspNetCore;