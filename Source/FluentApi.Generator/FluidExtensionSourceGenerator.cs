using Bars.Mvvm.FluidApi.Common;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Bars.Mvvm.FluidApi.Generator;

/// <summary>
/// Code generator for generating Fluid API extensions for Actipro MVVM classes. These extensions are provide easy chaining to set properties,
/// but will be extended to support more complex scenarios for convenience methods and reduce other boilerplate code in MVVM applications.
/// The generator will support both WPF and Avalonia platforms, generating appropriate code based on the type of classes found in the referenced assemblies.
/// </summary>
[Generator(LanguageNames.CSharp)]
public class FluidExtensionSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var externalTypeProvider = context.GetActiproAssemblyReferences()
            .Collect()
            .WithTrackingName("ExternalTypeProvider");
        context.RegisterSourceOutput(externalTypeProvider, GenerateFromReferences);
    }

    private static void GenerateFromReferences(SourceProductionContext ctx, ImmutableArray<MetadataReference> references)
    {
        foreach (var reference in references)
        {
            if (ctx.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            try
            {

                var types = reference.GetAllTypesFromReference()
                    .OfType<INamedTypeSymbol>()
                    .Where(cls => cls != null
                                  && cls.IsPublicNonAbstractNonGeneric()
                                  && cls.IsMvvmNamespace()
                                  && cls.DerivesFromObservableObjectBase())
                    .ToList();
                var generatedFor = types.FirstOrDefault(m => m.ContainingNamespace.ToDisplayString() == ActiproTypeExtensions.AvaloniaNamespace) != null
                    ? TargetLibrary.Avalonia : TargetLibrary.Wpf;
                GenerateForClasses(ctx, types, generatedFor);
            }
            catch (Exception e)
            {
                ctx.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor(
                        "FLUID001",
                        "Error generating code",
                        "An error occurred while generating code for {0}: {1}",
                        "Code Generation",
                        DiagnosticSeverity.Error,
                        true),
                    Location.None,
                    reference.Display,
                    e.Message));
            }
        }
    }

    private static void GenerateForClasses(SourceProductionContext context, IList<INamedTypeSymbol> classes, TargetLibrary generatedFor)
    {
        if(classes.Count == 0)
        {
            return;
        }

        var parser = new FluidExtensionPropertyParser(generatedFor);
        var generator = new FluidExtensionCodeGenerator(parser);
        foreach (var classSymbol in classes)
        {
            try
            {
                if (context.CancellationToken.IsCancellationRequested)
                {
                    return;
                }

                var code = generator.Generate(classSymbol);
                if (!string.IsNullOrWhiteSpace(code))
                {
                    context.AddSource($"{classSymbol.Name}.FluidExtensions.g.cs", code);
                }
            }
            catch (Exception e)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor(
                        "FLUID001",
                        "Error generating code",
                        "An error occurred while generating code for {0}: {1}",
                        "Code Generation",
                        DiagnosticSeverity.Error,
                        true),
                    Location.None,
                    classSymbol.ToDisplayString(),
                    e.Message));
            }
        }
    }
}

public enum TargetLibrary
{
    Avalonia,
    Wpf
}
