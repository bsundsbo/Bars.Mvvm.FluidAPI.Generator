using JetBrains.Annotations;

namespace Bars.Mvvm.FluentApi.Generator.Models;

/// <summary>
/// Represents a class used for generating code for setter for IHasVariantImages
/// </summary>
/// <param name="ClassName">Name of the class.</param>
public record HasVariantImageTemplateModel([UsedImplicitly(Reason = "Used in template")] string ClassName) : IExtensionTemplateModel;
