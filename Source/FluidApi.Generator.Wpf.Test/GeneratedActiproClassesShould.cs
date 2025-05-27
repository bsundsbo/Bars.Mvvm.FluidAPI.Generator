using ActiproSoftware.Windows.Controls.Bars.Mvvm;
using Bars.Mvvm.FluidApi.Common;
using FluentAssertions;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace Bars.Mvvm.FluidApi.Generator.Wpf.Test;

/// <summary>
/// Verifies the output of the generator, and assert against source classes.
/// </summary>
public class GeneratedActiproClassesShould
{
    [Theory]
    [InlineData(typeof(BarButtonViewModel), ActiproTypeExtensions.WpfNamespace)]
    public void Have_Extension_Class_For_Each_Source_Type(Type sourceType, string sourceNamespace)
    {
        var generatedTypes = this.GetType()
            .GetExtensionClassesFromTypeAssembly(sourceNamespace);
        var missingTypes = GetSourceTypes(sourceType)
            .Where(st => !generatedTypes.Exists(generatedType => generatedType.Name == $"{st.Name}Extensions"))
            .ToList();

        if (missingTypes.Count > 0)
        {
            Assert.Fail("Missing extension classes for: " + string.Join(Environment.NewLine, missingTypes.Select(t => t.Name)));
        }
    }

    [Theory]
    [InlineData(typeof(BarButtonViewModel), ActiproTypeExtensions.WpfNamespace)]
    public void Have_All_ReadWrite_Properties(Type sourceViewModel, string sourceNamespace)
    {
        var generatedTypes = this.GetType()
            .GetExtensionClassesFromTypeAssembly(sourceNamespace);
        var validationResults = new ValidationResult("Classes with missing read/write properties");
        foreach (var sourceType in GetSourceTypes(sourceViewModel))
        {
            var generatedMethods = generatedTypes.GetGeneratedMethodsForType(sourceType);
            if (generatedMethods.Count == 0)
            {
                // Skip as missing generated classes are tested in another test
                continue;
            }

            // Get all public read/write properties
            var readWriteProperties = sourceType.GetReadWriteProperties();
            foreach (var sourceProperty in readWriteProperties)
            {
                if (generatedMethods.TryGetValue($"With{sourceProperty.Name}", out var method))
                {
                    if (sourceProperty.PropertyType != typeof(bool) && sourceProperty.PropertyType != typeof(bool?))
                    {
                        continue;
                    }

                    var lastParameter = method.GetParameters().LastOrDefault();
                    if (lastParameter?.IsOptionalParameter<bool>() == false)
                    {
                        validationResults.AddError(sourceType.Name, sourceProperty.Name, $"{method.Name} is expected to have optional bool parameter as last parameter.");
                    }

                    // If the property is found, it is valid
                    continue;
                }

                // no generated method found for this property
                validationResults.AddError(sourceType.Name, sourceProperty.Name);
            }
        }

        // ASSERT
        validationResults.HasValidationErrors.Should().BeFalse(validationResults.GetErrorMessage());
    }

    [Theory]
    [InlineData(typeof(BarButtonViewModel), ActiproTypeExtensions.WpfNamespace)]
    public void HasVariantImages_Implements_WithImages(Type sourceViewModel, string sourceNamespace)
    {
        var generatedTypes = this.GetType()
            .GetExtensionClassesFromTypeAssembly(sourceNamespace);
        var validationResults = new ValidationResult("Classes with missing read/write properties");
        foreach (var sourceType in from sourceType in GetSourceTypes(sourceViewModel) where sourceType.Implements(typeof(IHasVariantImages)) let generatedMethods = generatedTypes.GetGeneratedMethodsForType(sourceType) where !generatedMethods.TryGetValue("WithImages", out _) select sourceType)
        {
            // no generated method found for this property
            validationResults.AddError(sourceType.Name, "WithImages");
        }

        // ASSERT
        validationResults.HasValidationErrors.Should().BeFalse(validationResults.GetErrorMessage());
    }

