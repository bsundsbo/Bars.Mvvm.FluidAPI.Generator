using Bars.Mvvm.FluidApi.Common;
using Bars.Mvvm.FluidGenerator.Models;
using Scriban;
using System;
using System.Collections.Generic;

namespace Bars.Mvvm.FluidApi.Generator;

/// <summary>
/// This class maps property template models to their corresponding Scriban templates.
/// </summary>
internal class TemplateMapper
{
    private readonly Dictionary<Type, Template> _templateMapping = new()
    {
        {
            typeof(ObservableCollectionTemplateModel), Template.Parse(EmbeddedResource.GetContent("Templates/GetOnlyObservableCollection.fg-cs"))
        },
        {
            typeof(CommandWithParameterTemplateModel), Template.Parse(EmbeddedResource.GetContent("Templates/CommandWithParameter.fg-cs"))
        },
        {
            typeof(BooleanPropertyTemplateModel), Template.Parse(EmbeddedResource.GetContent("Templates/BooleanProperty.fg-cs"))
        },
        {
            typeof(PropertyTemplateModel), Template.Parse(EmbeddedResource.GetContent("Templates/DefaultProperty.fg-cs"))
        },
        {
            typeof(HasVariantImageTemplateModel), Template.Parse(EmbeddedResource.GetContent("Templates/HasVariantImages.fg-cs"))
        }
    };

    /// <summary>
    /// Gets the Scriban template for the given model type.
    /// </summary>
    /// <param name="model">The <see cref="IExtensionTemplateModel"/>> to get a template for.</param>
    /// <returns>Template for processing the property into code.</returns>
    public Template? GetTemplate(IExtensionTemplateModel model)
    {
        return _templateMapping.TryGetValue(model.GetType(), out var t)
            ? t
            : null;
    }
}
