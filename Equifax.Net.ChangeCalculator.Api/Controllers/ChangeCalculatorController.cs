namespace Equifax.Net.ChangeCalculator.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ChangeCalculatorController : ControllerBase
{
    private readonly ILogger<ChangeCalculatorController> _logger;
    private readonly IChangeHandler _changeHandler;
    private readonly IChangeCalculationToTransactionResponseMapper _mapper;
    private readonly List<Denomination> _denominations;

    public ChangeCalculatorController(ILogger<ChangeCalculatorController> logger, IChangeHandler changeHandler,
        IChangeCalculationToTransactionResponseMapper mapper)
    {
        _logger = logger;
        _changeHandler = changeHandler;
        _mapper = mapper;
        _denominations = new List<Denomination>{
            new Denomination("GBP", "One Pence Coin", 0.01m),
            new Denomination("GBP", "Two Pence Coin", 0.02m),
            new Denomination("GBP", "Five Pence Coin", 0.05m),
            new Denomination("GBP", "Ten Pence Coin", 0.1m),
            new Denomination("GBP", "Twenty Pence Coin", 0.2m),
            new Denomination("GBP", "Fifty Pence Coin", 0.5m),
            new Denomination("GBP", "One Pound Coin", 1.0m),
            new Denomination("GBP", "Two Pounds Coin", 2.0m),
            new Denomination("GBP", "Five Pounds Note", 5.0m),
            new Denomination("GBP", "Ten Pounds Note", 10.0m),
            new Denomination("GBP", "Twenty Pounds Note", 20.0m),
            new Denomination("GBP", "Fifty Pounds Note", 50.0m)};
    }

    [HttpPost()]
    public ActionResult Post([FromBody] TransactionRequest request)
    {
        var validator = new TransactionRequestValidator();
        var results = validator.Validate(request);
        if (!results.IsValid)
        {
            var failures = results.Errors;
            //TODO logging
            return new BadRequestObjectResult(String.Join(",", failures.SelectMany(f => f.ErrorMessage)));
        }

        try
        {
            var changeCalculation = _changeHandler.CalculateChange(request, _denominations);
            var transactionResponse = _mapper.Map(changeCalculation);
            return new OkObjectResult(transactionResponse);
        }
        catch (TransactionFailedException ex)
        {
            //TODO logging
            return new NotFoundObjectResult(ex.Message);
        }
        catch (Exception)
        {
            //TODO logging
            return new StatusCodeResult(500);
        }
    }
}
