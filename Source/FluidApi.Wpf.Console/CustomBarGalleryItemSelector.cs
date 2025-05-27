#nullable disable
using System.Windows;
using ActiproSoftware.Windows.Controls.Bars.Mvvm;

namespace Bars.Mvvm.FluidGenerator.Console;

//[BarTemplateSelector]
public class CustomBarGalleryItemSelector : BarGalleryItemTemplateSelector
{
    public DataTemplate FontTemplate { get; set; }
    public DataTemplate FontMenuItemTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is FontItemViewModel)
        {
            return PrefersMenuItemAppearance(item, container)
                ? FontMenuItemTemplate
                : FontTemplate;
        }

        return base.SelectTemplate(item, container);
    }
}