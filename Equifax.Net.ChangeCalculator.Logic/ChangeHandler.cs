namespace Equifax.Net.ChangeCalculator.Logic;

public class ChangeHandler : IChangeHandler
{
    public TransactionResponse CalculateChange(TransactionRequest request, IEnumerable<Denomination> denominations)
    {
        var transactionResponse = new TransactionResponse(new Dictionary<Denomination, int>());
        if (request.AmountOfCash == request.Cost)
        {
            return transactionResponse;
        }
        var remainingTotal = request.AmountOfCash - request.Cost;
        var availableDenominations = denominations.Where(d => d.Currency == request.Currency).OrderByDescending(d => d.Value);
        if (!availableDenominations.Any())
        {
            throw new TransactionFailedException("No change available");
        }
        foreach (var denomination in availableDenominations)
        {
            var quantity = (int)(remainingTotal / denomination.Value);
            if (quantity >= 1)
            {
                remainingTotal = remainingTotal - (denomination.Value * quantity);
                transactionResponse.Change.Add(new Denomination(denomination.Currency, denomination.Description, denomination.Value), quantity);
            }
        }
        if (remainingTotal > 0.0m)
        {
            throw new TransactionFailedException("Correct change not available");
        }
        return transactionResponse;
    }
}
