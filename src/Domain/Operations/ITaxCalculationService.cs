namespace Domain.Operations;

public interface ITaxCalculationService
{
    TaxReport CalculateTaxForCapitalGain(IEnumerable<OrderItem> operations);
}
