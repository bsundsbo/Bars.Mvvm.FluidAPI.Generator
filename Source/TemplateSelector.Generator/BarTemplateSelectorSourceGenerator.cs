using Bars.Mvvm.FluidApi.Common;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
                BarTemplateSelectorAttributeCodeGenerator.Generate());
        });

        IncrementalValuesProvider<ClassModel> provider = context.SyntaxProvider
            .ForAttributeWithMetadataName(fullyQualifiedMetadataName: BarTemplateSelectorAttributeCodeGenerator.FullyQualifiedAttributeName,
                predicate: static (node, cancellationToken_) => node is ClassDeclarationSyntax,
                transform: static (ctx, cancellationToken) =>
                {
                    ISymbol classSymbol = ctx.TargetSymbol;

                    return new ClassModel(classSymbol.Name,
                        classSymbol.ContainingNamespace.ToDisplayString(),
                        GetProperties(classSymbol as INamedTypeSymbol));
                });

        context.RegisterSourceOutput(provider, static (context, classModel) =>
        {
            string sourceCode = BarTemplateSelectorCodeGenerator.Generate(classModel);

            //Concat class name and interface name to have unique file name if a class implements two interfaces with AutoImplement Attribute
            string generatedFileName = $"{classModel.Name}.g.cs";
            context.AddSource(generatedFileName, sourceCode);
        });
    }

    private static EquatableList<IPropertySymbol> GetProperties(INamedTypeSymbol? classSymbol)
    {
        EquatableList<IPropertySymbol> ret = [];
        if (classSymbol == null)
        {
            return ret;
        }

        var properties = classSymbol.GetMembers()
            .OfType<IPropertySymbol>()
            .Where(p => p.DeclaredAccessibility == Accessibility.Public && p is {IsStatic: false, IsReadOnly: false})
            .ToList();

        ret.AddRange(properties);
        return ret;
    }
}