    [Theory]
    [InlineData(typeof(BarButtonViewModel), ActiproTypeExtensions.WpfNamespace)]
    public void Have_Singular_And_Plural_For_ObservableCollection(Type sourceViewModel, string sourceNamespace)
    {
        var generatedTypes = this.GetType()
            .GetExtensionClassesFromTypeAssembly(sourceNamespace);
        var validation = new ValidationResult("Classes with missing WithItem\\Items pair with proper signature");

        foreach (var sourceType in GetSourceTypes(sourceViewModel))
        {
            var generatedMethods = generatedTypes.GetGeneratedMethodsForType(sourceType);
            if (generatedMethods.Count == 0)
            {
                // Skip as missing generated classes are tested in another test
                continue;
            }

            // Get all public read/write properties
            var readOnlyObservableProperties = sourceType.GetReadOnlyPropertiesOfObservableCollection();
            foreach (var sourceProperty in readOnlyObservableProperties)
            {
                var collectionTypeParameter = sourceProperty.PropertyType.GenericTypeArguments
                    .FirstOrDefault();
                // Analyze with plural method to add IEnumerable<T>
                generatedMethods.TryGetValue($"With{sourceProperty.Name}", out var pluralMethod);
                AnalyzePluralCollectionProperty(pluralMethod, sourceProperty.Name, validation, sourceType.Name, collectionTypeParameter);

                var singularName = sourceProperty.Name.Substring(0, sourceProperty.Name.Length - 1);
                generatedMethods.TryGetValue($"With{singularName}", out var singularMethod);
                AnalyzeSingularCollectionProperty(singularMethod, sourceType.Name, sourceProperty.Name, validation, collectionTypeParameter);
            }
        }

        // ASSERT
        validation.HasValidationErrors.Should()
            .BeFalse(validation.GetErrorMessage());
    }

    private static void AnalyzeSingularCollectionProperty(MethodInfo? singularMethod, string sourceTypeName, string sourcePropertyName, ValidationResult validation, Type? collectionTypeParameter)
    {
        if (singularMethod == null)
        {
            validation.AddError(sourceTypeName, sourcePropertyName, $"Missing singular method With{sourcePropertyName}");
            return;
        }

        var pluralParameters = singularMethod.GetParameters();
        if (pluralParameters.Length != 2)
        {
            validation.AddError(sourceTypeName, sourcePropertyName, $"{singularMethod.Name} missing 2 parameters");
            return;
        }

        if(pluralParameters[1].ParameterType != collectionTypeParameter)
        {
            validation.AddError(sourceTypeName, sourcePropertyName, $"{singularMethod.Name} second parameter is not IEnumerable<{collectionTypeParameter}>");
        }
    }

    private static void AnalyzePluralCollectionProperty(MethodInfo? pluralMethod, string sourcePropertyName, ValidationResult validation, string sourceTypeName, Type? collectionTypeParameter)
    {
        if (pluralMethod == null)
        {
            validation.AddError(sourceTypeName, sourcePropertyName, $"Missing plural method With{sourcePropertyName}");
            return;
        }

        var pluralParameters = pluralMethod.GetParameters();
        if (pluralParameters.Length != 2)
        {
            validation.AddError(sourceTypeName, sourcePropertyName, $"With{sourcePropertyName} missing 2 parameters");
            return;
        }

        var genericTypeArgument = pluralParameters[1].ParameterType.GenericTypeArguments
            .FirstOrDefault();
        if(pluralParameters[1].ParameterType.GetGenericTypeDefinition() != typeof(IEnumerable<>)
           || genericTypeArgument != collectionTypeParameter)
        {
            validation.AddError(sourceTypeName, sourcePropertyName, $"With{sourcePropertyName} second parameter is not IEnumerable<{collectionTypeParameter}>");
        }
    }

    private static bool IsValidClass(Type type)
    {
        return type is {IsClass: true, IsAbstract: false, IsGenericType: false, Namespace: ActiproTypeExtensions.WpfNamespace} && type.GetInterface("INotifyPropertyChanged") != null;
    }

    private static List<Type> GetSourceTypes(Type type)
    {
        return type
            .Assembly
            .GetTypes()
            .Where(IsValidClass)
            .ToList();
    }
}
