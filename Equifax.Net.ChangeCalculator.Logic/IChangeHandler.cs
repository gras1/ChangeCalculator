namespace Equifax.Net.ChangeCalculator.Logic;

public interface IChangeHandler
{
    TransactionResponse CalculateChange(TransactionRequest request, IEnumerable<Denomination> denominations);
}
