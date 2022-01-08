namespace Ardalis.GuardClauses;

public static class TransactionRequestGuard
{
    public static void DefaultTransactionRequest(this IGuardClause guardClause, TransactionRequest input, string parameterName)
    {
        if (input == default(TransactionRequest))
        {
            throw new ArgumentException("Cannot be default.", parameterName);
        }
    }
}
