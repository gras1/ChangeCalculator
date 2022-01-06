namespace Equifax.Net.ChangeCalculator.Shared.Exceptions;

public class TransactionFailedException : Exception
{
    public TransactionFailedException()
    {
    }

    public TransactionFailedException(string message)
        : base(message)
    {
    }
}
