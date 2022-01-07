namespace Equifax.Net.ChangeCalculator.Api.Mappers;

public interface IChangeCalculationToTransactionResponseMapper
{
    TransactionResponse Map(ChangeCalculation changeCalculation);
}
