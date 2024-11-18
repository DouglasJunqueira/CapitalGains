using Domain.Enums;

namespace Domain.Operations;

public record OrderItem(OperationType Operation, decimal UnitCost, decimal Quantity);
