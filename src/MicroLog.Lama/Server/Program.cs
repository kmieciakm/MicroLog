var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.Configure<HubConfig>(builder.Configuration.GetSection("HubConfig"));

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

// App configuraton
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    HubConfig config = app.Services.GetRequiredService<IOptions<HubConfig>>().Value;
    endpoints.MapHub<LogHub>($"/{config.Name}");
});

app.MapFallbackToFile("index.html");
app.Run();
