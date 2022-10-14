using SystemPropertiesChecker.Core.Internal.DotNet;

namespace SystemPropertiesChecker.Core.Tests.Internal.DotNet
{
    public class DotNetVersionTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(DotNetVersion).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(DotNetVersion sut)
        {
            sut.Should().BeAssignableTo<IDotNetVersion>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(DotNetVersion).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}