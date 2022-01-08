namespace Experian.Net.ChangeCalculator.Shared.Dtos;

public class ChangeCalculation
{
    public ChangeCalculation(Dictionary<Denomination, int> change)
    {
        Change = change;
    }

    public Dictionary<Denomination, int> Change {get; private set;}
}
