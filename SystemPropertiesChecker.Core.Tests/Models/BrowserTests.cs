using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using SystemPropertiesChecker.Core.Models;
using Xunit;

namespace SystemPropertiesChecker.Core.Tests.Models
{
    public class BrowserTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(Browser).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(Browser).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}