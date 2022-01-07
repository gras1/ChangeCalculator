namespace Equifax.Net.ChangeCalculator.Api.Mappers;

public class ChangeCalculationToTransactionResponseMapper : IChangeCalculationToTransactionResponseMapper
{
    public TransactionResponse Map(ChangeCalculation changeCalculation)
    {
        var changes = new List<Change>();
        foreach (var chg in changeCalculation.Change)
        {
            changes.Add(new Change{
                Denomination = new Denomination(chg.Key.Currency, chg.Key.Description, chg.Key.Value),
                Quantity = chg.Value
            });
        }

        return new TransactionResponse(changes);
    }
}
