using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using SystemPropertiesChecker.Core.Internal;
using Xunit;

namespace SystemPropertiesChecker.Core.Tests.Internal
{
    public class ConfigureCoreServicesTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ConfigureCoreServices).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(ConfigureCoreServices sut)
        {
            sut.Should().BeAssignableTo<IConfigureCoreServices>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ConfigureCoreServices).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}