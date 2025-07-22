using Bars.Mvvm.FluidApi.Common;
using System.Collections.Generic;
using System.Linq;
using Bars.Mvvm.FluidGenerator.Models;
using Microsoft.CodeAnalysis;

namespace Bars.Mvvm.FluidApi.Generator;

/// <summary>
/// Helper class for processing properties to generate code for.
/// </summary>
internal class FluidExtensionPropertyParser(TargetLibrary targetLibrary)
{
    public TargetLibrary TargetLibrary { get; } = targetLibrary;
    private const string _commandParameterSuffix = "CommandParameter";
    private const string _commandSuffix = "Command";
    private const string _observableCollectionTypeName = "ObservableCollection";
    private const string _objectModelNamespaceName = "System.Collections.ObjectModel";

    /// <summary>
    /// Get template models for all public properties of a class.
    /// </summary>
    /// <param name="classSymbol"></param>
    /// <returns></returns>
    public List<IExtensionTemplateModel> GetPropertyTemplateModels(INamedTypeSymbol? classSymbol)
    {
        if (classSymbol == null)
        {
            return [];
        }

        var properties = GetPublicProperties(classSymbol);

        List<IPropertySymbol> commandParameterProperties = [];
        commandParameterProperties.AddRange(properties.FindAll(p => p.Name.EndsWith(_commandParameterSuffix)));

        var propertyModels = new List<IExtensionTemplateModel>();
        foreach (var property in properties)
        {
            var propertyModel = GetPropertyModel(classSymbol, property, commandParameterProperties);
            if (propertyModel != null)
            {
                propertyModels.Add(propertyModel);
            }
        }

        if (TargetLibrary == TargetLibrary.Wpf
            && classSymbol.AllInterfaces.Any(i => i.Name == "IHasVariantImages"))
        {
            var model = new HasVariantImageTemplateModel(classSymbol.Name);
            propertyModels.Add(model);
        }

        return propertyModels.Distinct().ToList();
    }

    private static EquatableList<IPropertySymbol> GetPublicProperties(INamedTypeSymbol classSymbol)
    {
        var properties = new EquatableList<IPropertySymbol>();
        var currentType = classSymbol;
        while (currentType != null)
        {
            properties.AddRange(currentType.GetMembers()
                .OfType<IPropertySymbol>()
                .Where(p => p.DeclaredAccessibility == Accessibility.Public && !p.IsStatic));

            currentType = currentType.BaseType; // Move to the base type
        }

        return properties;
    }

    private static PropertyTemplateModel? GetPropertyModel(INamedTypeSymbol classSymbol,
        IPropertySymbol property,
        List<IPropertySymbol> commandParameterProperties)
    {
        if (property.Type.ToDisplayString().EndsWith(_commandSuffix))
        {
            var commandParameterProperty = commandParameterProperties.Find(p => p.Name.StartsWith(property.Name));
            commandParameterProperties.Remove(commandParameterProperty);
            if (commandParameterProperty != null)
            {
                // Property is ICommand that has a corresponding CommandParameter property
                return new CommandWithParameterTemplateModel(classSymbol, property, commandParameterProperty);
            }
        }

        if (property is {GetMethod: not null, SetMethod: not null})
        {
            return property.Type.SpecialType == SpecialType.System_Boolean
                   || property.Type.ToDisplayString() is "bool?" or "System.Nullable<bool>"
                ? new BooleanPropertyTemplateModel(classSymbol, property)
                : new PropertyTemplateModel(classSymbol, property);
        }

        if (property.IsReadOnly && IsObservableCollection(property, out ITypeSymbol? elementType))
        {
            return new ObservableCollectionTemplateModel(classSymbol, property, elementType);
        }

        return null;
    }

    private static bool IsObservableCollection(IPropertySymbol? propertySymbol, out ITypeSymbol? typeArgument)
    {
        var typeSymbol = propertySymbol?.Type as INamedTypeSymbol;
        if (typeSymbol?.Name == _observableCollectionTypeName
            && (typeSymbol.ContainingNamespace.ToDisplayString() == _objectModelNamespaceName || typeSymbol.ContainingNamespace.ToDisplayString() == "<global namespace>"))
        {
            typeArgument = typeSymbol.TypeArguments.First();
            return true;
        }

        typeArgument = null;
        return false;
    }
}
