using ActiproSoftware.Windows.Controls;
using ActiproSoftware.Windows.Controls.Bars;
using ActiproSoftware.Windows.Controls.Bars.Mvvm;
using ActiproSoftware.Windows.Input;
using MahApps.Metro.IconPacks;
using System.Windows;
using System.Windows.Input;

namespace Bars.Mvvm.FluidGenerator.Sample;

public class SampleBarManagerCommands
{
    public ICommand NotImplementedCommand { get; }
    public ICommand ShowSimpleFooterCommand { get; }
    public ICommand ShowFooterInfoBarCommand { get; set; }

    public SampleBarManagerCommands(RibbonViewModel ribbon)
    {
        NotImplementedCommand = new DelegateCommand<object>(OnNotImplementedCommand);
        ShowSimpleFooterCommand = new DelegateCommand<object>(_ =>
        {
            ribbon.WithFooter("I have a warning for you", BarImageOptions.Default.CreateImage(PackIconMaterialKind.Alert), RibbonFooterKind.Warning);
        });
        ShowFooterInfoBarCommand = new DelegateCommand<object>(_ =>
        {
            ribbon.WithFooter("Title", "Message", severity: InfoBarSeverity.Error, canClose: true);
        });
    }

    private static void OnNotImplementedCommand(object? param)
    {
        ThemedMessageBox.Show(param == null
            ? "Default command handler."
            : $"Default command handler with parameter '{param}'.", "Not Implemented", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
