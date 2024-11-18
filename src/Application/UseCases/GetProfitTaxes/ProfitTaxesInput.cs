using Domain.Enums;
using Domain.Operations;
using System.Text.Json.Serialization;

namespace Application.UseCases.GetProfitTaxes;

public class ProfitTaxesInput
{
    public IEnumerable<ProfitTaxesItem> Items { get; set; } = [];

    public IEnumerable<OrderItem> ToDomain()
        => Items.Select(_ => new OrderItem(_.Operation, _.UnitCost, _.Quantity));
}

public class ProfitTaxesItem
{
    [JsonPropertyName("operation")]
    public OperationType Operation { get; set; }
    [JsonPropertyName("unit-cost")]
    public decimal UnitCost { get; set; }
    [JsonPropertyName("quantity")]
    public decimal Quantity { get; set; }
}
