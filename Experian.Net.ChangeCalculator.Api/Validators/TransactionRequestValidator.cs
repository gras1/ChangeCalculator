namespace Experian.Net.ChangeCalculator.Api.Validators;

public class TransactionRequestValidator : AbstractValidator<TransactionRequest> {
  public TransactionRequestValidator() {
    RuleFor(x => x.AmountOfCash).GreaterThan(0.0m);
    RuleFor(x => x.Cost).GreaterThan(0.0m);
    RuleFor(customer => customer.AmountOfCash).GreaterThanOrEqualTo(customer => customer.Cost);
    RuleFor(x => x.Currency).NotEmpty();
    RuleFor(x => x.Currency).MinimumLength(3).WithMessage("The length of 'Currency' must be 3 characters.");
    RuleFor(x => x.Currency).MaximumLength(3).WithMessage("The length of 'Currency' must be 3 characters.");
  }    
}
