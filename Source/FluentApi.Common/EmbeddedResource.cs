using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Bars.Mvvm.FluidApi.Common;

/// <summary>
/// Helper class for reading embedded resources.
/// </summary>
public static class EmbeddedResource
{
    /// <summary>
    /// Get the content of an embedded resource as a string, using the relative path to the resource.
    /// The files must be marked as "Embedded Resource" in the project properties.
    /// </summary>
    /// <param name="relativePath"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static string GetContent(string relativePath)
    {
        var baseName = Assembly.GetExecutingAssembly().GetName().Name;
        var resourceName = relativePath
            .TrimStart('.')
            .Replace(Path.DirectorySeparatorChar, '.')
            .Replace(Path.AltDirectorySeparatorChar, '.');

        var manifestResourceName = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceNames()
            .FirstOrDefault(x => x!.EndsWith(resourceName, StringComparison.InvariantCulture));

        if (string.IsNullOrEmpty(manifestResourceName))
        {
            throw new InvalidOperationException(
                $"Did not find required resource ending in '{resourceName}' in assembly '{baseName}'."
            );
        }

        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(manifestResourceName);

        if (stream == null)
        {
            throw new InvalidOperationException(
                $"Did not find required resource '{manifestResourceName}' in assembly '{baseName}'."
            );
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
