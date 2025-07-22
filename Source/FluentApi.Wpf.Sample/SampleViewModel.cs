using ActiproSoftware.Windows;
using ActiproSoftware.Windows.Controls.Bars.Mvvm;

namespace Bars.Mvvm.FluentApi.Generator.Sample;

/// <summary>
/// Main viewmodel for the sample application.
/// </summary>
public class SampleViewModel : ObservableObjectBase
{
    /// <summary>
    /// Represents the ribbon showed in the sample application.
    /// </summary>
    public RibbonViewModel Ribbon { get; }

    public SampleViewModel()
    {
        var barManager = new SampleBarManager();
        Ribbon = barManager.GetRibbonViewModel();
    }
}