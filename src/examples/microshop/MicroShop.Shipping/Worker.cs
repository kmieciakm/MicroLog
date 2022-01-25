using MicroShop.Core;
using MicroShop.Core.Models;
using Microsoft.Extensions.Options;

namespace MicroShop.Shipping;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ProcessingConfig _config;

    private IShippingProcessingService _ShippingProcessingService { get; }

    public Worker(ILogger<Worker> logger, IOptions<ProcessingConfig> processingOptions,
        IShippingProcessingService shippingService)
    {
        _logger = logger;
        _config = processingOptions.Value;
        _ShippingProcessingService = shippingService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"SHIPPING - Worker started at: {DateTime.Now}");
        while (!stoppingToken.IsCancellationRequested && _config.Enabled)
        {
            ShippingRequest shipping = FakeDataProvider.GenerateFakeShippingRequest();
            _ShippingProcessingService.ProcessShippingRequest(shipping);
            await Task.Delay(_config.DelayInMilliseconds, stoppingToken);
        }
        _logger.LogInformation($"SHIPPING - Worker stoped at: {DateTime.Now}");
    }
}