using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Bars.Mvvm.FluidApi.Generator.Wpf.Test;

/// <summary>
/// Extension methods for helping with integration and unit tests
/// </summary>
public static class TestHelperExtensions
{
    /// <summary>
    /// Identify a class that is a likely candidate for an extension method class.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <param name="targetNamespace">The namespace for the type.</param>
    /// <returns><c>true</c> if the type is an extension class.</returns>
    private static bool IsExtensionMethodClass(this Type type, string targetNamespace)
    {
        if (!type.IsExtensionMethodClass())
        {
            return false;
        }

        return type.Namespace == targetNamespace;
    }

    /// <summary>
    /// Checks if the type is an extension class
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static bool IsExtensionMethodClass(this Type type)
    {
        return type is {IsClass: true, IsSealed: true} and
            {IsAbstract: true};
    }

    /// <summary>
    /// Get all types from the assembly of the given <paramref name="type"/> that are
    /// extension method classes.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="namespaceName"></param>
    /// <returns></returns>
    public static List<Type> GetExtensionClassesFromTypeAssembly(
        this Type type,
        string namespaceName)
    {
        var types = type
            .Assembly
            .GetTypes()
            .Where(t => t.IsExtensionMethodClass(namespaceName))
            .ToList();

        return types;
    }

    /// <summary>
    /// Get all public read/write properties of the given <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type to get properties for.</param>
    /// <returns></returns>
    public static List<PropertyInfo> GetReadWriteProperties(this Type type)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p is {CanRead: true, CanWrite: true})
            .ToList();
    }

    /// <summary>
    /// Get all public read-only properties of the given <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type to get properties for.</param>
    /// <returns></returns>
    public static List<PropertyInfo> GetReadOnlyProperties(this Type type)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p is {CanRead: true, CanWrite: false})
            .ToList();
    }

    public static List<PropertyInfo> GetReadOnlyPropertiesOfObservableCollection(this Type type)
    {
        return type.GetReadOnlyProperties()
            .Where(p => p.PropertyType.IsGenericType
                        && p.PropertyType.GetGenericTypeDefinition() == typeof(ObservableCollection<>))
            .ToList();
    }

    public static Dictionary<string, MethodInfo> GetGeneratedMethodsForType(this List<Type> generatedTypes, Type sourceType)
    {
        var generatedType = generatedTypes.Find(gt => gt.Name == $"{sourceType.Name}Extensions");
        if (generatedType == null)
        {
            return new Dictionary<string, MethodInfo>();
        }

        return generatedType.GetMethods(BindingFlags.Static | BindingFlags.Public)
            .ToDictionary(m => m.Name, m => m);
    }

    public static bool IsOptionalParameter<T>(this ParameterInfo parameter)
    {
        return parameter.IsOptional && (parameter.ParameterType == typeof(T)
                                        || Nullable.GetUnderlyingType(parameter.ParameterType) == typeof(T));
    }

    /// <summary>
    /// Determines if the given type implements the given interface.
    /// </summary>
    /// <param name="type">The type to check</param>
    /// <param name="interfaceType">The interface type to look for</param>
    /// <returns>Returns <c>true</c> if the type implements the interface; <c>false</c>, otherwise</returns>
    public static bool Implements(
        this Type type,
        Type interfaceType) =>
        type.GetInterfaces().Contains(interfaceType);
}


