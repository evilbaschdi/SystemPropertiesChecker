using System.Linq;
using SystemPropertiesChecker.Core.Internal.DotNet;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace SystemPropertiesChecker.Core.Tests.Internal.DotNet
{
    public class DotNetCoreRuntimesTests
    {
        [Theory, AutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(DotNetCoreRuntimes).GetConstructors());
        }

        [Theory, AutoData]
        public void Constructor_ReturnsIDotNetCoreRuntimes(DotNetCoreRuntimes sut)
        {
            Assert.IsAssignableFrom<IDotNetCoreRuntimes>(sut);
        }

        [Theory, AutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(DotNetCoreRuntimes).GetMethods().Where(method => !method.IsAbstract));
        }

        [Theory, AutoData]
        public void Value_ForParameterListRuntimes_ReturnsListOfInstalledRuntimes(
            DotNetCoreRuntimes sut)
        {
            // Arrange


            // Act
            var result = sut.Value;

            // Assert
            result.Should().NotBeNullOrWhiteSpace();
        }
    }
}