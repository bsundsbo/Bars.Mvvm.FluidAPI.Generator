# Bars.Mvvm.FluidApi.Generator

This source code generator generates extension methods for Actipro's WPF Bars ViewModels to allow for method chaining to set properties in a more fluid API style. 

# Features 
* Extension methods for fluent property assignment.
* Convenience methods to reduce boilerplate when building bar menus.

# Basic Usage

```csharp
// Method chaining to set properties.
new BarToggleButtonViewModel(key)
    .WithCommand(setTextAlignmentCommand, TextAlignment.Center)
    .WithKeyTipText("AC")
    .WithDescription("Center content with the page."));
```
Release Notes:

* 0.2.0
  * Added some minor tweaks to the WithFooter convenience method for InfoBar padding to be 0 rather than the default padding.
  * Extended the sample application and cleaned up some code related to another project that will be split into its own repository.


* 0.1.0 
  * Initial release with basic functionality for generating extension methods.
  * Convenience methods:
    * `WithCommand` to assign commands and parameters in a single call for ICommand properties with matching command parameter property.
    * `BarComboBox.WithUnmatchedTextCommand` with overload for setting IsUnmatchedTextAllowed.
    * `IHasVariantImages.WithImages` to set all image sizes for a `IHasVariantImages` using a registered image from `IBarImageProvider`.
    * `RibbonViewModel.WithFooter` with various overloads for quickly setting simple footer or info bar footer.
    * `WithItem`, `WithTab`, `WithGroup` for adding items to read-only `ObservableCollection<T>` properties on ViewModels like `RibbonTabViewModel.Groups`, `RibbonGroupViewModel.Items`, and `BarComboBoxViewModel.AboveMenuItems`.
    * `WithItems`, `WithTabs`, etc for adding a batch of items to `ObservableCollection<T>` properties.
    * `RibbonBackstageViewModel.WithItemSeparator` for adding a separator with top alignment, or provide parameter for bottom.