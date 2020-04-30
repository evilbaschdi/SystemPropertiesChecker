using System.Linq;
using SystemPropertiesChecker.Core.Internal.DotNet;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using EvilBaschdi.Testing;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Xunit;

namespace SystemPropertiesChecker.Core.Tests.Internal.DotNet
{
    public class DotNetVersionReleaseKeyMappingListTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(DotNetVersionReleaseKeyMappingList).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(DotNetVersionReleaseKeyMappingList sut)
        {
            sut.Should().BeAssignableTo<IDotNetVersionReleaseKeyMappingList>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(DotNetVersionReleaseKeyMappingList).GetMethods().Where(method => !method.IsAbstract));
        }

        [Theory]
        [NSubstituteOmitAutoPropertiesTrueInlineAutoData("123", "unknown")]
        [NSubstituteOmitAutoPropertiesTrueInlineAutoData("378389", ".NET Framework 4.5")]
        [NSubstituteOmitAutoPropertiesTrueInlineAutoData("528049", ".Net Framework 4.8")]
        [NSubstituteOmitAutoPropertiesTrueInlineAutoData("528209", "unknown")]
        [NSubstituteOmitAutoPropertiesTrueInlineAutoData("600000", "vNext")]
        public void Value_ForProvidedJsonKey_ReturnsJsonValue(
            string dummySettingsKey,
            string dummySettingsValue,
            [Frozen] IDotNetVersionReleaseKeyMapping dotNetVersionReleaseKeyMapping,
            DotNetVersionReleaseKeyMappingList sut,
            IConfiguration dummyConfiguration
        )
        {
            // Arrange
            dummyConfiguration["378389"] = ".NET Framework 4.5";
            dummyConfiguration["528049"] = ".Net Framework 4.8";
            dummyConfiguration["600000"] = "vNext";
            dotNetVersionReleaseKeyMapping.Value.Returns(dummyConfiguration);

            // Act
            var result = sut.ValueFor(dummySettingsKey);

            // Assert
            result.Should().Be(dummySettingsValue);
        }
    }
}