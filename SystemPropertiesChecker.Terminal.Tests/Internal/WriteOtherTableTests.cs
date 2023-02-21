using SystemPropertiesChecker.Terminal.Internal;

namespace SystemPropertiesChecker.Terminal.Tests.Internal;

public class WriteOtherTableTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(WriteOtherTable).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(WriteOtherTable sut)
    {
        sut.Should().BeAssignableTo<IWriteOtherTable>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(WriteOtherTable).GetMethods().Where(method => !method.IsAbstract));
    }
}