using Application.UseCases.GetProfitTaxes;
using Domain.Operations;

namespace IntegratedTests;

public class ProfitTaxesTests
{
    private readonly IProfitTaxesUseCase _useCase = new ProfitTaxesUseCase(new TaxCalculationService(), new());

    [Theory]
    [InlineData("{}", "Objeto de entrada inválido")]
    [InlineData("[{\"operation\":\"sell\",\"unit-cost\":10.00,\"quantity\":100}]", "A operação inicial deve ser compra")]
    [InlineData("[{\"operation\":\"buy\",\"unit-cost\":-10.00,\"quantity\":100}]", "Valor unitário deve ser positivo")]
    [InlineData("[{\"operation\":\"buy\",\"unit-cost\":10.00,\"quantity\":0}]", "Quantidade deve ser um número positivo")]
    [InlineData("[{\"operation\":\"buy\",\"unit-cost\":10.00,\"quantity\":50},{\"operation\":\"sell\",\"unit-cost\":10.00,\"quantity\":100}]", "Ordem das operações apresenta um erro levando à quantidades negativas")]
    [InlineData("[{\"operation\":\"buy\",\"unit-cost\":10.00,\"quantity\":100},{\"operation\":\"sell\",\"unit-cost\":15.00,\"quantity\":50},{\"operation\":\"sell\",\"unit-cost\":15.00,\"quantity\":50}]", "[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00}]")]
    [InlineData("[{\"operation\":\"buy\",\"unit-cost\":10.00,\"quantity\":10000},{\"operation\":\"sell\",\"unit-cost\":20.00,\"quantity\":5000},{\"operation\":\"sell\",\"unit-cost\":5.00,\"quantity\":5000}]", "[{\"tax\":0.00},{\"tax\":10000.00},{\"tax\":0.00}]")]
    [InlineData("[{\"operation\":\"buy\",\"unit-cost\":10.00,\"quantity\":10000},{\"operation\":\"sell\",\"unit-cost\":5.00,\"quantity\":5000},{\"operation\":\"sell\",\"unit-cost\":20.00,\"quantity\":3000}]", "[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":1000.00}]")]
    [InlineData("[{\"operation\":\"buy\",\"unit-cost\":10.00,\"quantity\":10000},{\"operation\":\"buy\",\"unit-cost\":25.00,\"quantity\":5000},{\"operation\":\"sell\",\"unit-cost\":15.00,\"quantity\":10000}]", "[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00}]")]
    [InlineData("[{\"operation\":\"buy\",\"unit-cost\":10.00,\"quantity\":10000},{\"operation\":\"buy\",\"unit-cost\":25.00,\"quantity\":5000},{\"operation\":\"sell\",\"unit-cost\":15.00,\"quantity\":10000},{\"operation\":\"sell\",\"unit-cost\":25.00,\"quantity\":5000}]", "[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":10000.00}]")]
    [InlineData("[{\"operation\":\"buy\",\"unit-cost\":10.00,\"quantity\":10000},{\"operation\":\"sell\",\"unit-cost\":2.00,\"quantity\":5000},{\"operation\":\"sell\",\"unit-cost\":20.00,\"quantity\":2000},{\"operation\":\"sell\",\"unit-cost\":20.00,\"quantity\":2000},{\"operation\":\"sell\",\"unit-cost\":25.00,\"quantity\":1000}]", "[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":3000.00}]")]
    [InlineData("[{\"operation\":\"buy\",\"unit-cost\":10.00,\"quantity\":10000},{\"operation\":\"sell\",\"unit-cost\":2.00,\"quantity\":5000},{\"operation\":\"sell\",\"unit-cost\":20.00,\"quantity\":2000},{\"operation\":\"sell\",\"unit-cost\":20.00,\"quantity\":2000},{\"operation\":\"sell\",\"unit-cost\":25.00,\"quantity\":1000},{\"operation\":\"buy\",\"unit-cost\":20.00,\"quantity\":10000},{\"operation\":\"sell\",\"unit-cost\":15.00,\"quantity\":5000},{\"operation\":\"sell\",\"unit-cost\":30.00,\"quantity\":4350},{\"operation\":\"sell\",\"unit-cost\":30.00,\"quantity\":650}]", "[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":3000.00},{\"tax\":0.00},{\"tax\":0.00},{\"tax\":3700.00},{\"tax\":0.00}]")]
    [InlineData("[{\"operation\":\"buy\",\"unit-cost\":10.00,\"quantity\":10000},{\"operation\":\"sell\",\"unit-cost\":50.00,\"quantity\":10000},{\"operation\":\"buy\",\"unit-cost\":20.00,\"quantity\":10000},{\"operation\":\"sell\",\"unit-cost\":50.00,\"quantity\":10000}]", "[{\"tax\":0.00},{\"tax\":80000.00},{\"tax\":0.00},{\"tax\":60000.00}]")]

    public void Cases(string input, string result)
    {
        var output = new StringWriter();
        Console.SetOut(output);

        Console.SetIn(new StringReader(input));

        _useCase.CalculateCapitalGainTax();

        output.ToString().Should().Contain(result);
    }
}
