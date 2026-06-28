using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace SystemPropertiesChecker.Avalonia.Tests;

public class ViewLocatorTests
{
    [Fact]
    public void Constructor_ReturnsInterfaceName()
    {
        var sut = new ViewLocator();
        sut.Should().BeAssignableTo<IDataTemplate>();
    }

    [Fact]
    public void Build_WithNull_ReturnsTextBlock()
    {
        var sut = new ViewLocator();
        var result = sut.Build(null);
        result.Should().BeOfType<TextBlock>();
        ((TextBlock)result!).Text.Should().Be("View Not Found");
    }

    [Fact]
    public void Match_WithNull_ReturnsFalse()
    {
        var sut = new ViewLocator();
        var result = sut.Match(null);
        result.Should().BeFalse();
    }
}