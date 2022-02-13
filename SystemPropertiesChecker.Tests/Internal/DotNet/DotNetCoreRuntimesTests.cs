using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using SystemPropertiesChecker.Core.Internal.DotNet;
using Xunit;

namespace SystemPropertiesChecker.Core.Tests.Internal.DotNet;

public class DotNetCoreRuntimesTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(DotNetCoreRunTimes).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsIDotNetCoreRuntimes(DotNetCoreRunTimes sut)
    {
        Assert.IsAssignableFrom<IDotNetCoreRunTimes>(sut);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(DotNetCoreRunTimes).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Value_ForParameterListRuntimes_ReturnsListOfInstalledRuntimes(
        DotNetCoreRunTimes sut)
    {
        // Arrange

        // Act
        var result = sut.Value;

        // Assert
        result.Should().NotBeNullOrWhiteSpace();
    }
}