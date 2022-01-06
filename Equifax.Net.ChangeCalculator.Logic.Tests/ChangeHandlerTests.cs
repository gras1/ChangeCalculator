namespace Equifax.Net.ChangeCalculator.Logic.Tests;

[ExcludeFromCodeCoverage]
[FeatureFile("./features/ChangeCalculatorSpecs.feature")]
public class ChangeHandlerTests : Xunit.Gherkin.Quick.Feature
{
    private TransactionRequest _intitialTransactionRequest;
    private TransactionRequest _givenTransactionRequest;
    private TransactionRequest _whenTransactionRequest;
    private List<Denomination> _availableDenominations;
    private ChangeHandler _changeHandler;
    
    public ChangeHandlerTests()
    {
        _intitialTransactionRequest = new TransactionRequest("GBP", 0m, 0m);
        _availableDenominations = new List<Denomination>{
            Denomination1,
            Denomination2,
            Denomination5,
            Denomination10,
            Denomination20,
            Denomination50,
            Denomination100,
            Denomination200,
            Denomination500,
            Denomination1000,
            Denomination2000,
            Denomination5000};
        _changeHandler = new ChangeHandler();
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
        
        //act
        var actual = _changeHandler.CalculateChange(_whenTransactionRequest, new List<Denomination>());

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
        var expected = new TransactionResponse(new Dictionary<Denomination, int>{ {Denomination100, 1} });
        
        //act
        var actual = _changeHandler.CalculateChange(_whenTransactionRequest, _availableDenominations);

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
        var expected = new TransactionResponse(new Dictionary<Denomination, int>{
            {Denomination1000, 1},
            {Denomination200, 2},
            {Denomination50, 1}});
        
        //act
        var actual = _changeHandler.CalculateChange(_whenTransactionRequest, _availableDenominations);

        //assert
        actual.Should().BeEquivalentTo(expected);
    }

    [And(@"I don't have the correct change")]
    public void IDontHaveTheCorrectChange()
    {
        _availableDenominations = new List<Denomination>{ {new Denomination("GBP", "Five Hundred Million Pound Note - that's hyper inflation for you!", 500_000_000.0m)} };
    }

    [Then(@"I expect a TransactionFailedException to be thrown stating correct change not available")]
    public void IExpectATransactionFailedExceptionToBeThrownStatingCorrectChangeNotAvailable()
    {
        //act
        Action act = () => _changeHandler.CalculateChange(_whenTransactionRequest, _availableDenominations);

        //assert
        act.Should().Throw<TransactionFailedException>().WithMessage("Correct change not available");
    }

    [And(@"There is no change available")]
    public void ThereIsNoChangeAvailable()
    {
        _availableDenominations = new List<Denomination>();
    }

    [Then(@"I expect a TransactionFailedException to be thrown stating no change available")]
    public void IExpectATransactionFailedExceptionToBeThrownStatingNoChangeAvailable()
    {
        //act
        Action act = () => _changeHandler.CalculateChange(_whenTransactionRequest, _availableDenominations);

        //assert
        act.Should().Throw<TransactionFailedException>().WithMessage("No change available");
    }

    [Then(@"I don't expect to receive any change back and a TransactionFailedException is not thrown")]
    public void IDontExpectToReceiveAnyChangeBackAndATransactionFailedExceptionIsNotThrown()
    {
        //arrange
        var actual = new TransactionResponse(new Dictionary<Denomination, int>{ { new Denomination("USD", "test", 0.05m), 1 }});
        var expected = new TransactionResponse(new Dictionary<Denomination, int>());

        //act
        Action act = () => actual = _changeHandler.CalculateChange(_whenTransactionRequest, _availableDenominations);
        act.Invoke();

        //assert
        act.Should().NotThrow<TransactionFailedException>();
        actual.Should().BeEquivalentTo(expected);
    }

    private Denomination Denomination1 = new Denomination("GBP", "One Pence Coin", 0.01m);
    private Denomination Denomination2 = new Denomination("GBP", "Two Pence Coin", 0.02m);
    private Denomination Denomination5 = new Denomination("GBP", "Five Pence Coin", 0.05m);
    private Denomination Denomination10 = new Denomination("GBP", "Ten Pence Coin", 0.1m);
    private Denomination Denomination20 = new Denomination("GBP", "Twenty Pence Coin", 0.2m);
    private Denomination Denomination50 = new Denomination("GBP", "Fifty Pence Coin", 0.5m);
    private Denomination Denomination100 = new Denomination("GBP", "One Pound Coin", 1.0m);
    private Denomination Denomination200 = new Denomination("GBP", "Two Pounds Coin", 2.0m);
    private Denomination Denomination500 = new Denomination("GBP", "Five Pounds Note", 5.0m);
    private Denomination Denomination1000 = new Denomination("GBP", "Ten Pounds Note", 10.0m);
    private Denomination Denomination2000 = new Denomination("GBP", "Twenty Pounds Note", 20.0m);
    private Denomination Denomination5000 = new Denomination("GBP", "Fifty Pounds Note", 50.0m);
}