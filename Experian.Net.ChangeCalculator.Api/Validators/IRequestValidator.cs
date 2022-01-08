namespace Experian.Net.ChangeCalculator.Api.Validators;

public interface IRequestValidator
{
    void ValidateRequest(TransactionRequest request);
}

