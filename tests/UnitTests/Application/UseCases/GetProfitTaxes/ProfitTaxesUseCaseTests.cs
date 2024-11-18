using Application.UseCases.GetProfitTaxes;
using Domain.Operations;
using NSubstitute;

namespace UnitTests.Application.UseCases.GetProfitTaxes;

public class ProfitTaxesUseCaseTests
{
    private readonly ITaxCalculationService _service = Substitute.For<ITaxCalculationService>();
    private readonly IProfitTaxesUseCase _useCase;

    public ProfitTaxesUseCaseTests()
    {
        _useCase = new ProfitTaxesUseCase(_service, new());
    }

    [Theory]
    [InlineData("{}","Objeto de entrada inválido")]
    [InlineData("[{\"operation\":\"sell\",\"unit-cost\":10.00,\"quantity\":100}]", "A operação inicial deve ser compra")]
    [InlineData("[{\"operation\":\"buy\",\"unit-cost\":-10.00,\"quantity\":100}]", "Valor unitário deve ser positivo")]
    [InlineData("[{\"operation\":\"buy\",\"unit-cost\":10.00,\"quantity\":0}]", "Quantidade deve ser um número positivo")]
    [InlineData("[{\"operation\":\"buy\",\"unit-cost\":10.00,\"quantity\":50},{\"operation\":\"sell\",\"unit-cost\":10.00,\"quantity\":100}]", "Ordem das operações apresenta um erro levando à quantidades negativas")]
    [InlineData("[{\"operation\":\"buy\",\"unit-cost\":10.00,\"quantity\":100},{\"operation\":\"sell\",\"unit-cost\":15.00,\"quantity\":50},{\"operation\":\"sell\",\"unit-cost\":15.00,\"quantity\":50}]","[{\"tax\":0.00},{\"tax\":0.00},{\"tax\":0.00}]")]

    public void Cases(string input,string result)
    {
        var output = new StringWriter();
        Console.SetOut(output);

        Console.SetIn(new StringReader(input));


        _service.CalculateTaxForCapitalGain(Arg.Any<IEnumerable<OrderItem>>()).Returns(new TaxReport
        {
            AvailableQuantity = 0,
            AveragePrice = 10,
            TotalLoss = 0,
            OperationsTax = [ new(0.00M, ""), new(0.00M, ""), new(0.00M, "")]
        });

        _useCase.CalculateCapitalGainTax();

        output.ToString().Should().Contain(result);
    }
}
