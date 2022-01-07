namespace Equifax.Net.ChangeCalculator.Shared.Models;

public class TransactionResponse
{
    public TransactionResponse(List<Change> change)
    {
        Change = change;
    }

    public List<Change> Change {get; private set;}
}
