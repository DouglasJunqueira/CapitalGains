namespace Application.UseCases.GetProfitTaxes;

public interface IProfitTaxesUseCase
{
    void CalculateCapitalGainTax();
    IEnumerable<ProfitTaxesOutput> CalculateTaxOverOperations(ProfitTaxesInput input);
}
