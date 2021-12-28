global using System;
global using System.Collections.Generic;
global using System.Diagnostics;
global using System.Linq;
global using System.Threading.Tasks;

global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.AspNetCore.ResponseCompression;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;

global using HotChocolate;
global using HotChocolate.Data;
global using HotChocolate.Types;
global using HotChocolate.Types.Pagination;

global using MongoDB.Bson;
global using MongoDB.Bson.Serialization.Attributes;
global using MongoDB.Bson.Serialization.Serializers;
global using MongoDB.Driver;
global using MongoDB.Driver.Core.Events;

global using MicroLog.Core;
global using MicroLog.Core.Abstractions;
global using MicroLog.Sink.Hub;
global using MicroLog.Sink.MongoDb;
global using MicroLog.Sink.MongoDb.Config;