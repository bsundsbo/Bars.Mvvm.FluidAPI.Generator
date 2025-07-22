using ActiproSoftware.Windows.Controls;
using ActiproSoftware.Windows.Controls.Bars.Mvvm;
using FluentAssertions;
using Xunit;

namespace Bars.Mvvm.FluentApi.Generator.Wpf.Test;
/// <summary>
/// Verifies the output of the generator, and assert against source classes.
/// </summary>
public class RibbonViewModelShould
{
    [Fact]
    public void WithFooter_Simple()
    {
        // verifies that BarComboBoxViewModel has two overloads for WithUnmatchedTextCommand
        var ribbon = new RibbonViewModel()
        .WithFooter("text");

        // ACT
        ribbon.Footer.Should()
            .BeOfType<RibbonFooterViewModel>()
            .Which.Content
            .Should().BeOfType<RibbonFooterSimpleContentViewModel>()
            .Which.Text.Should().Be("text");
    }

    [Fact]
    public void WithFooter_Content()
    {
        // verifies that BarComboBoxViewModel has two overloads for WithUnmatchedTextCommand
        var ribbon = new RibbonViewModel()
            .WithFooter(new RibbonFooterInfoBarContentViewModel().WithSeverity(InfoBarSeverity.Error));

        // ACT
        ribbon.Footer.Should()
            .BeOfType<RibbonFooterViewModel>()
            .Which.Content
            .Should().BeOfType<RibbonFooterInfoBarContentViewModel>()
            .Which.Severity.Should().Be(InfoBarSeverity.Error);
    }

    [Fact]
    public void WithFooter_InfoBar()
    {
        // verifies that BarComboBoxViewModel has two overloads for WithUnmatchedTextCommand
        var ribbon = new RibbonViewModel()
            .WithFooter("Title", "Message", true, iconSource: null, InfoBarSeverity.Warning);

        // ACT
        ribbon.Footer.Should()
            .BeOfType<RibbonFooterViewModel>()
            .Which.Content
            .Should().BeOfType<RibbonFooterInfoBarContentViewModel>()
            .Which.Severity.Should().Be(InfoBarSeverity.Warning);

        ribbon.Footer.Should()
            .BeOfType<RibbonFooterViewModel>()
            .Which.Content
            .Should().BeOfType<RibbonFooterInfoBarContentViewModel>()
            .Which.Title.Should().Be("Title");

        ribbon.Footer.Should()
            .BeOfType<RibbonFooterViewModel>()
            .Which.Content
            .Should().BeOfType<RibbonFooterInfoBarContentViewModel>()
            .Which.CanClose.Should().BeTrue();

        ribbon.Footer.Should()
            .BeOfType<RibbonFooterViewModel>()
            .Which.Content
            .Should().BeOfType<RibbonFooterInfoBarContentViewModel>()
            .Which.Message.Should().Be("Message");
    }
}
