using Domain.Operations;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace UnitTests.Domain.Operations;

public class TaxCalculationServiceTests
{
    private readonly JsonSerializerOptions Serializer = new()
    {
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
    };

    private readonly TaxCalculationService _service = new();

    public TaxCalculationServiceTests()
    {
        Serializer.Converters.Add(new JsonStringEnumConverter());
    }

    [Fact]
    public void Case01()
    {
        var operations = """
            [   
                {"operation":"buy", "unitCost":10.00, "quantity": 100},
                {"operation":"sell", "unitCost":15.00, "quantity": 50},
                {"operation":"sell", "unitCost":15.00, "quantity": 50}
            ]
            """;

        var results =
            """
            {"AveragePrice":10.00,"AvailableQuantity":0,"TotalLoss":0,"OperationsTax":
                [{"Tax":0,"Rule":"Comprar ações não paga imposto"},
                {"Tax":0,"Rule":"Valor total menor do que R$ 20000,00"},
                {"Tax":0,"Rule":"Valor total menor do que R$ 20000,00"}]
            }
            """;

        Validate(operations, results);
    }

    [Fact]
    public void Case02()
    {
        var operations = """
            [   
                {"operation":"buy", "unitCost":10.00, "quantity": 10000},
                {"operation":"sell", "unitCost":20.00, "quantity": 5000},
                {"operation":"sell", "unitCost":5.00, "quantity": 5000}
            ]
            """;

        var results =
            """
            {"AveragePrice":10.00,"AvailableQuantity":0,"TotalLoss":-25000.00,"OperationsTax":
                [{"Tax":0,"Rule":"Comprar ações não paga imposto"},
                {"Tax":10000.000,"Rule":"Operação com lucro"},
                {"Tax":0,"Rule":"Operação sem lucro"}]
            }
            """;

        Validate(operations, results);
    }

    [Fact]
    public void Case03()
    {
        var operations = """
            [   
                {"operation":"buy", "unitCost":10.00, "quantity": 10000},
                {"operation":"sell", "unitCost":5.00, "quantity": 5000},
                {"operation":"sell", "unitCost":20.00, "quantity": 3000}
            ]
            """;

        var results =
            """
            {"AveragePrice":10.00,"AvailableQuantity":2000,"TotalLoss":0,"OperationsTax":
                [{"Tax":0.00,"Rule":"Comprar ações não paga imposto"},
                {"Tax":0.00,"Rule":"Operação sem lucro"},
                {"Tax":1000.00,"Rule":"Operação com lucro"}]
            }
            """;

        Validate(operations, results);
    }

    [Fact]
    public void Case04()
    {
        var operations = """
            [   
                {"operation":"buy", "unitCost":10.00, "quantity": 10000},
                {"operation":"buy", "unitCost":25.00, "quantity": 5000},
                {"operation":"sell", "unitCost":15.00, "quantity": 10000}
            ]
            """;

        var results =
            """
            {"AveragePrice":15.00,"AvailableQuantity":5000,"TotalLoss":0.00,"OperationsTax":
                [{"Tax":0.00,"Rule":"Comprar ações não paga imposto"},
                {"Tax":0.00,"Rule":"Comprar ações não paga imposto"},
                {"Tax":0.00,"Rule":"Operação sem lucro"}]
            }
            """;

        Validate(operations, results);
    }

    [Fact]
    public void Case05()
    {
        var operations = """
            [   
                {"operation":"buy", "unitCost":10.00, "quantity": 10000},
                {"operation":"buy", "unitCost":25.00, "quantity": 5000},
                {"operation":"sell", "unitCost":15.00, "quantity": 10000},
                {"operation":"sell", "unitCost":25.00, "quantity": 5000}
            ]
            """;

        var results =
            """
            {"AveragePrice":15.00,"AvailableQuantity":0,"TotalLoss":0,"OperationsTax":
                [{"Tax":0.00,"Rule":"Comprar ações não paga imposto"},
                {"Tax":0.00,"Rule":"Comprar ações não paga imposto"},
                {"Tax":0.00,"Rule":"Operação sem lucro"},
                {"Tax":10000.00,"Rule":"Operação com lucro"}]
            }
            """;

        Validate(operations, results);
    }

