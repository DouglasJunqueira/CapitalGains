using Application.UseCases.GetProfitTaxes;
using System.Diagnostics.CodeAnalysis;

namespace Application;

[ExcludeFromCodeCoverage]
public static class Program
{
    public static void Main(string[] args)
    {
        var _useCase = new ProfitTaxesUseCase();
        _useCase.CalculateCapitalGainTax();
    }    
}
