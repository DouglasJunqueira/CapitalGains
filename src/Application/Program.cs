using Application.UseCases.GetProfitTaxes;
using Domain.Operations;
using System.Diagnostics.CodeAnalysis;

namespace Application;

[ExcludeFromCodeCoverage]
public static class Program
{
    public static void Main(string[] args)
    {
        var useCase = new ProfitTaxesUseCase(new TaxCalculationService(), new());
        useCase.CalculateCapitalGainTax();
    }    
}
