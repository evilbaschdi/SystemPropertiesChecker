using SystemPropertiesChecker.Core.Internal.DotNet;

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
        Assert.IsAssignableFrom<DotNetCoreListAsString>(sut);
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