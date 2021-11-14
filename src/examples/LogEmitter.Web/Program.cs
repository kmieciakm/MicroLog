using IMicroLogger = MicroLog.Core.Abstractions.ILogger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .Configure<LogCollectorConfig>(builder.Configuration.GetSection("LogCollectorConfig"))
    .AddSingleton<IMicroLogger, LogCollectorClient>()
    .AddLogging(builder => builder
        .ClearProviders()
        .AddAspMicroLogger());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// TODO: Craete extension method.
app.UseMiddleware<LogEmitter.Web.HttpContextEnricherMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
