namespace Experian.Net.ChangeCalculator.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ChangeCalculatorController : ControllerBase
{
    private readonly ILogger<ChangeCalculatorController> _logger;
    private readonly IChangeHandler _changeHandler;
    private readonly IChangeCalculationToTransactionResponseMapper _mapper;
    private readonly List<Denomination> _denominations;
    private readonly IRequestValidator _validator;

    public ChangeCalculatorController(ILogger<ChangeCalculatorController> logger, IChangeHandler changeHandler,
        IChangeCalculationToTransactionResponseMapper mapper, IOptions<List<Denomination>> options,
        IRequestValidator validator)
    {
        _logger = logger;
        _changeHandler = changeHandler;
        _mapper = mapper;
        _denominations = options.Value;
        _validator = validator;
    }

    /// <summary>
    /// Calculates the change for a given TransactionRequest containing a Currency, AmountOfCash and Cost
    /// </summary>
    /// <response code="200">Returns a populated TransactionResponse</response>
    /// <response code="400">If the request is null or not formed correctly</response>
    /// <response code="404">No change available or correct change not available</response>
    /// <response code="500">An unhandled exception occurred</response>
    [HttpPost()]
    public ActionResult Post([FromBody] TransactionRequest request)
    {
        try
        {
            _validator.ValidateRequest(request);
            var changeCalculation = _changeHandler.CalculateChange(request, _denominations);
            var transactionResponse = _mapper.Map(changeCalculation);
            return new OkObjectResult(transactionResponse);
        }
        catch (FluentValidation.ValidationException ex)
        {
            return new BadRequestObjectResult(ex.Message);
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
