using MicroShop.Core;
using MicroShop.Core.Models;
using Microsoft.Extensions.Options;

namespace MicroShop.Ordering;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ProcessingConfig _config;

    private IOrderProcessingService _OrderProcessingService { get; }

    public Worker(ILogger<Worker> logger, IOptions<ProcessingConfig> processingOptions,
        IOrderProcessingService orderProcessingService)
    {
        _logger = logger;
        _config = processingOptions.Value;
        _OrderProcessingService = orderProcessingService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"ORDERING - Worker started at: {DateTime.Now}");
        while (!stoppingToken.IsCancellationRequested && _config.Enabled)
        {
            Order order = FakeDataProvider.GenerateFakeOrder();
            _OrderProcessingService.ProcessOrder(order);
            await Task.Delay(_config.DelayInMilliseconds, stoppingToken);
        }
        _logger.LogInformation($"ORDERING - Worker stoped at: {DateTime.Now}");
    }
}