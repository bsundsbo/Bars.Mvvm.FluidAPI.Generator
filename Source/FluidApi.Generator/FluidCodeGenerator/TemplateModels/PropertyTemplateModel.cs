using Microsoft.CodeAnalysis;

namespace Bars.Mvvm.FluidGenerator.Models;

public record PropertyTemplateModel
{
    public PropertyTemplateModel(INamedTypeSymbol classSymbol, IPropertySymbol propertySymbol)
    {
        PropertyName = propertySymbol.Name;
        PropertyType = propertySymbol.Type.ToString();
        ParameterName = char.ToLowerInvariant(this.PropertyName[0]) + this.PropertyName.Substring(1);
        ClassName = classSymbol.Name;
        NamespaceName = classSymbol.ContainingNamespace.ToDisplayString();
    }

    public object NamespaceName { get; init; }

    public object ClassName { get; init; }

    public string ParameterName { get; init; }

    public string PropertyType { get; init; }

    public string PropertyName { get; init; }

    public override string ToString()
    {
        return $"{nameof(NamespaceName)}: {NamespaceName}, {nameof(ClassName)}: {ClassName}, {nameof(ParameterName)}: {ParameterName}, {nameof(PropertyType)}: {PropertyType}, {nameof(PropertyName)}: {PropertyName}";
    }
}
