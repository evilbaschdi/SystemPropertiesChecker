using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using SystemPropertiesChecker.Core.Internal;
using Xunit;

namespace SystemPropertiesChecker.Core.Tests.Internal
{
    public class PasswordExpirationDateTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(PasswordExpirationDate).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(PasswordExpirationDate sut)
        {
            sut.Should().BeAssignableTo<IPasswordExpirationDate>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(PasswordExpirationDate).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}