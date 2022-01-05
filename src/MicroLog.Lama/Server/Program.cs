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
    services.AddLogStatsProvider(builder.Configuration);

    services.AddSingleton(sp =>
    {
        const string COLLECTION_NAME = "logs";
        var registryConfig = sp
            .GetRequiredService<IOptions<MongoConfig>>()
            .Value;
        var mongoConnectionUrl = new MongoUrl(registryConfig.ConnectionString);
        var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);
        mongoClientSettings.ClusterConfigurator = cb =>
        {
            // This will print the executed command to the console
            cb.Subscribe<CommandStartedEvent>(e =>
            {
                Console.WriteLine($"{e.CommandName} - {e.Command.ToJson()}");
            });
        };
        var client = new MongoClient(mongoClientSettings);
        var database = client.GetDatabase(registryConfig.DatabaseName);
        return database.GetCollection<Log>(COLLECTION_NAME);
    });

    services
        .AddGraphQLServer()
        .AddQueryType<Query>()
        .AddMongoDbFiltering()
        .AddMongoDbSorting()
        .AddMongoDbProjections()
        .AddMongoDbPagingProviders();
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

    app.MapControllers();

    app.MapFallbackToFile("index.html");
}