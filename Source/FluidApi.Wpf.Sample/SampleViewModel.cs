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
        Ribbon = new RibbonViewModel()
            .WithItemContainerTemplateSelector(new CustomDataTemplateSelector())
            .WithQuickAccessToolBarMode(RibbonQuickAccessToolBarMode.None)
            .WithLayoutMode(RibbonLayoutMode.Simplified)
            .WithApplicationButton(new RibbonApplicationButtonViewModel("ApplicationButton")
                .WithLabel("Magic")
            )
            .WithFooter(new RibbonFooterViewModel()
                .WithKind(RibbonFooterKind.Warning)
                .WithContent("Warning message")
            )
            .WithTab(new RibbonTabViewModel("Tab1")
                .WithLabel("Tab label")
                .WithDescription("Tab description")
                .WithGroup(new RibbonGroupViewModel("Group1")
                    .WithItem(new BarButtonViewModel("Button1"))
                    .WithItem(new BarButtonViewModel("Button2")
                        .WithCommand(new DelegateCommand<string>(s => MessageBox.Show(s)), commandParameter: "this is the parameter")
                    )
                    .WithItem(new BarButtonViewModel(SampleControlKeys.BarButton3)
                        .WithCommand(new DelegateCommand<string>(_ => MessageBox.Show("Button3 clicked")
                            )
                        )
                    )
                    .WithItem(new RedBarButtonViewModel("buttonRed"))
                    .WithItem(new BlueBarButtonViewModel("buttonBlue"))
                ));
    }
}
