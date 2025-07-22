using ActiproSoftware.Windows.Controls.Bars.Mvvm;
using System.Windows.Media;

namespace Bars.Mvvm.FluentApi.Generator.Sample;

/// <summary>
/// Temporary class until the MVVM package supports IHasVariantImages for <see cref="RibbonBackstageTabViewModel"/> and gives
/// the ability to set images for this type through extension method.
/// </summary>
/// <param name="key"></param>
public class ImageRibbonBackstageTabViewModel(string key) : RibbonBackstageTabViewModel(key), IHasVariantImages
{
    ImageSource IHasVariantImages.MediumImageSource
    {
        get => null!;
        set { /* ignore */}
    }
}