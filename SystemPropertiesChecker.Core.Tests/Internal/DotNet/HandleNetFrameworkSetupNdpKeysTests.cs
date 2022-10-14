using SystemPropertiesChecker.Core.Internal.DotNet;

namespace SystemPropertiesChecker.Core.Tests.Internal.DotNet
{
    public class HandleNetFrameworkSetupNdpKeysTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(HandleNetFrameworkSetupNdpKeys).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(HandleNetFrameworkSetupNdpKeys sut)
        {
            sut.Should().BeAssignableTo<IHandleNetFrameworkSetupNdpKeys>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(HandleNetFrameworkSetupNdpKeys).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}