using SystemPropertiesChecker.Core.Internal.DotNet;

namespace SystemPropertiesChecker.Core.Tests.Internal.DotNet;

public class DotNetVersionReleaseKeyMappingTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(DotNetVersionReleaseKeyMapping).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(DotNetVersionReleaseKeyMapping sut)
    {
        sut.Should().BeAssignableTo<IDotNetVersionReleaseKeyMapping>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(DotNetVersionReleaseKeyMapping).GetMethods().Where(method => !method.IsAbstract));
    }
}