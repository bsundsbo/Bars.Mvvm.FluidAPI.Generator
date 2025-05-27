using Microsoft.CodeAnalysis;

namespace Bars.Mvvm.FluidGenerator.Models;

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

    public string? ParameterPropertyName { get; init; }
}
