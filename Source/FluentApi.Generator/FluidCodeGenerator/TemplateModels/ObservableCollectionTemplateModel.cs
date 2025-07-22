using Microsoft.CodeAnalysis;

namespace Bars.Mvvm.FluentApi.Generator.Models;

/// <summary>
/// Model used for generating ObservableCollection properties.
/// </summary>
public record ObservableCollectionTemplateModel : PropertyTemplateModel
{
    public ObservableCollectionTemplateModel(INamedTypeSymbol classSymbol,
        IPropertySymbol property,
        ITypeSymbol? typeArgument)
        : base(classSymbol, property)
    {
        TypeArgument = typeArgument;
        ParameterNameSingular = ParameterName.Substring(0, ParameterName.Length - 1);
        PropertyNameSingular = PropertyName.Substring(0, ParameterName.Length - 1);
    }

    /// <summary>
    /// Singular version of the property name.
    /// </summary>
    public string PropertyNameSingular { get; set; }

    /// <summary>
    /// Singular version of the parameter name.
    /// </summary>
    public string ParameterNameSingular { get; init; }

    /// <summary>
    /// The T of the ObservableCollection{T} collection.
    /// </summary>
    public ITypeSymbol? TypeArgument { get; set; }
}
