using ActiproSoftware.Windows.Controls.Bars;
using ActiproSoftware.Windows.Controls.Bars.Mvvm;

namespace Bars.Mvvm.FluentApi.Generator.Sample;

public class SampleBarManager
{
    private readonly SampleBarManagerCommands _commands;
    private readonly RibbonViewModel _ribbon;

    /// <summary>
    /// Gets the collection of control view models.
    /// </summary>
    /// <value>A <see cref="BarControlViewModelCollection"/>.</value>
    public BarControlViewModelCollection ControlViewModels { get; } = new();

    public SampleBarImageProvider ImageProvider { get; } = new();

    public SampleBarManager()
    {
        _ribbon = new RibbonViewModel();
        _commands = new SampleBarManagerCommands(_ribbon);

        // Register view models for controls
        RegisterControlViewModels();
    }

    private void RegisterControlViewModels()
    {
        ControlViewModels.Register(SampleControlKeys.Copy, key => new BarButtonViewModel(key)
            .WithCommand(_commands.NotImplementedCommand, "Copy")
            .WithToolBarItemVariantBehavior(ItemVariantBehavior.All)
            .WithImages(ImageProvider));
        ControlViewModels.Register(SampleControlKeys.Paste, key => new BarButtonViewModel(key)
            .WithCommand(_commands.NotImplementedCommand, "Paste")
            .WithToolBarItemVariantBehavior(ItemVariantBehavior.All)
            .WithImages(ImageProvider));
        ControlViewModels.Register(SampleControlKeys.Cut, key => new BarButtonViewModel(key)
            .WithCommand(_commands.NotImplementedCommand, "Cut")
            .WithToolBarItemVariantBehavior(ItemVariantBehavior.All)
            .WithImages(ImageProvider));

        ControlViewModels.Register(SampleControlKeys.FooterGroupSimple, key => new BarButtonViewModel(key)
            .WithLabel("Simple footer")
            .WithCommand(_commands.ShowSimpleFooterCommand)
            .WithToolBarItemVariantBehavior(ItemVariantBehavior.All)
            .WithImages(ImageProvider));
        ControlViewModels.Register(SampleControlKeys.FooterGroupInfoBar, key => new BarButtonViewModel(key)
            .WithLabel("Info bar footer")
            .WithCommand(_commands.ShowFooterInfoBarCommand)
            .WithToolBarItemVariantBehavior(ItemVariantBehavior.All)
            .WithImages(ImageProvider));
    }

    public RibbonViewModel GetRibbonViewModel()
    {
        return _ribbon
            .WithItemContainerTemplateSelector(new BarControlTemplateSelector())
            .WithQuickAccessToolBarMode(RibbonQuickAccessToolBarMode.None)
            .WithGroupLabelMode(RibbonGroupLabelMode.Always)
            .WithLayoutMode(RibbonLayoutMode.Simplified)
            .WithIsApplicationButtonVisible()
            .WithBackstage(GetBackstage())
            .WithApplicationButton(new RibbonApplicationButtonViewModel("ApplicationButton")
                .WithLabel("File"))
            .WithTab(new RibbonTabViewModel("Tab1")
                .WithLabel("Tab label")
                .WithDescription("Tab description")
                .WithGroup(new RibbonGroupViewModel("Edit")
                    .WithItem(ControlViewModels[SampleControlKeys.Copy])
                    .WithItem(ControlViewModels[SampleControlKeys.Paste])
                    .WithItem(ControlViewModels[SampleControlKeys.Cut]))
                .WithGroup(new  RibbonGroupViewModel(SampleControlKeys.FooterGroup)
                    .WithLabel("Footer")
                    .WithItem(ControlViewModels[SampleControlKeys.FooterGroupSimple])
                    .WithItem(ControlViewModels[SampleControlKeys.FooterGroupInfoBar])));
    }

    private RibbonBackstageViewModel GetBackstage()
    {
        return new RibbonBackstageViewModel()
            .WithItem(new ImageRibbonBackstageTabViewModel(SampleControlKeys.BackstageTabHome)
                .WithImages(ImageProvider)
                .WithLabel("Home"))
            .WithItem(new ImageRibbonBackstageTabViewModel(SampleControlKeys.BackstageTabNew)
                .WithImages(ImageProvider)
                .WithLabel("New"))
            .WithItemSeparator(RibbonBackstageHeaderAlignment.Top)
            .WithItem(new ImageRibbonBackstageHeaderButtonViewModel(SampleControlKeys.BackstageButtonClose)
                .WithImages(ImageProvider)
                .WithLabel("Close")
                .WithCommand(_commands.NotImplementedCommand))
            .WithItem(new ImageRibbonBackstageHeaderButtonViewModel(SampleControlKeys.BackstageButtonPrint)
                .WithImages(ImageProvider)
                .WithLabel("Print")
                .WithCommand(_commands.NotImplementedCommand));
    }
}