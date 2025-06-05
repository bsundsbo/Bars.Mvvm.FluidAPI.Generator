using ActiproSoftware.Windows.Controls.Bars.Mvvm;
using System.Windows;
using System.Windows.Controls;

namespace Bars.Mvvm.FluidGenerator.Sample;

/// <summary>
/// This class provides a custom data template selector using generated template mapping to showcase the
/// <see cref="BarTemplateSelectorAttribute"/>.
/// </summary>
[BarTemplateSelector(typeof(CustomDataTemplateSelectorResourceDictionary))]
public partial class CustomDataTemplateSelector : BarControlTemplateSelector
{
    /// <summary>
    /// The template used for a blue button.
    /// </summary>
    public ItemContainerTemplate BlueButtonTemplate { get; set; }

    /// <summary>
    /// The template used for a red button.
    /// </summary>
    public ItemContainerTemplate RedButtonTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, ItemsControl parentItemsControl)
    {
        return item switch
        {
            RedBarButtonViewModel => RedButtonTemplate,
            BlueBarButtonViewModel => BlueButtonTemplate,
            _ => base.SelectTemplate(item, parentItemsControl),
        };

    }
}
