using Microsoft.CodeAnalysis;
using System.Linq;

namespace Bars.Mvvm.FluentApi.Generator.Extensions.Models;

/// <summary>
/// Base model for specific property template models. All these properties are used in various templates when generating code.
/// </summary>
internal record PropertyTemplateModel : IExtensionPropertyTemplateModel
{
    public PropertyTemplateModel(INamedTypeSymbol classSymbol, IPropertySymbol propertySymbol)
    {
        PropertyName = propertySymbol.Name;
        PropertyType = propertySymbol.Type.ToString();
        ParameterName = char.ToLowerInvariant(this.PropertyName[0]) + this.PropertyName.Substring(1);
        ClassName = classSymbol.Name;
        NamespaceName = classSymbol.ContainingNamespace.ToDisplayString();
        var obsoleteAttribute = propertySymbol.GetAttributes()
            .FirstOrDefault(a => a.AttributeClass?.ToDisplayString() == "System.ObsoleteAttribute");
        IsObsolete = obsoleteAttribute != null;
    }

    /// <inheritdoc />
    public bool IsObsolete { get; }

    /// <summary>
    /// Namespace name of the class that contains the class where the property is declared.
    /// </summary>
    public string NamespaceName { get; }

    /// <summary>
    /// Class declaring the property.
    /// </summary>
    public string ClassName { get; }

    /// <summary>
    /// Formatted parameter name for the property, which is <see cref="PropertyName"/> in camel case.
    /// </summary>
    public string ParameterName { get; }

    /// <summary>
    /// Type of the property.
    /// </summary>
    public string PropertyType { get; }

    /// <summary>
    /// Name of the property generating extension method for.
    /// </summary>
    public string PropertyName { get; }

    public override string ToString()
    {
        return $"{nameof(NamespaceName)}: {NamespaceName}, {nameof(ClassName)}: {ClassName}, {nameof(ParameterName)}: {ParameterName}, {nameof(PropertyType)}: {PropertyType}, {nameof(PropertyName)}: {PropertyName} IsObsolete: {IsObsolete}";
    }
}