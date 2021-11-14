using MicroLog.Core.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace MicroLog.Core.Enrichers;

/// <summary>
/// Enrich log event with HTTP-specific information.
/// </summary>
public class HttpContextEnricher : ILogEnricher
{
    private const string PROPERTY_NAME = "HttpContext";

    private IHttpContextAccessor _HttpContextAccessor { get; set; }

    public HttpContextEnricher(IHttpContextAccessor httpAccessor)
    {
        _HttpContextAccessor = httpAccessor;
    }

    /// <summary>
    /// Adds information about an individual HTTP request / response to log event.
    /// </summary>
    /// <param name="log"></param>
    public void Enrich(LogEvent log)
    {
        if (_HttpContextAccessor is not null)
        {
            var httpProperty = BuildProperty(_HttpContextAccessor.HttpContext);
            if (httpProperty is not null)
            {
                LogProperty property = new()
                {
                    Name = PROPERTY_NAME,
                    Value = JsonSerializer.Serialize(httpProperty)
                };
                log.AddProperty(property);
            }
        }
    }

    private HttpContextProperty BuildProperty(HttpContext httpContext)
    {
        if (httpContext is null) return null;
        return new HttpContextProperty()
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
