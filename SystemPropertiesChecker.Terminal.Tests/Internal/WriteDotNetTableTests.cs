using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using SystemPropertiesChecker.Terminal.Internal;
using Xunit;

namespace SystemPropertiesChecker.Terminal.Tests.Internal
{
    public class WriteDotNetTableTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(WriteDotNetTable).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(WriteDotNetTable sut)
        {
            sut.Should().BeAssignableTo<IWriteDotNetTable>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(WriteDotNetTable).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}