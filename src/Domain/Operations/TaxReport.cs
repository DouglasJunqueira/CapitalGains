namespace Domain.Operations;

public class TaxReport
{
    public decimal AveragePrice { get; set; } = 0;
    public decimal AvailableQuantity { get; set; } = 0;
    public decimal TotalLoss { get; set; } = 0;
    public List<SingleOperationTax> OperationsTax { get; set; } = [];
}

public record SingleOperationTax(decimal Tax, string Rule);