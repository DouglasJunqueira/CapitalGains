using Domain.Operations;
using FluentValidation.Results;
using System.Text.Json;

namespace Application.UseCases.GetProfitTaxes;

internal class ProfitTaxesUseCase() : IProfitTaxesUseCase
{
    private readonly ProfitTaxesValidator _validationRules = new();
    private readonly TaxCalculationService _service = new();

    public void CalculateCapitalGainTax()
    {
        var input = new List<ProfitTaxesInput>();
        string userInput;

        do
        {
            userInput = Console.ReadLine()!;
            if (!string.IsNullOrWhiteSpace(userInput))
            {
                var validationResult = ValidateUserInput(userInput, input, _validationRules);
                if (!validationResult.IsValid)
                    Console.WriteLine(string.Join(", ", validationResult.Errors.Select(_ => _.ErrorMessage)));
            }
        } while (!string.IsNullOrWhiteSpace(userInput));

        input.ForEach(item =>
        {
            var result = CalculateTaxOverOperations(item);
            Console.WriteLine(JsonSerializer.Serialize(result, Program.Serializer));
        });

        Console.ReadLine();
    }

    public IEnumerable<ProfitTaxesOutput> CalculateTaxOverOperations(ProfitTaxesInput input)
    {
        var operationsResult = _service.CalculateTaxForCapitalGain(input.ToDomain());
        return ProfitTaxesOutput.ToOutput(operationsResult);
    }

    private static ValidationResult ValidateUserInput(string userInput, List<ProfitTaxesInput> input, ProfitTaxesValidator validator)
    {
        try
        {
            var item = new ProfitTaxesInput { Items = JsonSerializer.Deserialize<IEnumerable<ProfitTaxesItem>>(userInput, Program.Serializer)! };
            var validationResult = validator.Validate(item);
            if (validationResult.IsValid) input.Add(item);

            return validationResult;
        }
        catch
        {
            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("input", "Objeto de entrada inválido"));
            return validationResult;
        }
    }
}
