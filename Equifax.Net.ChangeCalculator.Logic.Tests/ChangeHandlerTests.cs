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

    [When(@"the customer gives me £5.50 exactly")]
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
        var actual = changeHandler.CalculateChange(_whenTransactionRequest, new List<Denomination>());

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
        var availableDenominations = new List<Denomination>{ denomination };
        var denominations = new Dictionary<Denomination, int>();
        denominations.Add(denomination, 1);
        var expected = new TransactionResponse(denominations);
        var changeHandler = new ChangeHandler();
        
        //act
        var actual = changeHandler.CalculateChange(_whenTransactionRequest, availableDenominations);

        //assert
        actual.Should().BeEquivalentTo(expected);
    }

    [When(@"the customer gives me £20")]
    public void TheCustomerGivesMeTwentyPounds()
    {
        _whenTransactionRequest = _givenTransactionRequest with { AmountOfCash = 20.0m };
    }

    [Then(@"I expect to receive £14.50 in change back")]
    public void IExpectToReceiveFourteenPoundsFiftyPenceChangeBack()
    {
        //arrange
        var denomination1 = new Denomination("GBP", "One Pence Coin", 0.01m);
        var denomination2 = new Denomination("GBP", "Two Pence Coin", 0.02m);
        var denomination5 = new Denomination("GBP", "Five Pence Coin", 0.05m);
        var denomination10 = new Denomination("GBP", "Ten Pence Coin", 0.1m);
        var denomination20 = new Denomination("GBP", "Twenty Pence Coin", 0.2m);
        var denomination50 = new Denomination("GBP", "Fifty Pence Coin", 0.5m);
        var denomination100 = new Denomination("GBP", "One Pound Coin", 1.0m);
        var denomination200 = new Denomination("GBP", "Two Pounds Coin", 2.0m);
        var denomination500 = new Denomination("GBP", "Five Pounds Note", 5.0m);
        var denomination1000 = new Denomination("GBP", "Ten Pounds Note", 10.0m);
        var denomination2000 = new Denomination("GBP", "Twenty Pounds Note", 20.0m);
        var denomination5000 = new Denomination("GBP", "Fifty Pounds Note", 50.0m);
        var availableDenominations = new List<Denomination>{
            denomination1,
            denomination2,
            denomination5,
            denomination10,
            denomination20,
            denomination50,
            denomination100,
            denomination200,
            denomination500,
            denomination1000,
            denomination2000,
            denomination5000};
        var denominations = new Dictionary<Denomination, int>();
        denominations.Add(denomination1000, 1);
        denominations.Add(denomination200, 2);
        denominations.Add(denomination50, 1);
        var expected = new TransactionResponse(denominations);
        var changeHandler = new ChangeHandler();
        
        //act
        var actual = changeHandler.CalculateChange(_whenTransactionRequest, availableDenominations);

        //assert
        actual.Should().BeEquivalentTo(expected);
    }
}