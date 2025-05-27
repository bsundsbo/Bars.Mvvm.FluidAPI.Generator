using Bars.Mvvm.FluidApi.Common;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Immutable;

namespace Bars.Mvvm.Resource.Generator;

/// <summary>
/// Source generator for generating Fluid extensions for ActiproSoftware's Bars MVVM framework.
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

                    var arg = attr.ConstructorArguments[0];
                    var dictType = arg.Value as INamedTypeSymbol;
                    return dictType is null
                        ? default((INamedTypeSymbol, INamedTypeSymbol))
                        : (classSymbol, dictType);

                })
            .Where(static pair => pair.Item1 is not null && pair.Item2 is not null)
            .Collect();

        context.RegisterSourceOutput(attributedClasses, static (ctx, items) =>
        {
            foreach (var (classSymbol, dictionarySymbol) in items!)
            {
                var dictName = dictionarySymbol.ToDisplayString();

                string sourceCode = BarTemplateSelectorCodeGenerator.Instance.Generate(classSymbol, dictionarySymbol);

                ctx.AddSource($"{classSymbol.Name}.g.cs", sourceCode);
                ctx.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor("GEN001", "Found Dictionary", $"Class {classSymbol.Name} references ResourceDictionary: {dictName}", "Generator", DiagnosticSeverity.Info, true),
                    classSymbol.Locations.FirstOrDefault()));
            }
        });
    }
}
