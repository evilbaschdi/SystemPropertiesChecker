using SystemPropertiesChecker.Core.Models;

namespace SystemPropertiesChecker.Core.Tests.Models;

public class SourceOsTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(SourceOs).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(SourceOs).GetMethods().Where(method => !method.IsAbstract & !method.Name.StartsWith("set") & !method.Name.StartsWith("init")));
    }
}