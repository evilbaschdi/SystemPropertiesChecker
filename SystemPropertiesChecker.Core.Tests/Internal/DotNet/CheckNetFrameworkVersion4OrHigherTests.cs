using SystemPropertiesChecker.Core.Internal.DotNet;

namespace SystemPropertiesChecker.Core.Tests.Internal.DotNet
{
    public class CheckNetFrameworkVersion4OrHigherTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(CheckNetFrameworkVersion45OrHigher).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(CheckNetFrameworkVersion45OrHigher sut)
        {
            sut.Should().BeAssignableTo<ICheckNetFrameworkVersion45OrHigher>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(CheckNetFrameworkVersion45OrHigher).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}