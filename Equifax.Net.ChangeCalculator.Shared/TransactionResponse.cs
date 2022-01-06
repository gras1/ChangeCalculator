namespace Equifax.Net.ChangeCalculator.Shared;

public class TransactionResponse
{
    public TransactionResponse(Dictionary<Denomination, int> change)
    {
        Change = change;
    }

    public Dictionary<Denomination, int> Change {get; private set;}
}
