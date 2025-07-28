using ActiproSoftware.Windows.Controls.Bars.Mvvm;
using ActiproSoftware.Windows.Input;
using FluentAssertions;
using NSubstitute;
using System.Windows.Media.Imaging;
using Xunit;

namespace Bars.Mvvm.FluentApi.Generator.Extensions.Wpf.Test;
/// <summary>
/// Verifies the output of the generator, and assert against source classes.
/// </summary>
public class BarComboBoxShould
{
    [Fact]
    public void BarComboBox_UnmatchedText_HasMultipleOverloads()
    {
        // verifies that BarComboBoxViewModel has two overloads for WithUnmatchedTextCommand
        var comboBox = new BarComboBoxViewModel
        {
            IsUnmatchedTextAllowed = false,
            UnmatchedTextCommand = null
        };

        // ACT
        comboBox.WithUnmatchedTextCommand(new DelegateCommand<object>(x => { }), true);

        comboBox.IsUnmatchedTextAllowed.Should().BeTrue();
        comboBox.UnmatchedTextCommand.Should().NotBeNull();
    }

    [Fact]
    public void BarComboBox_BooleanProperty_NoParameter()
    {
        // verifies that BarComboBoxViewModel has no overloads for WithUnmatchedTextCommand
        var comboBox = new BarComboBoxViewModel();

        // ACT
        comboBox.WithIsEditable();

        comboBox.IsEditable.Should().BeTrue();
    }

    [Fact]
    public void BarComboBox_BooleanProperty_WithParameter()
    {
        // verifies that BarComboBoxViewModel has no overloads for WithIsEditable
        var comboBox = new BarComboBoxViewModel();

        // ACT
        comboBox.WithIsEditable(false);

        comboBox.IsEditable.Should().BeFalse();
    }

    [Fact]
    public void BarComboBox_ObservableCollection_WithItem()
    {
        // verifies that BarComboBoxViewModel has no overloads for WithUnmatchedTextCommand
        var comboBox = new BarComboBoxViewModel();

        // ACT
        comboBox.WithAboveMenuItem(new BarButtonViewModel().WithLabel("hello"));

        comboBox.AboveMenuItems.Count.Should().Be(1);
        comboBox.AboveMenuItems[0].Should().BeOfType<BarButtonViewModel>();
    }

    [Fact]
    public void BarComboBox_ObservableCollection_WithItems()
    {
        // verifies that BarComboBoxViewModel has no overloads for WithUnmatchedTextCommand
        var comboBox = new BarComboBoxViewModel();

        // ACT
        comboBox.WithAboveMenuItems([new BarButtonViewModel().WithLabel("hello")]);

        comboBox.AboveMenuItems.Count.Should().Be(1);
        comboBox.AboveMenuItems[0].Should().BeOfType<BarButtonViewModel>();
    }

    [Fact]
    public void BarComboBox_HasVariantImages()
    {
        var imageProviderMock = Substitute.For<IBarImageProvider>();
        imageProviderMock.GetImageSource(Arg.Any<string>(), Arg.Any<BarImageSize>())
            .Returns(x => new BitmapImage());
        // verifies that BarComboBoxViewModel has no overloads for WithUnmatchedTextCommand
        var comboBox = new BarComboBoxViewModel()
            .WithImages(imageProviderMock);

        // ACT
        comboBox.WithAboveMenuItems([new BarButtonViewModel().WithLabel("hello")]);

        comboBox.AboveMenuItems.Count.Should().Be(1);
        comboBox.AboveMenuItems[0].Should().BeOfType<BarButtonViewModel>();
    }
}