    [Fact]
    public void Case06()
    {
        var operations = """
            [   
                {"operation":"buy", "unitCost":10.00, "quantity": 10000},
                {"operation":"sell", "unitCost":2.00, "quantity": 5000},
                {"operation":"sell", "unitCost":20.00, "quantity": 2000},
                {"operation":"sell", "unitCost":20.00, "quantity": 2000},
                {"operation":"sell", "unitCost":25.00, "quantity": 1000}
            ]
            """;

        var results =
            """
            {"AveragePrice":10.00,"AvailableQuantity":0,"TotalLoss":0,"OperationsTax":
                [{"Tax":0.00,"Rule":"Comprar ações não paga imposto"},
                {"Tax":0.00,"Rule":"Valor total menor do que R$ 20000,00"},
                {"Tax":0.00,"Rule":"Operando com prejuízo"},
                {"Tax":0.00,"Rule":"Operação com lucro"},
                {"Tax":3000.00,"Rule":"Operação com lucro"}]
            }
            """;

        Validate(operations, results);
    }

    [Fact]
    public void Case07()
    {
        var operations = """
            [{"operation":"buy", "unitCost":10.00, "quantity": 10000},
            {"operation":"sell", "unitCost":2.00, "quantity": 5000},
            {"operation":"sell", "unitCost":20.00, "quantity": 2000},
            {"operation":"sell", "unitCost":20.00, "quantity": 2000},
            {"operation":"sell", "unitCost":25.00, "quantity": 1000},
            {"operation":"buy", "unitCost":20.00, "quantity": 10000},
            {"operation":"sell", "unitCost":15.00, "quantity": 5000},
            {"operation":"sell", "unitCost":30.00, "quantity": 4350},
            {"operation":"sell", "unitCost":30.00, "quantity": 650}]
            """;

        var results =
            """
            {"AveragePrice":20.00,"AvailableQuantity":0,"TotalLoss":0.00,"OperationsTax":
                [{"Tax":0.00,"Rule":"Comprar ações não paga imposto"},
                {"Tax":0.00,"Rule":"Valor total menor do que R$ 20000,00"},
                {"Tax":0.00,"Rule":"Operando com prejuízo"},
                {"Tax":0.00,"Rule":"Operação com lucro"},
                {"Tax":3000.00,"Rule":"Operação com lucro"},
                {"Tax":0.00,"Rule":"Comprar ações não paga imposto"},
                {"Tax":0.00,"Rule":"Operação sem lucro"},
                {"Tax":3700.00,"Rule":"Operação com lucro"},
                {"Tax":0.00,"Rule":"Valor total menor do que R$ 20000,00"}]}
            """;

        Validate(operations, results);
    }

    [Fact]
    public void Case08()
    {
        var operations 
            = """
            [{"operation":"buy", "unitCost":10.00, "quantity": 10000},
            {"operation":"sell", "unitCost":50.00, "quantity": 10000},
            {"operation":"buy", "unitCost":20.00, "quantity": 10000},
            {"operation":"sell", "unitCost":50.00, "quantity": 10000}]
            """;

        var results =
            """
            {"AveragePrice":20.00,"AvailableQuantity":0,"TotalLoss":0,"OperationsTax":
                [{"Tax":0.00,"Rule":"Comprar ações não paga imposto"},
                {"Tax":80000.00,"Rule":"Operação com lucro"},
                {"Tax":0.00,"Rule":"Comprar ações não paga imposto"},
                {"Tax":60000.00,"Rule":"Operação com lucro"}]
            }
            """;

        Validate(operations, results);
    }

    private void Validate(string operations, string results)
    {
        var operationsModel = JsonSerializer.Deserialize<IEnumerable<OrderItem>>(operations, Serializer);
        var resultsModel = JsonSerializer.Deserialize<TaxReport>(results);

        var calculatedOperations = _service.CalculateTaxForCapitalGain(operationsModel!);
        resultsModel!.Should().BeEquivalentTo(calculatedOperations);
    }
}