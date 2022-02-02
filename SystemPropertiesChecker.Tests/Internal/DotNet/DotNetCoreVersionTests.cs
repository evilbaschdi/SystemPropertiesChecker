using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using SystemPropertiesChecker.Core.Internal.DotNet;
using Xunit;

namespace SystemPropertiesChecker.Core.Tests.Internal.DotNet;

public class DotNetCoreVersionTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(DotNetCoreVersion).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsIDotNetCoreVersion(DotNetCoreVersion sut)
    {
        sut.Should().BeAssignableTo<IDotNetCoreVersion>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(DotNetCoreVersion).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
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