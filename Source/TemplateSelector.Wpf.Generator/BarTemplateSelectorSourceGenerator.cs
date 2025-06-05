using Bars.Mvvm.FluidApi.Common;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Immutable;

namespace Bars.Mvvm.Resource.Generator;

/// <summary>
/// Source generator for generating ResourceKeys and mapping them to the DataTemplateSelector for less fuzz when creating new templates.
/// </summary>
[Generator(LanguageNames.CSharp)]
public class BarTemplateSelectorSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {
            // Generate marker attribute for the source generator
            ctx.AddSource($"{BarTemplateSelectorAttributeCodeGenerator.AttributeClassName}.g.cs",
                BarTemplateSelectorAttributeCodeGenerator.Instance.Generate());
        });

        var attributedClasses = context.SyntaxProvider
            .ForAttributeWithMetadataName(fullyQualifiedMetadataName: BarTemplateSelectorAttributeCodeGenerator.FullyQualifiedAttributeName,

                predicate: static (node, _) => true, // accept all — filtered later
                transform: static (ctx, ct) =>
                {
                    try
                    {
                        var classSymbol = ctx.TargetSymbol as INamedTypeSymbol;
                        if (classSymbol is null)
                        {
                            return default;
                        }

                        var attr = ctx.Attributes.FirstOrDefault();
                        if (attr is null || attr.ConstructorArguments.Length == 0)
                        {
                            return default;
                        }

                        // NOTE: validate and offer error diagnostics if the type is not a dictionary, is not partial
                        // NOTE: extend to provide the ResourceKeys class generation
                        var arg = attr.ConstructorArguments[0];
                        return arg.Value is not INamedTypeSymbol dictType
                            ? default((INamedTypeSymbol, INamedTypeSymbol))
                            : (classSymbol, dictType);

                    }
                    catch (Exception)
                    {
                        return default((INamedTypeSymbol, INamedTypeSymbol));
                    }
                })
            .Where(static pair => pair.Item1 is not null && pair.Item2 is not null)
            .Collect();

        context.RegisterSourceOutput(attributedClasses, static (ctx, items) =>
        {
            foreach (var (classSymbol, dictionarySymbol) in items!)
            {
                var dictName = dictionarySymbol.ToDisplayString();

                try
                {
                    string sourceCode = BarTemplateSelectorCodeGenerator.Instance.Generate(classSymbol, dictionarySymbol);

                    ctx.AddSource($"{classSymbol.Name}.g.cs", sourceCode);
                    ctx.ReportDiagnostic(Diagnostic.Create(
                        new DiagnosticDescriptor("GEN001", "Found Dictionary", $"Class {classSymbol.Name} references ResourceDictionary: {dictName}", "Generator", DiagnosticSeverity.Info, true),
                        classSymbol.Locations.FirstOrDefault()));
                }
                catch (Exception e)
                {
                    ctx.ReportDiagnostic(Diagnostic.Create(
                        new DiagnosticDescriptor("GEN001", "Error generating type", $"Target class {classSymbol.Name} : {e}", "Generator", DiagnosticSeverity.Error, true),
                        classSymbol.Locations.FirstOrDefault()));
                }
            }
        });
    }
}
