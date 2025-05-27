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
        var templateSelector = new BarControlTemplateSelector();
        var imageProvider = new BarImageProvider();

        Ribbon = new RibbonViewModel()
            .WithItemContainerTemplateSelector(templateSelector)
            .WithQuickAccessToolBarMode(RibbonQuickAccessToolBarMode.None)
            .WithIsOptionsButtonVisible(false)
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
                .WithGroups(new []{})
                .WithGroup(new RibbonGroupViewModel("Group1")
                    .WithItem(new BarButtonViewModel("Button1"))
                    .WithItem(new BarButtonViewModel("Button2")
                        .WithCommand(new DelegateCommand<string>(s => MessageBox.Show(s)), commandParameter: "this is the parameter")
                    )
                    .WithItem(new BarButtonViewModel("Button3")
                        .WithCommand(new DelegateCommand<string>(
                                _ => MessageBox.Show("Button3 clicked")
                            )
                        )
                        .WithImages(imageProvider, BarControlKeys.BarButton3)
                    )
                ));
        var setTextAlignmentCommand = new DelegateCommand<string>(_ => MessageBox.Show("Button3 clicked"));
        var viewModels = new BarControlViewModelCollection();

        viewModels.Register(BarControlKeys.AlignCenter, key
            => new BarToggleButtonViewModel(key)
                .WithCommand(setTextAlignmentCommand, TextAlignment.Center)
                .WithKeyTipText("AC")
                .WithDescription("Center content with the page."));
    }
}
