using ActiproSoftware.Windows.Controls.Bars.Mvvm;
using System.Windows;
using System.Windows.Controls;

namespace Bars.Mvvm.TemplateSelector.Console;

/// <summary>
/// some doc to be filled in later
/// </summary>
[BarTemplateSelector(typeof(CustomDataTemplateSelectorResourceDictionary))]
public partial class CustomControlTemplateSelector : BarControlTemplateSelector
{
    public ItemContainerTemplate BlueButtonTemplate { get; set; }

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
