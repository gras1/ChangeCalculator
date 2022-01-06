namespace Equifax.Net.ChangeCalculator.Shared.Models;

public readonly record struct TransactionRequest(string Currency, decimal AmountOfCash, decimal Cost);