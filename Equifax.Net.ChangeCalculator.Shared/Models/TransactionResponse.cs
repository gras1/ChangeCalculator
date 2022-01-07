namespace Equifax.Net.ChangeCalculator.Shared.Models;

public class TransactionResponse
{
    public TransactionResponse(List<string> change)
    {
        Change = change;
    }

    public List<string> Change {get; private set;}
}
