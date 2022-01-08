namespace Experian.Net.ChangeCalculator.Api.Tests.Controllers;

[ExcludeFromCodeCoverage]
[FeatureFile("./features/ChangeCalculatorControllerSpecs.feature")]
#pragma warning disable xUnit1013 // Public method should be marked as test
public class ChangeCalculatorControllerTests : Feature
{
    private readonly ILogger<ChangeCalculatorController> _logger;
    private readonly IChangeHandler _changeHandler;
    private readonly IChangeCalculationToTransactionResponseMapper _mapper;
    private readonly IOptions<List<Denomination>> _options;
    private readonly IRequestValidator _validator;
    private readonly ChangeCalculatorController _controller;
    private HttpClient _client;
    private TransactionRequest _intitialTransactionRequest;
    private TransactionRequest _givenTransactionRequest;
    private TransactionRequest _whenTransactionRequest;

    public ChangeCalculatorControllerTests()
    {
        _logger = A.Fake<ILogger<ChangeCalculatorController>>();
        _changeHandler = A.Fake<IChangeHandler>();
        _mapper = A.Fake<IChangeCalculationToTransactionResponseMapper>();
        _options = A.Fake<IOptions<List<Denomination>>>();
        _validator = A.Fake<IRequestValidator>();
        _controller = new ChangeCalculatorController(_logger, _changeHandler, _mapper, _options, _validator);

        var application = new WebApplicationFactory<Program>();
        var clientOptions = new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            BaseAddress = new Uri("http://localhost:5164")
        };
        _client = application.CreateClient(clientOptions);
        _intitialTransactionRequest = new TransactionRequest("GBP", 0m, 0m);
    }

    [Given(@"the customer buys something for £5.50")]
    public void CustomerBuysSomethingForFivePoundsFifty()
    {
        _givenTransactionRequest = _intitialTransactionRequest with { Cost = 5.5m };
    }

    [When(@"the customer gives me £20")]
    public void TheCustomerGivesMeTwentyPounds()
    {
        _whenTransactionRequest = _givenTransactionRequest with { AmountOfCash = 20.0m };
    }

    [Then(@"I expect to receive £14.50 in change back")]
    public async Task IExpectToReceiveFourteenPoundsFiftyPenceChangeBack()
    {
        //arrange
        var jsonString = JsonSerializer.Serialize(_whenTransactionRequest);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        var expectedTransactionResponse = new TransactionResponse(
            new List<string> {
                "1 x £10",
                "2 x £2",
                "1 x 50p"
            });
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        //act
        var actualResponse = await _client.PostAsync("/ChangeCalculator", content);

        //assert
        actualResponse.IsSuccessStatusCode.Should().BeTrue();

        var response = await actualResponse.Content.ReadAsStringAsync();
        var actualTransactionResponse = JsonSerializer.Deserialize<TransactionResponse>(response, options);

        actualTransactionResponse.Should().BeEquivalentTo(expectedTransactionResponse);
    }

    [When(@"the customer gives me £5.50 exactly")]
    public void TheCustomerGivesMeFivePoundsFifty()
    {
        _whenTransactionRequest = _givenTransactionRequest with { AmountOfCash = 5.5m };
    }

    [Then(@"I don't expect to receive any change back")]
    public async Task IDontExpectToReceiveAnyChangeBack()
    {
        //arrange
        var jsonString = JsonSerializer.Serialize(_whenTransactionRequest);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        var expectedTransactionResponse = new TransactionResponse(new List<string>());
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        //act
        var actualResponse = await _client.PostAsync("/ChangeCalculator", content);

        //assert
        actualResponse.IsSuccessStatusCode.Should().BeTrue();

        var response = await actualResponse.Content.ReadAsStringAsync();
        var actualTransactionResponse = JsonSerializer.Deserialize<TransactionResponse>(response, options);

        actualTransactionResponse.Should().BeEquivalentTo(expectedTransactionResponse);
    }

    [When(@"the customer gives me £1")]
    public void TheCustomerGivesMeOnePound()
    {
        _whenTransactionRequest = _givenTransactionRequest with { AmountOfCash = 1.0m };
    }

    [Then(@"A bad request is returned that states there is 'Not enough money to make the purchase'")]
    public async Task ABadRequestIsReturnedThatStatesThereIsNotEnoughMoneyToMakeThePurchase()
    {
        //arrange
        var jsonString = JsonSerializer.Serialize(_whenTransactionRequest);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        //act
        var actualResponse = await _client.PostAsync("/ChangeCalculator", content);

        //assert
        actualResponse.IsSuccessStatusCode.Should().BeFalse();
        ((int)actualResponse.StatusCode).Should().Be(400);

        var response = await actualResponse.Content.ReadAsStringAsync();
        response.Should().Be("Not enough money to make the purchase");
    }

    [Fact]
    public void Post_WhenCalculateChangeThrowsTransactionFailedException_ReturnsNotFoundObjectResult()
    {
        //arrange
        var request = new TransactionRequest("GBP", 2.0m, 1.0m);
        A.CallTo(() => _changeHandler.CalculateChange(A<TransactionRequest>.Ignored, A<IEnumerable<Denomination>>.Ignored)).Throws(new TransactionFailedException("test"));

        //act
        ActionResult actionResult = _controller.Post(request);
        var response = actionResult;
        var notFoundObjectResult = response as NotFoundObjectResult;

        //assert
        Assert.NotNull(notFoundObjectResult);
        notFoundObjectResult?.StatusCode.Should().Be(404);
        ((string)notFoundObjectResult?.Value!).Should().Be("test");
    }

    [Fact]
    public void Post_WhenCalculateChangeThrowsException_ReturnsStatusCodeResult()
    {
        //arrange
        var request = new TransactionRequest("GBP", 2.0m, 1.0m);
        A.CallTo(() => _changeHandler.CalculateChange(A<TransactionRequest>.Ignored, A<IEnumerable<Denomination>>.Ignored)).Throws(new Exception());

        //act
        ActionResult actionResult = _controller.Post(request);
        var response = actionResult;
        var statusCodeResult = response as StatusCodeResult;

        //assert
        Assert.NotNull(statusCodeResult);
        statusCodeResult?.StatusCode.Should().Be(500);
    }

    [Fact]
    public void Post_WhenMapThrowsArgumentNullException_ReturnsStatusCodeResult()
    {
        //arrange
        var request = new TransactionRequest("GBP", 2.0m, 1.0m);
        var changeCalculation = new ChangeCalculation(new Dictionary<Denomination, int>());
        A.CallTo(() => _changeHandler.CalculateChange(A<TransactionRequest>.Ignored, A<IEnumerable<Denomination>>.Ignored)).Returns(changeCalculation);
        A.CallTo(() => _mapper.Map(A<ChangeCalculation>.Ignored)).Throws(new ArgumentNullException());

        //act
        ActionResult actionResult = _controller.Post(request);
        var response = actionResult;
        var statusCodeResult = response as StatusCodeResult;

        //assert
        Assert.NotNull(statusCodeResult);
        statusCodeResult?.StatusCode.Should().Be(500);
    }

    [Fact]
    public void Post_WhenMapReturnsPopulatedTransactionResponse_ReturnsChange()
    {
        //arrange
        var request = new TransactionRequest("GBP", 2.0m, 1.0m);
        var changeCalculation = new ChangeCalculation(new Dictionary<Denomination, int>());
        var response = new TransactionResponse(new List<string>{ "1 x 1p" });
        A.CallTo(() => _changeHandler.CalculateChange(A<TransactionRequest>.Ignored, A<IEnumerable<Denomination>>.Ignored)).Returns(changeCalculation);
        A.CallTo(() => _mapper.Map(A<ChangeCalculation>.Ignored)).Returns(response);

        //act
        ActionResult actionResult = _controller.Post(request);
        var actualResponse = actionResult;
        var okObjectResult = actualResponse as OkObjectResult;

        //assert
        Assert.NotNull(okObjectResult);
        okObjectResult?.StatusCode.Should().Be(200);
        ((TransactionResponse)okObjectResult?.Value!).Should().BeEquivalentTo(response);
    }
#pragma warning restore xUnit1013 // Public method should be marked as test
}