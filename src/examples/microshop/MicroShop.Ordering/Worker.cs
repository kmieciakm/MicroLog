using Microsoft.Extensions.Options;

namespace MicroShop.Ordering;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ProcessingConfig _config;

    public Worker(ILogger<Worker> logger, IOptions<ProcessingConfig> processingOptions)
    {
        _logger = logger;
        _config = processingOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested && _config.Enabled)
        {
            _logger.LogInformation($"ORDERING - Worker running at: {DateTime.Now}");
            await Task.Delay(1000, stoppingToken);
        }
    }
}