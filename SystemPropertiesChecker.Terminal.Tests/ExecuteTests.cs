using EvilBaschdi.Core;

namespace SystemPropertiesChecker.Terminal.Tests;

public class ExecuteTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(Execute).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(Execute sut)
    {
        sut.Should().BeAssignableTo<IRun>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(Execute).GetMethods().Where(method => !method.IsAbstract));
    }
}