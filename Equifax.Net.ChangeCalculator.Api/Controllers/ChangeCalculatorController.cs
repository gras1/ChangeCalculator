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
            new Denomination("GBP", "1p", 0.01m),
            new Denomination("GBP", "2p", 0.02m),
            new Denomination("GBP", "5p", 0.05m),
            new Denomination("GBP", "10p", 0.1m),
            new Denomination("GBP", "20p", 0.2m),
            new Denomination("GBP", "50p", 0.5m),
            new Denomination("GBP", "£1", 1.0m),
            new Denomination("GBP", "£2", 2.0m),
            new Denomination("GBP", "£5", 5.0m),
            new Denomination("GBP", "£10", 10.0m),
            new Denomination("GBP", "£20", 20.0m),
            new Denomination("GBP", "£50", 50.0m)};
    }

    [HttpPost()]
    public ActionResult Post([FromBody] TransactionRequest request)
    {
        var validator = new TransactionRequestValidator();
        var results = validator.Validate(request);
        if (!results.IsValid)
        {
            var failures = results.Errors.Select(err => err.ErrorMessage).ToArray();
            //TODO logging
            return new BadRequestObjectResult((String.Join(" ", failures, 0, failures.Count())).Trim());
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
