using ActiproSoftware.Windows.Controls.Bars.Mvvm;
using MahApps.Metro.IconPacks;

namespace Bars.Mvvm.FluentApi.Generator.Sample;

public class SampleBarImageProvider : BarImageProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SampleBarImageProvider"/> class.
    /// </summary>
    public SampleBarImageProvider()
    {
        Register(SampleControlKeys.BackstageTabHome, options => options.CreateImage(PackIconMaterialKind.Home));
        Register(SampleControlKeys.BackstageTabNew, options => options.CreateImage(PackIconMaterialKind.NewBox));
        Register(SampleControlKeys.BackstageButtonPrint, options => options.CreateImage(PackIconMaterialKind.Printer));
        Register(SampleControlKeys.BackstageButtonClose, options => options.CreateImage(PackIconMaterialKind.Close));
        Register(SampleControlKeys.Cut, options => options.CreateImage(PackIconMaterialKind.ContentCut));
        Register(SampleControlKeys.Copy, options => options.CreateImage(PackIconMaterialKind.ContentCopy));
        Register(SampleControlKeys.Paste, options => options.CreateImage(PackIconMaterialKind.ContentPaste));
        Register(SampleControlKeys.FooterGroupSimple, options => options.CreateImage(PackIconMaterialKind.PageLayoutFooter));
        Register(SampleControlKeys.FooterGroupInfoBar, options => options.CreateImage(PackIconMaterialKind.PageLayoutHeaderFooter));
    }
}
