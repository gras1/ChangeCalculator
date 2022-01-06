namespace Equifax.Net.ChangeCalculator.Shared;

public class TransactionResponse
{
    public TransactionResponse(Dictionary<Denomination, decimal> change, decimal totalChange)
    {
        Change = change;
        TotalChange = totalChange;
    }

    public Dictionary<Denomination, decimal> Change {get; private set;}
    public decimal TotalChange {get; private set;}
}
