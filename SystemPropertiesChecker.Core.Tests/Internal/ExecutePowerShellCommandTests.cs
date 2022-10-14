using SystemPropertiesChecker.Core.Internal;

namespace SystemPropertiesChecker.Core.Tests.Internal
{
    public class ExecutePowerShellCommandTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ExecutePowerShellCommand).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(ExecutePowerShellCommand sut)
        {
            sut.Should().BeAssignableTo<IExecutePowerShellCommand>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ExecutePowerShellCommand).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}