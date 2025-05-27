using Microsoft.CodeAnalysis;

namespace Bars.Mvvm.FluidApi.Common;

/// <summary>
/// Extension methods for <see cref="INamedTypeSymbol"/>.
/// </summary>
public static class ActiproTypeExtensions
{
    public const string WpfNamespace = "ActiproSoftware.Windows.Controls.Bars.Mvvm";
    // ReSharper disable MemberCanBePrivate.Global
    public const string WpfObservableObjectBase = "ActiproSoftware.Windows.ObservableObjectBase";
    public const string AvaloniaNamespace = "ActiproSoftware.UI.Avalonia.Controls.Bars.Mvvm";
    public const string AvaloniaObservableObject = "ActiproSoftware.ObservableObjectBase";
    // ReSharper restore MemberCanBePrivate.Global

    /// <summary>
    /// Helper method for determining if the type is a class that we want to generate for.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static bool IsPublicNonAbstractNonGeneric(this INamedTypeSymbol? t)
    {
        return t is {TypeKind: TypeKind.Class}
            and {IsAbstract: false, IsReferenceType: true, IsGenericType: false}
            and {DeclaredAccessibility: Accessibility.Public, IsStatic: false};
    }

    /// <summary>
    /// Get a value indicating whether the type is in the namespace ActiproSoftware.Windows.Controls.Bars.Mvvm.
    /// </summary>
    /// <param name="type">The type to check</param>
    /// <returns><c>true</c> if type is in the namespace; <c>false</c> otherwise.</returns>
    public static bool IsMvvmNamespace(this INamedTypeSymbol? type)
    {
        // This may be possible to check by using the namespace symbol and SymbolEqualityComparer.Default
        // but that requires Combining the result with the Compilation.
        // Will try this in another iteration of the product
        var containingNamespace = type?.ContainingNamespace.ToDisplayString();
        return type is not null && containingNamespace is WpfNamespace or AvaloniaNamespace;
    }

    /// <summary>
    /// Get a value indicating whether the type derives from ActiproSoftware.Windows.ObservableObjectBase.
    /// </summary>
    /// <param name="type">The type to analyze.</param>
    /// <returns><c>true</c> if it is an ObservableObjectBase; <c>false</c> otherwise.</returns>
    public static bool DerivesFromObservableObjectBase(this INamedTypeSymbol? type)
    {
        if (type is null)
        {
            return false;
        }

        var baseType = type.BaseType;

        while (baseType is not null)
        {
            // I've tried getting the ObservableObjectBase through compilation.GetTypeByMetadataName() and compare
            // against originalDefinition using SymbolEqualityComparer.Default, however, that does not work.
            // Presumably because it has been defined in another assembly that is not fully loaded.
            // Looking for a faster way to do this rather than creating a ToDisplayString()
            var originalDefinition = baseType.OriginalDefinition.ToDisplayString();
            if (originalDefinition is WpfObservableObjectBase or AvaloniaObservableObject)
            {
                return true;
            }

            baseType = baseType.BaseType;
        }

        return false;
    }
}
