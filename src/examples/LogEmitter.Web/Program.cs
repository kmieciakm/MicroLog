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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
