namespace Experian.Net.ChangeCalculator.Shared.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
    {
    }

    public ValidationException(string message)
        : base(message)
    {
    }
}
