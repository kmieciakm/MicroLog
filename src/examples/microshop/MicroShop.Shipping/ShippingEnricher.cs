using MicroLog.Core;
using MicroLog.Core.Abstractions;
using MicroShop.Core.Models;
using System.Text.Json;

namespace MicroShop.Shipping;

public class ShippingEnricher : ILogEnricher
{
    private readonly string PROPERTY_NAME = "Shipping";
    public ShippingRequest? Shipping { get; set; }

    public void Enrich(LogEvent log)
    {
        if (Shipping is not null)
        {
            LogProperty property = new()
            {
                Name = PROPERTY_NAME,
                Value = JsonSerializer.Serialize(Shipping)
            };
            log.AddProperty(property);
        }
    }
}
