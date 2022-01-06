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
        //arrange
        var expected = new TransactionResponse(new Dictionary<Denomination, int>());
        var changeHandler = new ChangeHandler();
        
        //act
        var actual = changeHandler.CalculateChange(_whenTransactionRequest);

        //assert
        actual.Should().BeEquivalentTo(expected);
    }

    [When(@"the customer gives me £6.50")]
    public void TheCustomerGivesMeSixPoundsFifty()
    {
        _whenTransactionRequest = _givenTransactionRequest with { AmountOfCash = 6.5m };
    }

    [Then(@"I expect to receive a £1 coin in change back")]
    public void IExpectToReceiveOnePoundCoinInChangeBack()
    {
        //arrange
        var denomination = new Denomination("GBP", "One Pound Coin", 1.0m);
        var denominations = new Dictionary<Denomination, int>();
        denominations.Add(denomination, 1);
        var expected = new TransactionResponse(denominations);
        var changeHandler = new ChangeHandler();
        
        //act
        var actual = changeHandler.CalculateChange(_whenTransactionRequest);

        //assert
        actual.Should().BeEquivalentTo(expected);
    }
}