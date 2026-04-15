using SystemPropertiesChecker.Core.Internal.DotNet;

namespace SystemPropertiesChecker.Core.Tests.Internal.DotNet;

public class ParseReleaseKeyByReleaseKeyMappingListTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ParseReleaseKeyByReleaseKeyMappingList).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ParseReleaseKeyByReleaseKeyMappingList sut)
    {
        sut.Should().BeAssignableTo<IParseReleaseKeyByReleaseKeyMappingList>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ParseReleaseKeyByReleaseKeyMappingList).GetMethods().Where(method => !method.IsAbstract));
    }
}