namespace Equifax.Net.ChangeCalculator.Logic;

public interface IChangeHandler
{
    ChangeCalculation CalculateChange(TransactionRequest request, IEnumerable<Denomination> denominations);
}
