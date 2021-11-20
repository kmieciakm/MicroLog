using MicroLog.Core.Enrichers;
using Microsoft.AspNetCore.Builder;

namespace MicroLog.Core.Extensions;

public static class EnricherExtensions
{
    public static void UseHttpContextEnricher(this IApplicationBuilder app)
    {
        app.UseMiddleware<HttpContextEnricherMiddleware>();
    }
}
