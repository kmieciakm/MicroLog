using MicroShop.Core;
using MicroShop.Core.Models;

namespace MicroShop.Shipping;

public interface IShippingProcessingService
{
    void ProcessShippingRequest(ShippingRequest shipping);
}

internal class FakeShippingProcessingService : IShippingProcessingService
{
    private readonly ILogger<FakeShippingProcessingService> _logger;

    private ShippingEnricher _ShippingEnricher { get; }

    public FakeShippingProcessingService(
        ILogger<FakeShippingProcessingService> logger, ShippingEnricher shippingEnricher)
    {
        _logger = logger;
        _ShippingEnricher = shippingEnricher;
    }

    public void ProcessShippingRequest(ShippingRequest shipping)
    {
        _ShippingEnricher.Shipping = shipping;

        _logger.LogInformation($"Started processing ...");

        Random random = new();
        var process = random.RandomizeAction<ShippingRequest>(
            10,
            shipping => ProcessCorrectly(shipping),
            shipping => ProcessIncorrectly(shipping));
        process.Invoke(shipping);

        _logger.LogInformation($"Ended processing.");
    }

    private void ProcessCorrectly(ShippingRequest shipping)
    {
        _logger.LogInformation(
            $"Checking packages ...");
        _logger.LogInformation(
            $"All packages are ready.");
        _logger.LogInformation(
            $"Scheduling transport ...");
        _logger.LogDebug(
            $"Transport from: {shipping.Sender.Location} to {shipping.Buyer.Location}.");
        _logger.LogInformation(
            $"Sending transport confirmation to {shipping.Buyer.Name} ({shipping.Buyer.Email}).");
    }

    private void ProcessIncorrectly(ShippingRequest shipping)
    {
        Random random = new();
        var process = random.RandomizeAction<ShippingRequest>(
            25,
            shipping => PackageNotPrepared(),
            shipping => UnpaidOrder());
        process.Invoke(shipping);

        void PackageNotPrepared()
        {
            _logger.LogInformation(
                $"Checking payment status ...");
            _logger.LogInformation(
                $"Order has been paid in time.");
            _logger.LogInformation(
                $"Checking packages ...");
            _logger.LogWarning(
                $"Packages are not prepared yet.");
            string secondPackageId = shipping.PackagesIds.ElementAtOrDefault(1).ToString() ?? "";
            _logger.LogDebug(
                $"Not ready packages ids: {shipping.PackagesIds.First()} {secondPackageId}.");
            _logger.LogInformation(
                $"Rescheduling delivery ...");
            _logger.LogInformation(
                $"Sending transport delay email to {shipping.Buyer.Name} ({shipping.Buyer.Email}).");
        }

        void UnpaidOrder()
        {
            _logger.LogInformation(
                $"Checking payment status ...");
            _logger.LogError(
                $"Order {shipping.OrderId} has not been paid in time.");
        }
    }
}
