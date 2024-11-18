using Domain.Enums;

namespace Domain.Operations;

public class TaxCalculationService : ITaxCalculationService
{
    private readonly decimal TaxPercentual = 0.2M;
    private readonly decimal MinimalOperationValueForTax = 20000.00M;

    public TaxReport CalculateTaxForCapitalGain(IEnumerable<OrderItem> operations)
    {
        var report = new TaxReport();

        foreach (var operation in operations)
            report.OperationsTax.Add(operation.Operation == OperationType.buy ? Buy(operation, report) : Sell(operation, report));

        return report;
    }

    private static SingleOperationTax Buy(OrderItem operation, TaxReport report)
    {
        report.AveragePrice = CalculateNewAveragePrice(operation, report);
        report.AvailableQuantity += operation.Quantity;
        return new(0.00M, "Comprar ações não paga imposto");
    }

    private SingleOperationTax Sell(OrderItem operation, TaxReport report)
    {
        var operationValue = operation.UnitCost * operation.Quantity;
        report.TotalLoss += CalculateLossOverSingleOperation(operation, report.AveragePrice);
        report.AvailableQuantity -= operation.Quantity;

        if (operationValue <= MinimalOperationValueForTax)
            return new(0.00M, $"Valor total menor do que R$ {MinimalOperationValueForTax}");

        if (operation.UnitCost <= report.AveragePrice)
            return new(0.00M, "Operação sem lucro");

        if (operationValue <= Math.Abs(report.TotalLoss))
        {
            report.TotalLoss += (operation.UnitCost - report.AveragePrice) * operation.Quantity;
            return new(0.00M, "Operando com prejuízo");
        }

        var calculatedTax = CalculateTaxOverSingleOperation(operation, report);
        report.TotalLoss = 0;

        return new (calculatedTax, "Operação com lucro");
    }

    private static decimal CalculateNewAveragePrice(OrderItem operation, TaxReport report)
        => (report.AveragePrice * report.AvailableQuantity + operation.UnitCost * operation.Quantity) / (operation.Quantity + report.AvailableQuantity);

    private decimal CalculateTaxOverSingleOperation(OrderItem operation, TaxReport report)
        => Math.Round(operation.UnitCost <= report.AveragePrice ? 0.00M : ((operation.UnitCost - report.AveragePrice) * operation.Quantity + report.TotalLoss) * TaxPercentual, 2);

    private static decimal CalculateLossOverSingleOperation(OrderItem operation, decimal averagePrice)
        => operation.UnitCost >= averagePrice ? 0.00M : (operation.UnitCost - averagePrice) * operation.Quantity;
}
