using SystemPropertiesChecker.Core.Internal.DotNet;

namespace SystemPropertiesChecker.Core.Tests.Internal.DotNet
{
    public class DotNetCoreListAsStringTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(DotNetCoreListAsString).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(DotNetCoreListAsString sut)
        {
            sut.Should().BeAssignableTo<IDotNetCoreListAsString>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(DotNetCoreListAsString).GetMethods().Where(method => !method.IsAbstract));
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Value_ForParameterList_ReturnsListAsString(
            DotNetCoreListAsString sut)
        {
            // Arrange

            // Act
            var result = sut.Value;

            // Assert
            result.Should().NotBeNullOrWhiteSpace();
        }
    }
}