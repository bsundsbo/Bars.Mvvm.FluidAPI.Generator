using System.Text;

namespace Bars.Mvvm.FluidApi.Generator.Wpf.Test;

/// <summary>
/// Populates the validation result with the errors found.
/// </summary>
public class ValidationResult(string errorDescription)
{
    private readonly Dictionary<string, List<string>> _errors = new();

    /// <summary>
    /// Indicates if there are any validation errors.
    /// </summary>
    public bool HasValidationErrors => _errors.Count > 0;
    public void AddError(string sourceTypeName, string propertyName)
    {
        _errors.TryAdd(sourceTypeName, new List<string>());
        _errors[sourceTypeName].Add(propertyName);
    }

    public void AddError(string sourceTypeName, string propertyName, string descriptiveMessage)
    {
        _errors.TryAdd(sourceTypeName, new List<string>());
        _errors[sourceTypeName].Add($"{propertyName}: {descriptiveMessage}");
    }

    /// <summary>
    /// Get the error message for the validation result.
    /// </summary>
    public string GetErrorMessage()
    {
        var builder = new StringBuilder();
        builder.AppendLine(errorDescription);
        foreach (var error in _errors)
        {
            builder.AppendLine($"  {error.Key}:");
            foreach (var propertyName in error.Value)
            {
                builder.AppendLine($"    {propertyName}");
            }
        }

        return builder.ToString();
    }
}
