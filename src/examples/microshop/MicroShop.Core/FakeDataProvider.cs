using MicroShop.Core.Models;

namespace MicroShop.Core;

public static class FakeDataProvider
{
    public static ShippingAddress GenerateFakeShippingAddress()
    {
        ShippingAddress fakeShippingAddress = new()
        {
            Name = Faker.Name.FullName(),
            Email = Faker.Internet.Email(),
            Country = Faker.Address.Country(),
            City = Faker.Address.City(),
            Street = Faker.Address.StreetAddress(),
        };
        return fakeShippingAddress;
    }

    public static ShippingRequest GenerateFakeShippingRequest()
    {
        ShippingRequest fakeShippingRequest = new()
        {
            ShippingId = Guid.NewGuid(),
            OrderId = Guid.NewGuid(),
            OrderDate = DateTime.Now,
            PredictedDeliveryDate = DateTime.Now.AddDays(4),
            PackagesIds = Faker.Extensions
                .EnumerableExtensions
                .Times(
                    Faker.RandomNumber.Next(1, 4),
                    index => Guid.NewGuid()),
            Sender = GenerateFakeShippingAddress(),
            Buyer = GenerateFakeShippingAddress()
        };
        return fakeShippingRequest;
    }

    public static Order GenerateFakeOrder()
    {
        var totalNetPrice = Faker.RandomNumber.Next(10, 300);
        var totalPrice = totalNetPrice * (1 + Faker.RandomNumber.Next(3, 21) / 100);

        Order fakeOrder = new()
        {
            OrderId = Guid.NewGuid(),
            BuyerId = Guid.NewGuid(),
            BuyerName = Faker.Name.FullName(),
            BuyerEmail = Faker.Internet.Email(),
            OrderedAt = DateTime.Now,
            TotalNetPrice = totalNetPrice,
            TotalPrice = totalPrice,
            CurrencyCode = Faker.Currency.ThreeLetterCode(),
            ProductsIds = Faker.Extensions
                .EnumerableExtensions
                .Times(
                    Faker.RandomNumber.Next(1, 4),
                    index => Guid.NewGuid())
        };
        return fakeOrder;
    }
}