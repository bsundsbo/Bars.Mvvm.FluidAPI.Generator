using System.Windows;

namespace Bars.Mvvm.FluidGenerator.Sample;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        DataContext = new SampleViewModel();
        InitializeComponent();
    }
}