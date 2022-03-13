using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using SystemPropertiesChecker.Terminal.Internal;
using Xunit;

namespace SystemPropertiesChecker.Terminal.Tests.Internal
{
    public class WriteHistoryTableTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(WriteHistoryTable).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(WriteHistoryTable sut)
        {
            sut.Should().BeAssignableTo<IWriteHistoryTable>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(WriteHistoryTable).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}