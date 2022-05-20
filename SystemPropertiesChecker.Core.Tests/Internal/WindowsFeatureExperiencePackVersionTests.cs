using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using SystemPropertiesChecker.Core.Internal;
using Xunit;

namespace SystemPropertiesChecker.Core.Tests.Internal
{
    public class WindowsFeatureExperiencePackVersionTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(WindowsFeatureExperiencePackVersion).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(WindowsFeatureExperiencePackVersion sut)
        {
            sut.Should().BeAssignableTo<IWindowsFeatureExperiencePackVersion>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(WindowsFeatureExperiencePackVersion).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}