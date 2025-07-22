using Microsoft.CodeAnalysis;

namespace Bars.Mvvm.FluentApi.Generator.Models;

/// <summary>
/// This class represents the backing properties for rendering proper values to
/// the template for boolean properties where the value defaults to true.
/// </summary>
public record BooleanPropertyTemplateModel : PropertyTemplateModel
{
    public BooleanPropertyTemplateModel(INamedTypeSymbol classSymbol,
        IPropertySymbol propertySymbol)
        : base(classSymbol, propertySymbol)
    {
    }
}
