using System.Collections.ObjectModel;
using EvilBaschdi.Core;
using SystemPropertiesChecker.Core.Internal;
using SystemPropertiesChecker.Core.Models;

namespace SystemPropertiesChecker.Core.Tests.Internal
{
    public class HklmSystemSetupSourcesInstallDatesTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(HklmSystemSetupSourcesInstallDates).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(HklmSystemSetupSourcesInstallDates sut)
        {
            sut.Should().BeAssignableTo<ISourceOsCollection>();
            sut.Should().BeAssignableTo<CachedValue<ObservableCollection<SourceOs>>>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(HklmSystemSetupSourcesInstallDates).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}