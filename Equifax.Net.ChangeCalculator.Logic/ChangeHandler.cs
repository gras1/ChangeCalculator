namespace Equifax.Net.ChangeCalculator.Logic;

public class ChangeHandler : IChangeHandler
{
    public TransactionResponse CalculateChange(TransactionRequest request)
    {
        var transactionResponse = new TransactionResponse(new Dictionary<Denomination, decimal>(), 0m);
        if (request.AmountOfCash == request.Cost)
        {
            return transactionResponse;
        }
        throw new NotImplementedException();
    }
}
