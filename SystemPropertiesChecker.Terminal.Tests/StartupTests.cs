using EvilBaschdi.Core.DependencyInjection;

namespace SystemPropertiesChecker.Terminal.Tests;

public class StartupTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(Startup).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(Startup sut)
    {
        sut.Should().BeAssignableTo<IConfigureServiceCollection>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(Startup).GetMethods().Where(method => !method.IsAbstract));
    }
}