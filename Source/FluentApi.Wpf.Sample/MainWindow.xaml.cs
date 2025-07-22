using System.Windows;

namespace Bars.Mvvm.FluentApi.Generator.Sample;

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