using ActiproSoftware.Windows.Controls.Bars;
using ActiproSoftware.Windows.Controls.Bars.Mvvm;
using FluentAssertions;
using Xunit;

namespace Bars.Mvvm.FluidApi.Generator.Wpf.Test;
/// <summary>
/// Verifies the output of the generator, and assert against source classes.
/// </summary>
public class RibbonBackstageViewModelShould
{
    [Fact]
    public void WithItemSeparator_NoParameter_Alignment()
    {
        var backstage = new RibbonBackstageViewModel()
            .WithItem(new RibbonBackstageHeaderButtonViewModel())
            .WithItemSeparator();

        // ASSERT
        backstage.Items.Count.Should().Be(2);
        backstage.Items[0].Should().BeOfType<RibbonBackstageHeaderButtonViewModel>();
        backstage.Items[1].Should().BeOfType<RibbonBackstageHeaderSeparatorViewModel>();
        var separator = (RibbonBackstageHeaderSeparatorViewModel)backstage.Items[1];
        separator.HeaderAlignment.Should().Be(RibbonBackstageHeaderAlignment.Top);
    }

    [Fact]
    public void WithItemSeparator_Alignment()
    {
        var backstage = new RibbonBackstageViewModel()
            .WithItem(new RibbonBackstageHeaderButtonViewModel())
            .WithItemSeparator(RibbonBackstageHeaderAlignment.Bottom);

        // ASSERT
        backstage.Items.Count.Should().Be(2);
        backstage.Items[0].Should().BeOfType<RibbonBackstageHeaderButtonViewModel>();
        backstage.Items[1].Should().BeOfType<RibbonBackstageHeaderSeparatorViewModel>();
        var separator = (RibbonBackstageHeaderSeparatorViewModel)backstage.Items[1];
        separator.HeaderAlignment.Should().Be(RibbonBackstageHeaderAlignment.Bottom);
    }
}
