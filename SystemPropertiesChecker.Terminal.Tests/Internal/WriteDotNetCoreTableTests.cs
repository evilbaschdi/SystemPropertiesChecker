using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using SystemPropertiesChecker.Terminal.Internal;
using Xunit;

namespace SystemPropertiesChecker.Terminal.Tests.Internal
{
    public class WriteDotNetCoreTableTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(WriteDotNetCoreTable).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(WriteDotNetCoreTable sut)
        {
            sut.Should().BeAssignableTo<IWriteDotNetCoreTable>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(WriteDotNetCoreTable).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}