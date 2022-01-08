namespace Equifax.Net.ChangeCalculator.Api.Mappers;

public class ChangeCalculationToTransactionResponseMapper : IChangeCalculationToTransactionResponseMapper
{
    public TransactionResponse Map(ChangeCalculation changeCalculation)
    {
        Guard.Against.Null(changeCalculation, nameof(changeCalculation));

        var change = new List<string>();
        foreach (var chg in changeCalculation.Change)
        {
            change.Add($"{chg.Value} x {chg.Key.Description}");
        }

        return new TransactionResponse(change);
    }
}
