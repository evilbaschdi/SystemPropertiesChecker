using AutoFixture.Idioms;
using EvilBaschdi.DependencyInjection;
using EvilBaschdi.Testing;
using FluentAssertions;
using Xunit;

namespace SystemPropertiesChecker.Terminal.Tests
{
    public class ConfigureServicesTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ConfigureServices).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(ConfigureServices sut)
        {
            sut.Should().BeAssignableTo<IConfigureServices>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ConfigureServices).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}