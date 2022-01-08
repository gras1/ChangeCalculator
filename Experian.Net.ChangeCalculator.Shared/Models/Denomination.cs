namespace Experian.Net.ChangeCalculator.Shared.Models;

public readonly record struct Denomination(string Currency, string Description, decimal Value);
