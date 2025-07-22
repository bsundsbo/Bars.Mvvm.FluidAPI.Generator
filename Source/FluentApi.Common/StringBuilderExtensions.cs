using System.Text;

// ReSharper disable UnusedMethodReturnValue.Global

namespace Bars.Mvvm.FluentApi.Common;

/// <summary>
/// Contains extension methods for <see cref="StringBuilder"/> to simplify code generation.
/// </summary>
public static class StringBuilderExtensions
{
    /// <summary>
    /// Appends a line with the specified indentation and value to the <see cref="StringBuilder"/>.
    /// </summary>
    public static StringBuilder AppendLineWithIndent(this StringBuilder sourceBuilder, int indent, string value)
    {
        return sourceBuilder.Append(new string(' ', indent * 4))
            .AppendLine(value);
    }

    /// <summary>
    /// Appends a start bracket with the specified indentation to the <see cref="StringBuilder"/>.
    /// </summary>
    public static StringBuilder AppendLineStartBracket(this StringBuilder sourceBuilder, int indent)
    {
        return sourceBuilder.Append(new string(' ', indent * 4))
            .AppendLine("{");
    }

    /// <summary>
    /// Appends a end bracket with the specified indentation to the <see cref="StringBuilder"/>.
    /// </summary>
    public static StringBuilder AppendLineEndBracket(this StringBuilder sourceBuilder, int indent)
    {
        return sourceBuilder.Append(new string(' ', indent * 4))
            .AppendLine("}");
    }
}
