using MicroShop.Core;
using MicroShop.Core.Models;

namespace MicroShop.Ordering;

public interface IOrderProcessingService
{
    void ProcessOrder(Order order);
}

public class FakeOrderProcessingService : IOrderProcessingService
{
    private readonly ILogger<FakeOrderProcessingService> _logger;

    private OrderEnricher _OrderEnricher { get; }

    public FakeOrderProcessingService(ILogger<FakeOrderProcessingService> logger, OrderEnricher orderEnricher)
    {
        _logger = logger;
        _OrderEnricher = orderEnricher;
    }

    public void ProcessOrder(Order order)
    {
        _OrderEnricher.Order = order;

        _logger.LogInformation($"Started processing ..");

        Random random = new();
        var process = random.RandomizeAction<Order>(
            5,
            order => ProcessCorrectly(order),
            order => ProcessIncorrectly(order));

        process.Invoke(order);

        _logger.LogInformation($"Ended processing");
    }

    private void ProcessCorrectly(Order order)
    {
        _logger.LogInformation(
            $"Checking products availability ..");
        _logger.LogInformation(
            $"All ordered products are accessible ..");
        _logger.LogInformation(
            $"Booking products ..");
        _logger.LogInformation(
            $"Products booked");
        _logger.LogDebug(
            $"Booked products ids: {string.Join("-", order.ProductsIds)}");
        _logger.LogInformation(
            $"Accessing buyer location ..");
        _logger.LogInformation(
            $"Creating shipping request ..");
        _logger.LogInformation(
            $"Sending order confiration email to {order.BuyerName} ({order.BuyerEmail})");
    }

    private void ProcessIncorrectly(Order order)
    {
        Random random = new();
        var process = random.RandomizeAction<Order>(
            50,
            order => UnavailableProduct(),
            order => BuyerBlocked());
        process.Invoke(order);

        void UnavailableProduct()
        {
            _logger.LogInformation(
                $"Checking products availability ..");
            _logger.LogWarning(
                $"Product of id: {order.ProductsIds.First()} is temporary unavailable");
            _logger.LogInformation(
                $"Redirecting order to delay queue");
            _logger.LogInformation(
                $"Sending order delay email to {order.BuyerName} ({order.BuyerEmail})");
        }

        void BuyerBlocked()
        {
            _logger.LogInformation(
            $"Checking products availability ..");
            _logger.LogInformation(
                $"All ordered products are accessible ..");
            _logger.LogInformation(
                $"Booking products ..");
            _logger.LogInformation(
                $"Products booked");
            _logger.LogDebug(
                $"Booked products ids: {string.Join("-", order.ProductsIds)}");
            _logger.LogInformation(
                $"Accessing buyer location ..");
            _logger.LogError(
                $"Buyer {order.BuyerName} ({order.BuyerId}) is blocked. Order cannot be delivered");
            _logger.LogDebug(
                $"Blocking reason: exceed credit limit");
        }
    }
}