using MircoLog.Lama.Server;
using MircoLog.Lama.Server.GraphQL;

var builder = WebApplication.CreateBuilder(args);

ConfigureOptions(builder.Services);
ConfigureServices(builder.Services);

var app = builder.Build();

ConfigureApplication(app);
ConfigureEndpoints(app);

app.Run();

void ConfigureOptions(IServiceCollection services)
{
    services.Configure<HubConfig>(builder.Configuration.GetSection("HubConfig"));
}

void ConfigureServices(IServiceCollection services)
{
    services.AddControllersWithViews();
    services.AddRazorPages();

    services.AddSignalR();
    services.AddResponseCompression(opts =>
    {
        opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
            new[] { "application/octet-stream" });
    });

    services.AddLogRegistry(builder.Configuration);

    services
        .AddGraphQLServer()
        .AddQueryType<Query>();
}

void ConfigureApplication(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseBlazorFrameworkFiles();
    app.UseStaticFiles();
    app.UseRouting();
}

void ConfigureEndpoints(WebApplication app)
{
    HubConfig config = app
        .Services
        .GetRequiredService<IOptions<HubConfig>>()
        .Value;

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapHub<LogHub>($"/{config.Name}");
        endpoints.MapGraphQL("/api");
    });

    app.MapFallbackToFile("index.html");
}