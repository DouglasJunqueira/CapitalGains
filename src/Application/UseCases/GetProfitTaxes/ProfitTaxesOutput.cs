using Domain.Operations;
using System.Text.Json.Serialization;

namespace Application.UseCases.GetProfitTaxes;

public class ProfitTaxesOutput(decimal tax)
{
    [JsonPropertyName("tax")]
    public decimal Tax { get; set; } = tax;

    public static IEnumerable<ProfitTaxesOutput> ToOutput(TaxReport report)
        => report.OperationsTax.Select(_ => new ProfitTaxesOutput(_.Tax));
}
