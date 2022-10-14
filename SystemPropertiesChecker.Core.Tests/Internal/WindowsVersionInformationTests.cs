using SystemPropertiesChecker.Core.Internal;

namespace SystemPropertiesChecker.Core.Tests.Internal
{
    public class WindowsVersionInformationTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(WindowsVersionInformation).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(WindowsVersionInformation sut)
        {
            sut.Should().BeAssignableTo<IWindowsVersionInformation>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(WindowsVersionInformation).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}