using System.Linq;
using SystemPropertiesChecker.Core.Internal.DotNet;
using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using Xunit;

namespace SystemPropertiesChecker.Core.Tests.Internal.DotNet
{
    public class DotNetCoreSdksTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(DotNetCoreSdks).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsIDotNetCoreSdks(DotNetCoreSdks sut)
        {
            Assert.IsAssignableFrom<IDotNetCoreSdks>(sut);
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(DotNetCoreSdks).GetMethods().Where(method => !method.IsAbstract));
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Value_ForParameterListSdks_ReturnsListOfInstalledSdks(
            DotNetCoreSdks sut)
        {
            // Arrange


            // Act
            var result = sut.Value;

            // Assert
            result.Should().NotBeNullOrWhiteSpace();
        }
    }
}