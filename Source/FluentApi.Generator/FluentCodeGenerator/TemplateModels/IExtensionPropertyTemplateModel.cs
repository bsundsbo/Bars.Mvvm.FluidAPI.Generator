namespace Bars.Mvvm.FluentApi.Generator.Extensions.Models;

/// <summary>
/// Interface for property template models that have an obsolete attribute.
/// </summary>
internal interface IExtensionPropertyTemplateModel : IExtensionTemplateModel
{
    /// <summary>
    /// Indicates whether the property is obsolete and should be marked as such in the generated code.
    /// </summary>
    bool IsObsolete { get; }

    /// <summary>
    /// Get the name of the property.
    /// </summary>
    string PropertyName { get; }
}
