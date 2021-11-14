using IMicroLogger = MicroLog.Core.Abstractions.ILogger;

namespace LogEmitter.Web;

// TODO: Move to more suitable library.
public class HttpContextEnricherMiddleware
{
    private RequestDelegate _next { get; set; }
    private HttpContextAccessor _httpContextAccessor { get; set; } = new();

    public HttpContextEnricherMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IMicroLogger logger)
    {
        _httpContextAccessor.HttpContext = context;

        await _next.Invoke(context);

        var enricher = new HttpContextEnricher(_httpContextAccessor);
        var message = $"Request HTTP {context.Request.Method} {context.Request.GetEncodedUrl()}";
        await logger.LogInformationAsync(message, enricher);
    }
}
