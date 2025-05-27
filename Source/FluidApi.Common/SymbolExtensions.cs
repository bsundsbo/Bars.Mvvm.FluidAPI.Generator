using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Bars.Mvvm.FluidApi.Common;

public static class SymbolExtensions
{
    public static bool HasBaseTypeInNamespace(this INamedTypeSymbol? type, string? targetNamespace)
    {
        if (type is null || targetNamespace == null)
        {
            return false;
        }

        var baseType = type.BaseType;

        while (baseType is not null)
        {
            if (baseType.ContainingNamespace?.ToDisplayString() == targetNamespace)
            {
                return true;
            }

            baseType = baseType.BaseType;
        }

        return false;
    }

    public static bool HasBaseTypeInNamespace(this INamedTypeSymbol? type, INamespaceSymbol? targetNamespace)
    {
        if (type is null || targetNamespace == null)
        {
            return false;
        }

        var baseType = type.BaseType;

        while (baseType is not null)
        {
            if (baseType.ContainingNamespace is not null &&
                SymbolEqualityComparer.Default.Equals(baseType.ContainingNamespace, targetNamespace))
            {
                return true;
            }

            baseType = baseType.BaseType;
        }

        return false;
    }

    public static IEnumerable<INamespaceSymbol> GetNamespaceMembersRecursively(this INamespaceSymbol namespaceSymbol)
    {
        yield return namespaceSymbol;

        foreach (var child in namespaceSymbol.GetNamespaceMembers())
        {
            foreach (var sub in child.GetNamespaceMembersRecursively())
            {
                yield return sub;
            }
        }
    }

    public static bool ImplementsInterface(this INamedTypeSymbol? symbol, INamedTypeSymbol? interfaceSymbol)
    {
        if (symbol == null || interfaceSymbol == null)
        {
            return false;
        }

        return symbol.AllInterfaces
            .Any(i => SymbolEqualityComparer.Default.Equals(i.OriginalDefinition, interfaceSymbol));
    }
}