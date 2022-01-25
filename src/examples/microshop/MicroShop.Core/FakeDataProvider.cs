using MicroShop.Core.Models;

namespace MicroShop.Core;

public static class FakeDataProvider
{
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
                    Faker.RandomNumber.Next(0, 8),
                    index => Guid.NewGuid())
        };
        return fakeOrder;
    }
}