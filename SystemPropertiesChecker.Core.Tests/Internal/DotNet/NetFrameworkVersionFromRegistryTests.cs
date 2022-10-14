using SystemPropertiesChecker.Core.Internal.DotNet;

namespace SystemPropertiesChecker.Core.Tests.Internal.DotNet
{
    public class NetFrameworkVersionFromRegistryTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(NetFrameworkVersionFromRegistry).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(NetFrameworkVersionFromRegistry sut)
        {
            sut.Should().BeAssignableTo<INetFrameworkVersionFromRegistry>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(NetFrameworkVersionFromRegistry).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}