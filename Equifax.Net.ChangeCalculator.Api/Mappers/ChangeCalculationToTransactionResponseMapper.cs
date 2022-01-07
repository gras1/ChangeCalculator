namespace Equifax.Net.ChangeCalculator.Api.Mappers;

public class ChangeCalculationToTransactionResponseMapper : IChangeCalculationToTransactionResponseMapper
{
    public TransactionResponse Map(ChangeCalculation changeCalculation)
    {
        var changes = new List<string>();
        foreach (var chg in changeCalculation.Change)
        {
            changes.Add($"{chg.Value} x {chg.Key.Description}");
        }

        return new TransactionResponse(changes);
    }
}
