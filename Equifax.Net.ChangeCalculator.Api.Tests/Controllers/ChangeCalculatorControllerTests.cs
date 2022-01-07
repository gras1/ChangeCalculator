namespace Equifax.Net.ChangeCalculator.Api.Tests.Controllers;

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
    public void Post_WithNullRequest_ThrowsArgumentNullException()
    {
        //act
        Action act = () => _controller.Post(default(TransactionRequest));

        //assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'request')");
    }
}