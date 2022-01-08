namespace Experian.Net.ChangeCalculator.Api.Validators;

public class RequestValidator : IRequestValidator
{
    public void ValidateRequest(TransactionRequest request)
    {
        if (request == default)
        {
            throw new FluentValidation.ValidationException("request parameter cannot be default");
        }
        var validator = new TransactionRequestValidator();
        var results = validator.Validate(request);
        if (!results.IsValid)
        {
            var failures = results.Errors.Select(err => err.ErrorMessage).ToArray();
            throw new FluentValidation.ValidationException((String.Join(" ", failures, 0, failures.Count())).Trim());
        }
        if (request.AmountOfCash < request.Cost)
        {
            throw new FluentValidation.ValidationException("Not enough money to make the purchase");
        }
    }
}

