using SystemPropertiesChecker.Core.Internal;

namespace SystemPropertiesChecker.Core.Tests.Internal
{
    public class RegistryHiveLocalMachineSoftwareMicrosoftWindowsNtCurrentVersionTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(RegistryHiveLocalMachineSoftwareMicrosoftWindowsNtCurrentVersion).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(RegistryHiveLocalMachineSoftwareMicrosoftWindowsNtCurrentVersion sut)
        {
            sut.Should().BeAssignableTo<IRegistryHiveLocalMachineSoftwareMicrosoftWindowsNtCurrentVersion>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(RegistryHiveLocalMachineSoftwareMicrosoftWindowsNtCurrentVersion).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}