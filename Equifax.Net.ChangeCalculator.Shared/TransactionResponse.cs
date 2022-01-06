namespace Equifax.Net.ChangeCalculator.Shared;

public class TransactionResponse
{
    public TransactionResponse(Dictionary<Denomination, int> change)
    {
        Change = change;
    }

    public Dictionary<Denomination, int> Change {get; private set;}

    public string ChangeInWords()
    {
        var sb = new StringBuilder();
        foreach (var chg in this.Change) {
            sb.Append($"{chg.Value} x {chg.Key.Description}, ");
        }
        return sb.ToString().Trim().TrimEnd(',');
    }
}
