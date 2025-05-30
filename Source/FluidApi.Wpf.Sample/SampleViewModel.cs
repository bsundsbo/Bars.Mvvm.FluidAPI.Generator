using ActiproSoftware.Windows;
using ActiproSoftware.Windows.Controls.Bars;
using ActiproSoftware.Windows.Controls.Bars.Mvvm;
using ActiproSoftware.Windows.Input;
using System.Windows;

namespace Bars.Mvvm.FluidGenerator.Sample;

public class SampleViewModel : ObservableObjectBase
{
    public RibbonViewModel Ribbon { get; }

    public SampleViewModel()
    {
        var barManager = new SampleBarManager();
        Ribbon = barManager.GetRibbonViewModel();
    }
}