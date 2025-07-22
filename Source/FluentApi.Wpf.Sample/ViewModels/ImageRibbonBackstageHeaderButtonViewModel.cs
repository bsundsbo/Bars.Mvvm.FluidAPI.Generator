using ActiproSoftware.Windows.Controls.Bars.Mvvm;
using System.Windows.Media;

namespace Bars.Mvvm.FluidGenerator.Sample;

/// <summary>
/// Temporary class until the MVVM package supports IHasVariantImages for backstage header buttons and gives
/// the ability to set images for backstage header buttons through extension method.
/// </summary>
/// <param name="key"></param>
public class ImageRibbonBackstageHeaderButtonViewModel(string key) : RibbonBackstageHeaderButtonViewModel(key), IHasVariantImages
{
    ImageSource IHasVariantImages.MediumImageSource
    {
        get => null!;
        set { /* ignore */}
    }

    ImageSource IHasVariantImages.LargeImageSource
    {
        get => null!;
        set { /* ignore */}
    }
}
