using ActiproSoftware.Windows.Controls.Bars.Mvvm;
using System.Windows.Media;

namespace Bars.Mvvm.FluidGenerator.Sample;

public static class AdditionExtensionMethods
{
    /// <summary>
    /// Add a simple footer with text and an optional image source to the target <see cref="RibbonViewModel"/>.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="text"></param>
    /// <param name="imageSource"></param>
    /// <returns></returns>
    public static RibbonViewModel WithFooter(this RibbonViewModel target, string text, ImageSource? imageSource = null)
    {
        return target.WithFooter(
            new RibbonFooterViewModel()
                .WithContent(new RibbonFooterSimpleContentViewModel()
                    .WithText(text)
                    .WithImageSource(imageSource)));
    }
}
