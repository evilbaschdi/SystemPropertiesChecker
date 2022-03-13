using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using EvilBaschdi.Core;
using EvilBaschdi.Testing;
using FluentAssertions;
using SystemPropertiesChecker.Core.Internal;
using Xunit;

namespace SystemPropertiesChecker.Terminal.Tests
{
    public class ExecuteTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(Execute).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(Execute sut)
        {
            sut.Should().BeAssignableTo<IRun>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(Execute).GetMethods().Where(method => !method.IsAbstract));
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Run_ForProvidedConfigureCoreServices_BuildsConsoleOutput(
            [Frozen] IConfigureCoreServices configureCoreServices,
            Execute sut
        )
        {
            // Arrange

            // Act
            sut.Run();

            // Assert
        }
    }
}