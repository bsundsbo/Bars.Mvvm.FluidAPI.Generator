using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Bars.Mvvm.FluidApi.Generator;

/// <summary>
/// Extension methods for <see cref="IncrementalGeneratorInitializationContext"/>.
/// </summary>
public static class IncrementalGeneratorInitializationContextExtensions
{
    /// <summary>
    /// Gets the <see cref="IncrementalValuesProvider{T}"/> of <see cref="MetadataReference"/> from
    /// the <see cref="IncrementalGeneratorInitializationContext"/>.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private static IncrementalValuesProvider<MetadataReference> GetMetadataReferencesProvider(this IncrementalGeneratorInitializationContext context)
    {
        var metadataProviderProperty = context.GetType().GetProperty(nameof(context.MetadataReferencesProvider))
                                       ?? throw new ArgumentException($"The property '{nameof(context.MetadataReferencesProvider)}' not found");

        var metadataProvider = metadataProviderProperty.GetValue(context);

        if (metadataProvider is IncrementalValuesProvider<MetadataReference> metadataValuesProvider)
        {
            return metadataValuesProvider;
        }

        if (metadataProvider is IncrementalValueProvider<MetadataReference> metadataValueProvider)
        {
            return metadataValueProvider.SelectMany(static (reference, _) => ImmutableArray.Create(reference));
        }

        throw new ArgumentException(
            $"The '{nameof(context.MetadataReferencesProvider)}' is neither an 'IncrementalValuesProvider<{nameof(MetadataReference)}>' nor an 'IncrementalValueProvider<{nameof(MetadataReference)}>.'");
    }

    /// <summary>
    /// Gets all metadata references that are Actipro assemblies.
    /// </summary>
    public static IncrementalValuesProvider<MetadataReference> GetActiproAssemblyReferences(this IncrementalGeneratorInitializationContext context)
    {
        return context
            .GetMetadataReferencesProvider()
            .Where(static reference => reference.IsActiproAssembly())
            .WithTrackingName("ActiproAssemblyFilter");
    }
}
