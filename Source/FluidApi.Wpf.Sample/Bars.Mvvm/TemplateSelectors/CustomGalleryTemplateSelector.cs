using ActiproSoftware.Windows.Controls.Bars.Mvvm;
using System.Windows;

namespace Bars.Mvvm.FluidGenerator.Sample;

/// <summary>
/// This class has to be partial, and decorated with <see cref="BarTemplateSelectorAttribute"/>,
/// The type passed to the constructor of the attribute must be a resource dictionary with x:Class referencing this class in
/// the XAML.
///
/// <para>
/// The properties of <see cref="CustomDataTemplateSelectorResourceKeys"/> are gnnerated by the Source Generator package
/// <c>Bars.Mvvm.TemplateSelector.SourceGenerator</c>.
/// </para>
/// </summary>
[BarTemplateSelector(typeof(CustomGalleryTemplateSelectorResourceDictionary))]
public partial class CustomGalleryTemplateSelector : BarGalleryItemTemplateSelector
{
    public DataTemplate ReferenceCodeTemplate { get; set; }
    public DataTemplate ReferenceCodeMenuItemTemplate { get; set; }

    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        bool isMenuItem = PrefersMenuItemAppearance(item, container);
        if (item is ReferenceCodeGalleryItemViewModel)
        {
            return isMenuItem ? ReferenceCodeMenuItemTemplate : ReferenceCodeTemplate;
        }

        return base.SelectTemplate(item, container);
    }
}
