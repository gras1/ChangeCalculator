namespace Experian.Net.ChangeCalculator.Api.Tests.Mappers;

[ExcludeFromCodeCoverage]
#pragma warning disable xUnit1013 // Public method should be marked as test
public class ChangeCalculationToTransactionResponseMapperTests
{
    [Fact]
    public void Map_WithNullChangeCalculation_ThrowsArgumentNullException()
    {
        //arrange
        var mapper = new ChangeCalculationToTransactionResponseMapper();
        ChangeCalculation? changeCalculation = null;

        //act
        Action act = () => mapper.Map(changeCalculation!);

        //assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'changeCalculation')");
    }
    
    [Fact]
    public void Map_WithPopulatedChangeCalculation_ReturnsPopulatedTransactionResponse()
    {
        //arrange
        var mapper = new ChangeCalculationToTransactionResponseMapper();
        var changeCalculation = new ChangeCalculation(new Dictionary<Denomination, int>());
        changeCalculation.Change.Add(new Denomination("GBP", "1p", 0.01m), 1);
        var expected = new TransactionResponse(new List<string>{ "1 x 1p" });

        //act
        var actual = mapper.Map(changeCalculation);

        //assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void Map_WithEmptyChangeCalculation_ReturnsEmptyTransactionResponse()
    {
        //arrange
        var mapper = new ChangeCalculationToTransactionResponseMapper();
        var changeCalculation = new ChangeCalculation(new Dictionary<Denomination, int>());
        var expected = new TransactionResponse(new List<string>());

        //act
        var actual = mapper.Map(changeCalculation);

        //assert
        actual.Should().BeEquivalentTo(expected);
    }
#pragma warning restore xUnit1013 // Public method should be marked as test
}
