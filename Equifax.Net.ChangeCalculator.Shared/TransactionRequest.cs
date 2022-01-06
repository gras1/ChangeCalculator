namespace Equifax.Net.ChangeCalculator.Shared;

public readonly record struct TransactionRequest(string Currency, decimal AmountOfCash, decimal Cost);