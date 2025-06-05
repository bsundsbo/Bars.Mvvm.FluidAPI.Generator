using ActiproSoftware.Windows;

namespace Bars.Mvvm.FluidGenerator.Sample;

/// <summary>
/// Sample reference code model.
/// </summary>
public class ReferenceCode(string code, string name) : ObservableObjectBase
{
    private string _code = code;
    private string _name = name;

    /// <summary>
    /// Name of the reference code.
    /// </summary>
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    /// <summary>
    /// Code of the reference code.
    /// </summary>
    public string Code
    {
        get => _code;
        set => SetProperty(ref _code, value);
    }
}
