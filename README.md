# Bars.Mvvm
# Fluid API for Actipro WPF Bars
The goal of this project is to extend ActiproSoftware's [WPF-Library](https://github.com/Actipro/WPF-Controls) to offer a more simplified implementation of [bar menus](https://www.actiprosoftware.com/products/controls/wpf/bars) through MVVM.

[![NuGet](https://img.shields.io/nuget/v/Bars.Mvvm.FluidGenerator.svg)](https://www.nuget.org/packages/Bars.Mvvm.FluidApi.Generator/)

# Fluid API Features
* **Fluent API**: The fluid API allows you to create bar menus in a more declarative way, making it easier to read and maintain. 
* **Convenience Methods**: The API provides convenience methods to set properties and commands in a single call, reducing boilerplate code.
* **DataTemplate selector generator**: reduces the need to manually create `ComponentResourceKey` and map to the properties of `DataTemplateSelector` or `ItemContainerTemplateSelector`.

## Convenience methods
Providing fluid API for convenience methods to set properties and construct new viewmodels.

### Images
The `WithImages` method allows you to set all image sizes for a bar item using a registered image key. Available for all view models implementing `IHasVariantImages` interface and all images can be set in one call.

```csharp
return new BarButtonViewModel(key)
    .WithImages(imageProvider, key);
```

### Command and command parameters
Assign a command and its parameter in a single call using the `WithCommand` method. This is available for all view models that have a matching `CommandParameter` property, such as `WithPopupOpeningCommand`, `WithUnmatchedTextCommand`, etc.

```csharp
return new BarToggleButtonViewModel(key)
    .WithCommand(setTextAlignmentCommand, TextAlignment.Center);
```

### Tabs, Items, AboveMenuItem, etc.
Properties that are declared on the ViewModel as `ObservableCollection<T>`, such as `RibbonTabViewModel.Groups`, RibbonGroupViewModel.Items, BarComboBoxViewModel.AboveMenuItems.

Adding a batch of items 
```csharp
var items = new []
{
    new BarButtonViewModel("Item1"),
    new BarButtonViewModel("Item2")
};
return new BarComboBoxViewModel(key)
    .WithAboveMenuItems(items);
```

Or one by one

```csharp
return new RibbonGroupViewModel("Group1")
    .WithItem(new BarButtonViewModel("Button1"))
    .WithItem(new BarButtonViewModel("Button2");
```
### BarComboBoxViewModel.WithUnmatchedTextCommand
Use `WithUnmatchedTextCommand` to set IsUnmatchedTextAllowed and UnmatchedTextCommand in a single call.
```csharp
return new BarComboBoxViewModel()
    .WithUnmatchedTextCommand(unmatchedTextCommand, true);
```

## Simple sample
As a simple syntax example, the following code shows how to create a `BarToggleButtonViewModel` with a command and some additional properties using the fluid API.

**Note** that the `WithCommand` method is an extension method that allows you to set the command and its parameter in a single call. This is available for all sort of `ICommand` that have a matching `CommandParameter` property, such as `WithPopupOpeningCommand`, `WithUnmatchedTextCommand`, etc.

<details>
<summary>Old syntax</summary>

### Old syntax

```csharp
viewModels.Register(BarControlKeys.AlignCenter, key
	=> new BarToggleButtonViewModel(key, SetTextAlignmentCommand)
	{
		KeyTipText = "AC", 
		Description = "Center content with the page.", 
		CommandParameter = TextAlignment.Center
	});
```

</details>

### New syntax
```csharp
viewModels.Register(BarControlKeys.AlignCenter, key
    => new BarToggleButtonViewModel(key)
        .WithCommand(setTextAlignmentCommand, TextAlignment.Center)
        .WithKeyTipText("AC")
        .WithDescription("Center content with the page."));
```


The fluid API is available as a incremental Source Generator to reduce the need to upgrade to a new package every time Actipro adds new properties to their ViewModels and then having to wait for a new version of the Fluid API NuGet package to get full support.


## License

Copyright © Bjørnar Sundsbø and Contributors. All rights reserved.

`Bars.Mvvm` is provided as-is under the BSD 3-Clause License. For more information see [LICENSE](./LICENSE).
# NuGet Packages




## Getting Started
1. Install the NuGet package `Bars.Mvvm.FluidGenerator` in your project.

