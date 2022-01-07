namespace Equifax.Net.ChangeCalculator.Api.Tests.Controllers;

[ExcludeFromCodeCoverage]
public class ChangeCalculatorControllerTests
{
    private readonly ILogger<ChangeCalculatorController> _logger;
    private readonly IChangeHandler _changeHandler;
    private readonly IChangeCalculationToTransactionResponseMapper _mapper;
    private readonly IOptions<List<Denomination>> _options;
    private readonly ChangeCalculatorController _controller;
    
    public ChangeCalculatorControllerTests()
    {
        _logger = A.Fake<ILogger<ChangeCalculatorController>>();
        _changeHandler = A.Fake<IChangeHandler>();
        _mapper = A.Fake<IChangeCalculationToTransactionResponseMapper>();
        _options = A.Fake<IOptions<List<Denomination>>>();
        _controller = new ChangeCalculatorController(_logger, _changeHandler, _mapper, _options);
    }

    [Fact]
    public void Post_WithDefaultRequest_ReturnsBadRequestObjectResult()
    {
        //act
        ActionResult actionResult = _controller.Post(default(TransactionRequest));
        var response = actionResult;
        var badRequestObjectResult = response as BadRequestObjectResult;

        //assert
        Assert.NotNull(badRequestObjectResult);
        badRequestObjectResult?.StatusCode.Should().Be(400);
        ((string)badRequestObjectResult?.Value!).Should().Be("request parameter cannot be default");
    }

    [Fact]
    public void Post_WithInvalidCurrency1Request_ReturnsBadRequestObjectResult()
    {
        TransactionRequest request = new TransactionRequest("test", 2.01m, 2.0m);
        //act
        ActionResult actionResult = _controller.Post(request);
        var response = actionResult;
        var badRequestObjectResult = response as BadRequestObjectResult;

        //assert
        Assert.NotNull(badRequestObjectResult);
        badRequestObjectResult?.StatusCode.Should().Be(400);
        ((string)badRequestObjectResult?.Value!).Should().Be("The length of 'Currency' must be 3 characters.");
    }

    [Fact]
    public void Post_WithInvalidCurrency2Request_ReturnsBadRequestObjectResult()
    {
        TransactionRequest request = new TransactionRequest("t", 2.01m, 2.0m);
        //act
        ActionResult actionResult = _controller.Post(request);
        var response = actionResult;
        var badRequestObjectResult = response as BadRequestObjectResult;

        //assert
        Assert.NotNull(badRequestObjectResult);
        badRequestObjectResult?.StatusCode.Should().Be(400);
        ((string)badRequestObjectResult?.Value!).Should().Be("The length of 'Currency' must be 3 characters.");
    }

    [Fact]
    public void Post_WithInvalidAmountOfCash1Request_ReturnsBadRequestObjectResult()
    {
        TransactionRequest request = new TransactionRequest("GBP", 1.9m, 2.0m);
        //act
        ActionResult actionResult = _controller.Post(request);
        var response = actionResult;
        var badRequestObjectResult = response as BadRequestObjectResult;

        //assert
        Assert.NotNull(badRequestObjectResult);
        badRequestObjectResult?.StatusCode.Should().Be(400);
        ((string)badRequestObjectResult?.Value!).Should().Be("'Amount Of Cash' must be greater than or equal to '2.0'.");
    }

    [Fact]
    public void Post_WithInvalidAmountOfCash2Request_ReturnsBadRequestObjectResult()
    {
        TransactionRequest request = new TransactionRequest("GBP", 0.0m, 2.0m);
        //act
        ActionResult actionResult = _controller.Post(request);
        var response = actionResult;
        var badRequestObjectResult = response as BadRequestObjectResult;

        //assert
        Assert.NotNull(badRequestObjectResult);
        badRequestObjectResult?.StatusCode.Should().Be(400);
        ((string)badRequestObjectResult?.Value!).Should().Be("'Amount Of Cash' must be greater than '0.0'. 'Amount Of Cash' must be greater than or equal to '2.0'.");
    }

    [Fact]
    public void Post_WithInvalidCost1Request_ReturnsBadRequestObjectResult()
    {
        TransactionRequest request = new TransactionRequest("GBP", 2.0m, 0.0m);
        //act
        ActionResult actionResult = _controller.Post(request);
        var response = actionResult;
        var badRequestObjectResult = response as BadRequestObjectResult;

        //assert
        Assert.NotNull(badRequestObjectResult);
        badRequestObjectResult?.StatusCode.Should().Be(400);
        ((string)badRequestObjectResult?.Value!).Should().Be("'Cost' must be greater than '0.0'.");
    }
}