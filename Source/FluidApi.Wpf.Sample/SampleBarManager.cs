using ActiproSoftware.Windows.Controls;
using ActiproSoftware.Windows.Controls.Bars;
using ActiproSoftware.Windows.Controls.Bars.Mvvm;
using ActiproSoftware.Windows.Input;
using System.Windows;
using System.Windows.Input;

namespace Bars.Mvvm.FluidGenerator.Sample;

public class SampleBarManager
{
    private readonly ICommand _notImplementedCommand;

    /// <summary>
    /// Gets the collection of control view models.
    /// </summary>
    /// <value>A <see cref="BarControlViewModelCollection"/>.</value>
    public BarControlViewModelCollection ControlViewModels { get; } = new();
    public BarImageProvider ImageProvider { get; } = new();

    public SampleBarManager()
    {
        _notImplementedCommand = new DelegateCommand<object>(param =>
            {
                ThemedMessageBox.Show(param == null
                        ? "Default command handler."
                        : $"Default command handler with parameter '{param}'.",
                    "Not Implemented",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        );

        // Register common images used by view models
        RegisterImages();

        // Register view models for controls
        RegisterControlViewModels();
    }

    private void RegisterControlViewModels()
    {
        ControlViewModels.Register(SampleControlKeys.Copy, key => new BarButtonViewModel(key)
            .WithCommand(_notImplementedCommand, "Parameter"));
        ControlViewModels.Register(SampleControlKeys.Cut, key => new RedBarButtonViewModel(key)
            .WithDescription($"This red button is read from {nameof(CustomDataTemplateSelector)}")
            .WithCommand(_notImplementedCommand, "I am a great red button read from template!"));
        ControlViewModels.Register(SampleControlKeys.ReferenceCodes, CreateReferenceCodeControl);
    }

    private static IHasKey CreateReferenceCodeControl(string key)
    {
        var items = new List<ReferenceCode>
        {
            new ("Code1", "Description for Code1"),
            new ("Code2", "Description for Code2"),
            new ("Code3", "Description for Code3")
        };

        var galleryItems = items.Select(item => new ReferenceCodeGalleryItemViewModel(item))
            .ToList();
        return new BarComboBoxViewModel(key, galleryItems)
            .WithLabel("Reference Codes")
            .WithDescription("This is a combo box with a custom template selector.")
            .WithItemTemplateSelector(new CustomGalleryTemplateSelector());
    }

    private void RegisterImages()
    {
        // No images registered in this sample yet
    }

    public RibbonViewModel GetRibbonViewModel()
    {
        return new RibbonViewModel()
            .WithItemContainerTemplateSelector(new CustomDataTemplateSelector())
            .WithQuickAccessToolBarMode(RibbonQuickAccessToolBarMode.None)
            .WithIsApplicationButtonVisible()
            .WithGroupLabelMode(RibbonGroupLabelMode.Always)
            .WithLayoutMode(RibbonLayoutMode.Simplified)
            .WithApplicationButton(new RibbonApplicationButtonViewModel("ApplicationButton")
                .WithLabel("Magic"))
            .WithTab(new RibbonTabViewModel("Tab1")
                .WithLabel("Tab label")
                .WithDescription("Tab description")
                .WithGroup(new RibbonGroupViewModel("Edit group")
                    .WithItem(ControlViewModels[SampleControlKeys.Copy])
                    .WithItem(ControlViewModels[SampleControlKeys.Cut])
                    .WithItem(ControlViewModels[SampleControlKeys.ReferenceCodes])));
    }
}