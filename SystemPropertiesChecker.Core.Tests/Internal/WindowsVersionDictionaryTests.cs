using SystemPropertiesChecker.Core.Internal;

namespace SystemPropertiesChecker.Core.Tests.Internal
{
    public class WindowsVersionDictionaryTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(WindowsVersionDictionary).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(WindowsVersionDictionary sut)
        {
            sut.Should().BeAssignableTo<IWindowsVersionDictionary>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(WindowsVersionDictionary).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}