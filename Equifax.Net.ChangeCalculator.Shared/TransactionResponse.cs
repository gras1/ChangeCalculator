namespace Equifax.Net.ChangeCalculator.Shared;

public class TransactionResponse
{
    public TransactionResponse(Dictionary<Denomination, decimal> change)
    {
        Change = change;
    }

    public Dictionary<Denomination, decimal> Change {get; private set;}
}
