using Domain.Enums;
using FluentValidation;

namespace Application.UseCases.GetProfitTaxes;

public class ProfitTaxesValidator : AbstractValidator<ProfitTaxesInput>
{
    public ProfitTaxesValidator()
    {
        RuleFor(_ => _.Items)
            .NotEmpty()
            .WithMessage("As operações devem ser informadas");

        RuleFor(_ => _.Items.First())
            .Must(_ => _.Operation == OperationType.buy)
            .WithMessage("A operação inicial deve ser compra");

        RuleFor(_ => _.Items)
            .Must(_ => CheckNegativeOperations(_))
            .WithMessage("Ordem das operações apresenta um erro levando à quantidades negativas");

        RuleForEach(_ => _.Items)
            .SetValidator(new GetCapitalGainTaxesItemValidator());
    }

    private static bool CheckNegativeOperations(IEnumerable<ProfitTaxesItem> items)
    {
        var quantity = 0.0M;
        foreach (var item in items)
        {
            quantity = item.Operation == OperationType.buy ? quantity + item.Quantity : quantity - item.Quantity;
            if (quantity < 0) return false;
        }

        return true;
    }
}

public class GetCapitalGainTaxesItemValidator : AbstractValidator<ProfitTaxesItem>
{
    public GetCapitalGainTaxesItemValidator()
    {
        RuleFor(_ => _.UnitCost)
            .NotEmpty()
            .WithMessage("Valor unitário deve ser informado")
            .GreaterThan(0)
            .WithMessage("Valor unitário deve ser positivo");

        RuleFor(_ => _.Quantity)
            .NotEmpty()
            .WithMessage("Quantidade deve ser informada")
            .GreaterThan(0)
            .WithMessage("Quantidade deve ser um número positivo");

        RuleFor(_ => _.Operation)
            .NotEmpty()
            .WithMessage("Opção de operação deve ser informada");
    }
}