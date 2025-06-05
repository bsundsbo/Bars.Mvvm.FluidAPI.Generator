using ActiproSoftware.Windows.Controls.Bars.Mvvm;

namespace Bars.Mvvm.FluidGenerator.Sample;

/// <summary>
/// This class contains the keys for the sample controls, which are referenced when registering the controls with <see cref="BarControlViewModelCollection"/>
/// and the <see cref="RibbonViewModel"/>, <see cref="StandaloneToolBarViewModel"/> or <see cref="MiniToolBarViewModel"/> is created.
/// </summary>
public static class SampleControlKeys
{
    /// <summary>
    /// Represents the key for the copy control.
    /// </summary>
    public const string Copy = nameof(Copy);
    /// <summary>
    /// Represents the key for the cut control.
    /// </summary>
    public static string Cut => nameof(Cut);

    /// <summary>
    /// Represents the key for the reference codes control.
    /// </summary>
    public static string ReferenceCodes => nameof(ReferenceCodes);
}
