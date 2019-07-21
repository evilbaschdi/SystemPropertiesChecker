using System.Linq;
using SystemPropertiesChecker.Core.Internal.DotNet;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace SystemPropertiesChecker.Core.Tests.Internal.DotNet
{
    public class DotNetCoreSdksTests
    {
        [Theory, AutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(DotNetCoreSdks).GetConstructors());
        }

        [Theory, AutoData]
        public void Constructor_ReturnsIDotNetCoreSdks(DotNetCoreSdks sut)
        {
            Assert.IsAssignableFrom<IDotNetCoreSdks>(sut);
        }

        [Theory, AutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(DotNetCoreSdks).GetMethods().Where(method => !method.IsAbstract));
        }

        [Theory, AutoData]
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