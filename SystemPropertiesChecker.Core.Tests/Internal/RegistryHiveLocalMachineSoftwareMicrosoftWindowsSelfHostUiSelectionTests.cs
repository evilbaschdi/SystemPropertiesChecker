using SystemPropertiesChecker.Core.Internal;

namespace SystemPropertiesChecker.Core.Tests.Internal
{
    public class RegistryHiveLocalMachineSoftwareMicrosoftWindowsSelfHostUiSelectionTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(RegistryHiveLocalMachineSoftwareMicrosoftWindowsSelfHostUiSelection).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(RegistryHiveLocalMachineSoftwareMicrosoftWindowsSelfHostUiSelection sut)
        {
            sut.Should().BeAssignableTo<IRegistryHiveLocalMachineSoftwareMicrosoftWindowsSelfHostUiSelection>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(RegistryHiveLocalMachineSoftwareMicrosoftWindowsSelfHostUiSelection).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}