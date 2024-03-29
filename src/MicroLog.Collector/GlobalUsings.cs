﻿global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Net;
global using System.Threading;
global using System.Threading.Tasks;

global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Options;

global using MicroLog.Collector.RabbitMq;
global using MicroLog.Collector.RabbitMq.Config;
global using MicroLog.Core;
global using MicroLog.Core.Abstractions;
global using MicroLog.Sink.Hub;
global using MicroLog.Sink.MongoDb;
global using MicroLog.Sink.MongoDb.Config;