using Microsoft.CodeAnalysis;

namespace Bars.Mvvm.FluidGenerator.Models;

/// <summary>
/// Base model for specific property template models. All these properties are used in various templates when generating code.
/// </summary>
public record PropertyTemplateModel : IExtensionTemplateModel
{
    public PropertyTemplateModel(INamedTypeSymbol classSymbol, IPropertySymbol propertySymbol)
    {
        PropertyName = propertySymbol.Name;
        PropertyType = propertySymbol.Type.ToString();
        ParameterName = char.ToLowerInvariant(this.PropertyName[0]) + this.PropertyName.Substring(1);
        ClassName = classSymbol.Name;
        NamespaceName = classSymbol.ContainingNamespace.ToDisplayString();
    }

    /// <summary>
    /// Namespace name of the class that contains the class where the property is declared.
    /// </summary>
    public object NamespaceName { get; init; }

    /// <summary>
    /// Class declaring the property.
    /// </summary>
    public object ClassName { get; init; }

    /// <summary>
    /// Formatted parameter name for the property, which is <see cref="PropertyName"/> in camel case.
    /// </summary>
    public string ParameterName { get; init; }

    /// <summary>
    /// Type of the property.
    /// </summary>
    public string PropertyType { get; init; }

    /// <summary>
    /// Name of the property generating extension method for.
    /// </summary>
    public string PropertyName { get; init; }

    public override string ToString()
    {
        return $"{nameof(NamespaceName)}: {NamespaceName}, {nameof(ClassName)}: {ClassName}, {nameof(ParameterName)}: {ParameterName}, {nameof(PropertyType)}: {PropertyType}, {nameof(PropertyName)}: {PropertyName}";
    }
}