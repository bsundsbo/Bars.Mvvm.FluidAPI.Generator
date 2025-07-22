using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bars.Mvvm.FluidGenerator.Tests.FluidCodeGenerator;

public static class SymbolFactory
{
    public static INamedTypeSymbol? CreateClassSymbol(string sourceCode, string className)
    {
        // Parse the source code into a syntax tree
        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

        // Create a compilation
        var compilation = CSharpCompilation.Create(
            "TestAssembly",
            [syntaxTree],
            [MetadataReference.CreateFromFile(typeof(object).Assembly.Location)],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        // Get the class symbol
        var classSymbol = compilation.GetTypeByMetadataName(className);
        return classSymbol;
    }
}
