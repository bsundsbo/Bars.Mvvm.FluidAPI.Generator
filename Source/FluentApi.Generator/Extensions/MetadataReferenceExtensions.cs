using Bars.Mvvm.FluentApi.Common;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

// ReSharper disable once CheckNamespace
namespace Bars.Mvvm.FluentApi.Generator;

/// <summary>
/// Extensions methods for <see cref="MetadataReference"/>.
/// </summary>
public static class MetadataReferenceExtensions
{
    private const string _actiproMvvmWpfAssembly = "ActiproSoftware.Bars.Mvvm.Wpf.dll";
    private const string _actiproMvvmAvaloniaAssembly = "ActiproSoftware.Avalonia.Bars.Mvvm.dll";

    /// <summary>
    /// Check if the assembly is an Actipro assembly.
    /// </summary>
    /// <param name="reference">The reference to analyze.</param>
    /// <returns><c>true</c> if the assembly should be included; <c>false</c> otherwise.</returns>
    public static bool IsActiproAssembly(this MetadataReference reference)
    {
        // We are currently filtering WPF and Avalonia assemblies
        return reference.GetModules().Any(m => m.Name is _actiproMvvmWpfAssembly or _actiproMvvmAvaloniaAssembly);
    }

    /// <summary>
    /// Attempts to extract all top-level named types from a metadata reference.
    /// </summary>
    /// <param name="metadataReference">The reference to analyze.</param>
    /// <returns>List of named symbols from the reference.</returns>
    public static ImmutableArray<INamedTypeSymbol> GetAllTypesFromReference(this MetadataReference metadataReference)
    {
        if (metadataReference is not PortableExecutableReference)
        {
            return ImmutableArray<INamedTypeSymbol>.Empty;
        }

        return GetAllTypesFromMetadataImageReference(metadataReference);
    }

    /// <summary>
    /// Get the modules within the metadata reference.
    /// </summary>
    /// <param name="metadataReference"></param>
    /// <returns></returns>
    private static IEnumerable<ModuleInfo> GetModules(this MetadataReference metadataReference)
    {
        return metadataReference switch
        {
            // Project reference (ISymbol)
            CompilationReference compilationReference => compilationReference.Compilation.Assembly.Modules.Select(m =>
                new ModuleInfo(m.Name, compilationReference.Compilation.Assembly.Identity.Version)),

            // DLL
            PortableExecutableReference portable when portable.GetMetadata() is AssemblyMetadata assemblyMetadata =>
                assemblyMetadata.GetModules()
                    .Select(m => new ModuleInfo(m.Name, m.GetMetadataReader().GetAssemblyDefinition().Version)),
            _ => [],
        };
    }

    private static ImmutableArray<INamedTypeSymbol> GetAllTypesFromMetadataImageReference(MetadataReference metadataReference)
    {
        var compilation = CSharpCompilation.Create(
            assemblyName: "TempAssembly",
            syntaxTrees: [],
            references: [metadataReference]);

        if (compilation.GetAssemblyOrModuleSymbol(metadataReference) is not IAssemblySymbol assemblySymbol)
        {
            return ImmutableArray<INamedTypeSymbol>.Empty;
        }

        var types = GetAllTypesFromNamespace(assemblySymbol.GlobalNamespace);
        return types.Where(t => t.IsPublicNonAbstractNonGeneric())

            // ReSharper disable once UseCollectionExpression
            .ToImmutableArray();
    }

    private static IEnumerable<INamedTypeSymbol> GetAllTypesFromNamespace(INamespaceSymbol ns)
    {
        foreach (var type in ns.GetTypeMembers())
        {
            yield return type;

            foreach (var nested in GetAllNestedTypes(type))
            {
                yield return nested;
            }
        }

        foreach (var subNs in ns.GetNamespaceMembers())
        {
            foreach (var type in GetAllTypesFromNamespace(subNs))
            {
                yield return type;
            }
        }
    }

    private static IEnumerable<INamedTypeSymbol> GetAllNestedTypes(INamedTypeSymbol type)
    {
        foreach (var nested in type.GetTypeMembers())
        {
            yield return nested;

            foreach (var deeper in GetAllNestedTypes(nested))
            {
                yield return deeper;
            }
        }
    }
}
