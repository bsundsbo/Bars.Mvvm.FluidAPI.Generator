using Microsoft.CodeAnalysis;

namespace Bars.Mvvm.FluentApi.Generator.Models;

/// <summary>
/// This class represents a template model for a command with a parameter
/// to assign the command parameter with the command.
/// </summary>
public record CommandWithParameterTemplateModel : PropertyTemplateModel
{
    public CommandWithParameterTemplateModel(INamedTypeSymbol classSymbol,
        IPropertySymbol property,
        IPropertySymbol commandParameterProperty)
        : base(classSymbol, property)
    {
        ParameterPropertyName = commandParameterProperty.Name;
    }

    /// <summary>
    /// Property name of the command parameter that will be assigned in the extension method.
    /// </summary>
    public string? ParameterPropertyName { get; init; }
}
