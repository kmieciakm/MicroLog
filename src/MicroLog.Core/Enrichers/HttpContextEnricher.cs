using MicroLog.Core.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MicroLog.Core.Enrichers
{
    public class HttpContextEnricher : ILogEnricher
    {
        private const string PROPERTY_NAME = "HttpContext";

        private HttpContextProperty _HttpContextProperty { get; set; }

        public HttpContextEnricher(IHttpContextAccessor httpAccessor)
            : this(httpAccessor.HttpContext)
        {
        }

        private HttpContextEnricher(HttpContext httpContext)
        {
            if (httpContext is null) return;

            _HttpContextProperty = new()
            {
                Request = new()
                {
                    Url = httpContext.Request.GetEncodedUrl(),
                    Method = httpContext.Request.Method
                },
                Response = new()
                {
                    StatusCode = httpContext.Response.StatusCode.ToString()
                }
            };
        }

        public void Enrich(LogEvent log)
        {
            if (_HttpContextProperty is not null)
            {
                LogProperty property = new()
                {
                    Name = PROPERTY_NAME,
                    Value = JsonSerializer.Serialize(_HttpContextProperty)
                };
                log.Enrich(property);
            }
        }

        private class HttpContextProperty
        {
            public HttpRequestProperty Request { get; set; }
            public HttpResponseProperty Response { get; set; }
        }

        private class HttpRequestProperty
        {
            public string Url { get; set; }
            public string Method { get; set; }
        }

        private class HttpResponseProperty
        {
            public string StatusCode { get; set; }
        }
    }
}
