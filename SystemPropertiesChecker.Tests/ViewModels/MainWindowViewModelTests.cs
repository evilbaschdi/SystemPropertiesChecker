using EvilBaschdi.CoreExtended.Mvvm.ViewModel;
using SystemPropertiesChecker.ViewModels;

namespace SystemPropertiesChecker.Tests.ViewModels
{
    public class MainWindowViewModelTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(MainWindowViewModel).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(MainWindowViewModel sut)
        {
            sut.Should().BeAssignableTo<ApplicationStyleViewModel>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(MainWindowViewModel).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}