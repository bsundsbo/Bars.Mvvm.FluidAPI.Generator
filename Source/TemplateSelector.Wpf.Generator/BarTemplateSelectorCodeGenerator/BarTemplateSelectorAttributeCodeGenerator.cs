using Bars.Mvvm.FluidApi.Common;
using System.Text;
using Scriban;
using System.Diagnostics.CodeAnalysis;

namespace Bars.Mvvm.Resource.Generator;

/// <summary>
/// This generator will generate the BarTemplateSelectorAttribute to decorate DataTemplateSelectors.
/// </summary>
internal class BarTemplateSelectorAttributeCodeGenerator
{
    private const string _attributeNameSpace = "ActiproSoftware.Windows.Controls.Bars.Mvvm";
    private const string _attributeName = "BarTemplateSelector";
    public const string AttributeClassName = $"{_attributeName}Attribute";
    public const string FullyQualifiedAttributeName = $"{_attributeNameSpace}.{AttributeClassName}";

    public static BarTemplateSelectorAttributeCodeGenerator Instance { get; } = new();
    [SuppressMessage("SonarLint", "S2325:Naming Styles", Justification = "Don't want to make static")]
    public string Generate()
    {
        var template = Template.Parse(EmbeddedResource.GetContent("Templates/BarTemplateSelectorAttribute.fg-cs"));
        StringBuilder attrBuilder = new();
        attrBuilder.Append(template.Render());
        return attrBuilder.ToString();
    }
}
