using Application.UseCases.GetProfitTaxes;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Application;

public static class Program
{
    public static JsonSerializerOptions Serializer = new()
    {
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),   
    };

    public static void Main(string[] args)
    {
        Serializer.Converters.Add(new JsonStringEnumConverter());
        var _useCase = new ProfitTaxesUseCase();
        _useCase.CalculateCapitalGainTax();
    }    
}
