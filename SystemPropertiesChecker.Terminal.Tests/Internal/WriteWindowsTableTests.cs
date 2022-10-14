using SystemPropertiesChecker.Terminal.Internal;

namespace SystemPropertiesChecker.Terminal.Tests.Internal
{
    public class WriteWindowsTableTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(WriteWindowsTable).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(WriteWindowsTable sut)
        {
            sut.Should().BeAssignableTo<IWriteWindowsTable>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(WriteWindowsTable).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}