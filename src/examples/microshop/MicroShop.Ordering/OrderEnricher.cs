using MicroLog.Core;
using MicroLog.Core.Abstractions;
using MicroShop.Core.Models;
using System.Text.Json;

namespace MicroShop.Ordering;

public class OrderEnricher : ILogEnricher
{
    private readonly string PROPERTY_NAME = "Order";
    public Order? Order { get; set; }

    public void Enrich(LogEvent log)
    {
        if (Order is not null)
        {
            LogProperty property = new()
            {
                Name = PROPERTY_NAME,
                Value = JsonSerializer.Serialize(Order)
            };
            log.AddProperty(property);
        }
    }
}
