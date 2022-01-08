namespace Experian.Net.ChangeCalculator.Shared.Dtos;

public readonly record struct Denomination(string Currency, string Description, decimal Value);
