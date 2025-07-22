using ActiproSoftware.Windows.Controls.Bars.Mvvm;

namespace Bars.Mvvm.FluentApi.Generator.Sample;

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
    public static string Paste => nameof(Paste);
    /// <summary>
    /// Represents the key for the cut control.
    /// </summary>
    public static string Cut => nameof(Cut);
    public static string BackstageTabHome => nameof(BackstageTabHome);
    public static string BackstageTabNew => nameof(BackstageTabNew);
    public static string BackstageButtonClose => nameof(BackstageButtonClose);
    public static string BackstageButtonPrint => nameof(BackstageButtonPrint);
    public static string FooterGroup => nameof(FooterGroup);
    public static string FooterGroupSimple => nameof(FooterGroupSimple);
    public static string FooterGroupInfoBar => nameof(FooterGroupInfoBar);
}
