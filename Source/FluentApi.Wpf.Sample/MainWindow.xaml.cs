using System.Windows;

namespace Bars.Mvvm.FluentApi.Wpf.Sample;

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