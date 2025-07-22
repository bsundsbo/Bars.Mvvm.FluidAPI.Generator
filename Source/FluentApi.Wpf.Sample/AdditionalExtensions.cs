using ActiproSoftware.Windows.Controls.Bars.Mvvm;
using MahApps.Metro.IconPacks;
using System.Windows.Media;

namespace Bars.Mvvm.FluentApi.Generator.Sample;

public static class AdditionalExtensions
{
    /// <summary>
    /// Extension method to support WithImages for types that have been subclassed to support IHasVariantImages
    /// where not yet supported by the original library.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="imageProvider"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T WithImages<T>(this T target, IBarImageProvider imageProvider)
        where T : BarKeyedObjectViewModelBase, IHasVariantImages
    {
        target.LargeImageSource = imageProvider.GetImageSource(target.Key, BarImageSize.Large);
        target.MediumImageSource = imageProvider.GetImageSource(target.Key, BarImageSize.Medium);
        target.SmallImageSource = imageProvider.GetImageSource(target.Key, BarImageSize.Small);
        return target;
    }

    /// <summary>
    /// Te
    /// </summary>
    /// <param name="_"></param>
    /// <param name="kind"></param>
    /// <returns></returns>
    public static ImageSource? CreateImage(this BarImageOptions _, PackIconMaterialKind kind)
    {
        return new MaterialImageExtension(kind)
            // ReSharper disable once NullableWarningSuppressionIsUsed
            .ProvideValue(null!) as ImageSource;
    }
}
