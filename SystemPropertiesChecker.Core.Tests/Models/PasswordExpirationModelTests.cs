using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using SystemPropertiesChecker.Core.Models;
using Xunit;

namespace SystemPropertiesChecker.Core.Tests.Models
{
    public class PasswordExpirationModelTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(PasswordExpirationModel).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(PasswordExpirationModel).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}