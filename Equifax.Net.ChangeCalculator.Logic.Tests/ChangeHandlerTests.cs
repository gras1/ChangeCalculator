namespace Equifax.Net.ChangeCalculator.Logic.Tests;

[ExcludeFromCodeCoverage]
[FeatureFile("./features/ChangeCalculatorSpecs.feature")]
public class ChangeHandlerTests : Xunit.Gherkin.Quick.Feature
{
    private TransactionRequest _intitialTransactionRequest;
    private TransactionRequest _givenTransactionRequest;
    private TransactionRequest _whenTransactionRequest;
    
    public ChangeHandlerTests()
    {
        _intitialTransactionRequest = new TransactionRequest("GBP", 0m, 0m);
    }

    [Given(@"the customer buys something for £5.50")]
    public void CustomerBuysSomethingForFivePoundsFifty()
    {
        _givenTransactionRequest = _intitialTransactionRequest with { Cost = 5.5m };
    }

    [When(@"the customer gives me £5.50")]
    public void TheCustomerGivesMeFivePoundsFifty()
    {
        _whenTransactionRequest = _givenTransactionRequest with { AmountOfCash = 5.5m };
    }

    [Then(@"I don't expect to receive any change back")]
    public void IDontExpectToReceiveAnyChangeBack()
    {
        var expected = new TransactionResponse(new Dictionary<Denomination, decimal>(), 0m);
        var changeHandler = new ChangeHandler();
        
        var actual = changeHandler.CalculateChange(_whenTransactionRequest);

        actual.TotalChange.Should().Be(expected.TotalChange);
    }
}