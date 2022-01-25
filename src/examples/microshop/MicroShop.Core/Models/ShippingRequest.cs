namespace MicroShop.Core.Models;

public class ShippingRequest
{
    public Guid ShippingId { get; set; }
    public Guid OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime PredictedDeliveryDate { get; set; }
    public IEnumerable<Guid> PackagesIds { get; set; } = Enumerable.Empty<Guid>();
    public ShippingAddress Sender { get; set; }
    public ShippingAddress Buyer { get; set; }
}

public class ShippingAddress
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string Location => $"{Street}, {City}";
}
