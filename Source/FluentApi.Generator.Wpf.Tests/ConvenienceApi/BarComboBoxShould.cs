using ActiproSoftware.Windows.Controls.Bars.Mvvm;
using ActiproSoftware.Windows.Input;
using Bars.Mvvm.FluidApi.Common;
using FluentAssertions;
using System.Reflection;
using Xunit;

namespace Bars.Mvvm.FluidApi.Generator.Wpf.Test;
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
}
