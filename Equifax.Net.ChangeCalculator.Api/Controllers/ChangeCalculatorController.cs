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
        IChangeCalculationToTransactionResponseMapper mapper, IOptions<List<Denomination>> options)
    {
        _logger = logger;
        _changeHandler = changeHandler;
        _mapper = mapper;
        _denominations = options.Value;
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
