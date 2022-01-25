namespace MicroShop.Core.Models;

public class Order
{
    public Guid OrderId { get; set; }
    public Guid BuyerId { get; set; }
    public string? BuyerName { get; set; }
    public string? BuyerEmail { get; set; }
    public DateTime OrderedAt { get; set; }
    public int TotalNetPrice { get; set; }
    public int TotalPrice { get; set; }
    public string CurrencyCode { get; set; } = "EUR";
    public IEnumerable<Guid> ProductsIds { get; set; } = Enumerable.Empty<Guid>();
}
