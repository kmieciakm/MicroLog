using MicroLog.Core.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MicroLog.Collector.Middleware
{
    public class CollectorRequestBroker
    {
        private RequestDelegate _next { get; set; }

        public CollectorRequestBroker(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;
            if (path.HasValue && path.Value.ToLower().StartsWith($"/api/collector"))
            {
                var services = context.RequestServices;
                var publisher = services.GetService<ILogPublisher>();
                if (publisher is null)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotImplemented;
                    await context.Response.WriteAsync("Publishing is disabled in this service.");
                    return;
                }
            }

            await _next.Invoke(context);
        }
    }
}
