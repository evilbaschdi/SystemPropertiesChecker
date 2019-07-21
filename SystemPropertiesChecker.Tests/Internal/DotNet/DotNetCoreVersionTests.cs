using System.Linq;
using SystemPropertiesChecker.Core.Internal.DotNet;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace SystemPropertiesChecker.Core.Tests.Internal.DotNet
{
    public class DotNetCoreVersionTests
    {
        [Theory, AutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(DotNetCoreVersion).GetConstructors());
        }

        [Theory, AutoData]
        public void Constructor_ReturnsIDotNetCoreVersion(DotNetCoreVersion sut)
        {
            sut.Should().BeAssignableTo<IDotNetCoreVersion>();
        }

        [Theory, AutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(DotNetCoreVersion).GetMethods().Where(method => !method.IsAbstract));
        }

        [Theory, AutoData]
        public void Value_ForParameterVersion_ReturnsVersion(
            DotNetCoreVersion sut)
        {
            // Arrange

            // Act
            var result = sut.Value;

            // Assert
            result.Should().NotBeNullOrWhiteSpace();
        }
    }
}