using SystemPropertiesChecker.Core.Models;

namespace SystemPropertiesChecker.Core.Tests.Models
{
    public class WindowsVersionInformationModelTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(WindowsVersionInformationModel).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(WindowsVersionInformationModel sut)
        {
            sut.Should().BeAssignableTo<IWindowsVersionInformationModel>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(WindowsVersionInformationModel).GetMethods()
                                                                   .Where(method => !method.IsAbstract & !method.Name.StartsWith("set") & !method.Name.StartsWith("init")));
        }
    }
}