using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using SystemPropertiesChecker.Core.Internal;
using Xunit;

namespace SystemPropertiesChecker.Core.Tests.Internal
{
    public class OtherInformationTextTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(OtherInformationText).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(OtherInformationText sut)
        {
            sut.Should().BeAssignableTo<IOtherInformationText>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(OtherInformationText).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}