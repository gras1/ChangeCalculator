using ValidationException = Experian.Net.ChangeCalculator.Shared.Exceptions.ValidationException;

namespace Experian.Net.ChangeCalculator.Api.Controllers;

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

    /// <summary>
    /// Calculates the change for a given TransactionRequest containing a Currency, AmountOfCash and Cost
    /// </summary>
    /// <response code="200">Returns a populated TransactionResponse</response>
    /// <response code="400">If the request is null or not formed correctly</response>
    /// <response code="500">An unhandled exception occurred</response>
    [HttpPost()]
    public ActionResult Post([FromBody] TransactionRequest request)
    {
        try
        {
            ValidateRequest(request);
        }
        catch (ValidationException ex)
        {
            return new BadRequestObjectResult(ex.Message);
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

    private void ValidateRequest(TransactionRequest request)
    {
        if (request == default(TransactionRequest))
        {
            throw new ValidationException("request parameter cannot be default");
        }
        var validator = new TransactionRequestValidator();
        var results = validator.Validate(request);
        if (!results.IsValid)
        {
            var failures = results.Errors.Select(err => err.ErrorMessage).ToArray();
            throw new ValidationException((String.Join(" ", failures, 0, failures.Count())).Trim());
        }
        if (request.AmountOfCash < request.Cost)
        {
            throw new ValidationException("Not enough money to make the purchase");
        }
    }
}
