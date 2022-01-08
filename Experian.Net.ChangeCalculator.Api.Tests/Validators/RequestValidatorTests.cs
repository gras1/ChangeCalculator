namespace Experian.Net.ChangeCalculator.Api.Tests.Validators;

[ExcludeFromCodeCoverage]
public class RequestValidatorTests
{
    private readonly RequestValidator _validator;

    public RequestValidatorTests()
    {
        _validator = new RequestValidator();
    }

    [Fact]
    public void ValidateRequest_WithDefaultRequest_ThrowsValidationException()
    {
        //act
        Action act = () => _validator.ValidateRequest(default);

        //assert
        act.Should().Throw<FluentValidation.ValidationException>().WithMessage("request parameter cannot be default");
    }

    [Fact]
    public void ValidateRequest_WithInvalidCurrency1Request_ThrowsValidationException()
    {
        //act
        Action act = () => _validator.ValidateRequest(new TransactionRequest("test", 2.01m, 2.0m));

        //assert
        act.Should().Throw<FluentValidation.ValidationException>().WithMessage("The length of 'Currency' must be 3 characters.");
    }

    [Fact]
    public void ValidateRequest_WithInvalidCurrency2Request_ThrowsValidationException()
    {
        //act
        Action act = () => _validator.ValidateRequest(new TransactionRequest("t", 2.01m, 2.0m));

        //assert
        act.Should().Throw<FluentValidation.ValidationException>().WithMessage("The length of 'Currency' must be 3 characters.");
    }

    [Fact]
    public void ValidateRequest_WithInvalidAmountOfCash1Request_ThrowsValidationException()
    {
        //act
        Action act = () => _validator.ValidateRequest(new TransactionRequest("GBP", 1.9m, 2.0m));

        //assert
        act.Should().Throw<FluentValidation.ValidationException>().WithMessage("Not enough money to make the purchase");
    }

    [Fact]
    public void ValidateRequest_WithInvalidAmountOfCash2Request_ThrowsValidationException()
    {
        //act
        Action act = () => _validator.ValidateRequest(new TransactionRequest("GBP", 0.0m, 2.0m));

        //assert
        act.Should().Throw<FluentValidation.ValidationException>().WithMessage("'Amount Of Cash' must be greater than '0.0'.");
    }

    [Fact]
    public void ValidateRequest_WithInvalidCost1Request_ThrowsValidationException()
    {
        //act
        Action act = () => _validator.ValidateRequest(new TransactionRequest("GBP", 2.0m, 0.0m));

        //assert
        act.Should().Throw<FluentValidation.ValidationException>().WithMessage("'Cost' must be greater than '0.0'.");
    }

    [Fact]
    public void ValidateRequest_WithValidRequest_DoesNotThrowValidationException()
    {
        //act
        Action act = () => _validator.ValidateRequest(new TransactionRequest("XXX", 2.0m, 0.1m));

        //assert
        act.Should().NotThrow<FluentValidation.ValidationException>();
    }
}

