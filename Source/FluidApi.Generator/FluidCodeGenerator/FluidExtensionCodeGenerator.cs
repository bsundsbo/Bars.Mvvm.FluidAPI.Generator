﻿using Bars.Mvvm.FluidApi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Bars.Mvvm.FluidGenerator.Models;
using Microsoft.CodeAnalysis;
using Scriban;

namespace Bars.Mvvm.FluidApi.Generator;

/// <summary>
/// This generator is responsible for generating Fluid API for Bars MVVM ViewModels for easy configuration.
/// Example usage is
/// <code>
/// new BarButtonViewModel("key")
///     .WithLabel("Click!")
///     .WithCommand(new RoutedCommand())
///     .WithImages(new BarImageProvider(), "key");
/// </code>
/// </summary>
internal class FluidExtensionCodeGenerator(FluidExtensionPropertyParser parser)
{
    private static int _counter;
    private readonly TemplateMapper _templateMapper = new();

    private readonly Dictionary<TargetLibrary, Dictionary<string, string>> _convenienceExtensionMethodTemplates = new()
    {
        // Holds manually defined extension methods for specific ViewModels
        {
            TargetLibrary.Wpf, new Dictionary<string, string>
            {
                { "BarComboBoxViewModel", EmbeddedResource.GetContent("CustomExtensions/Wpf/BarComboBoxViewModel.fg-cs") },
                { "RibbonViewModel", EmbeddedResource.GetContent("CustomExtensions/Wpf/RibbonViewModel.fg-cs") },
                { "RibbonBackstageViewModel", EmbeddedResource.GetContent("CustomExtensions/Wpf/RibbonBackstageViewModel.fg-cs") },
            }
        }
    };

    public string Generate(INamedTypeSymbol classSymbol)
    {
        var propertyTemplateModels = parser.GetPropertyTemplateModels(classSymbol);
        if (propertyTemplateModels.Count == 0)
        {
            return string.Empty;
        }

        var nsName = classSymbol.ContainingNamespace.ToDisplayString();
        var className = classSymbol.Name;

        var sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine("// <auto-generated/>");
        sourceBuilder.AppendLine($"// generation counter: {Interlocked.Increment(ref _counter)}");

        GenerateUsings(sourceBuilder);

        sourceBuilder.AppendLine($"namespace {nsName};");
        sourceBuilder.AppendLine($"public static class {className}Extensions");
        sourceBuilder.AppendLineStartBracket(0);

        GenerateExtensionMethods(propertyTemplateModels, sourceBuilder, className);

        // Class end bracket
        sourceBuilder.AppendLineEndBracket(0);
        return sourceBuilder.ToString();
    }

    private void GenerateUsings(StringBuilder sourceBuilder)
    {
        if (parser.TargetLibrary == TargetLibrary.Avalonia)
        {
            sourceBuilder.AppendLine("#nullable enable");
            sourceBuilder.AppendLine("using Avalonia.Controls.Templates;");
            sourceBuilder.AppendLine("using Avalonia.Input;");
            sourceBuilder.AppendLine("using Avalonia.Styling;");
        }
        else
        {
            sourceBuilder.AppendLine("using System;");
            sourceBuilder.AppendLine("using System.Collections.Generic;");
            sourceBuilder.AppendLine("using System.Windows.Input;");
        }
    }

    private void GenerateExtensionMethods(List<IExtensionTemplateModel> propertyTemplateModels, StringBuilder sourceBuilder, string className)
    {
        foreach (var propertyModel in propertyTemplateModels)
        {
            var template = _templateMapper.GetTemplate(propertyModel);
            if (template == null)
            {
                continue;
            }

            var output = template.Render(propertyModel, MemberRenamer);
            sourceBuilder.AppendLine(output);
        }

        // Append custom extensions if any
        AppendCustomExtensions(className, sourceBuilder);
    }

    private void AppendCustomExtensions(string className, StringBuilder sourceBuilder)
    {
        // Custom templates for specific generated types
        if (!_convenienceExtensionMethodTemplates.TryGetValue(parser.TargetLibrary, out var customTemplateDictionary))
        {
            return;
        }

        if (!customTemplateDictionary.TryGetValue(className, out string customTemplates))
        {
            return;
        }

        sourceBuilder.AppendLine(customTemplates);
    }

    private static string MemberRenamer(MemberInfo member) => member.Name;
}
