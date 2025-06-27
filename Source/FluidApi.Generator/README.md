# Bars.Mvvm.FluidApi.Generator
[![NuGet](https://img.shields.io/nuget/v/Bars.Mvvm.FluidApi.Generator.svg)](https://www.nuget.org/packages/Bars.Mvvm.FluidApi.Generator/)
[![Downloads](https://img.shields.io/nuget/dt/Bars.Mvvm.FluidApi.Generator?label=Downloads)](https://www.nuget.org/packages/Bars.Mvvm.FluidApi.Generator/)

This package generates extension methods for Actipro's WPF Bars ViewModels for building bars menu and configuring properties, providing a fluid API for creating bar menus in a more declarative way.

# Features 
* Generated settings for individual properties, to allow for chaining methods in a fluid API style.
* Convenience methods for reducing boiler plate when constructing bar menus.

# Getting started
```
dotnet add package Bars.Mvvm.FluidApi.Generator
```

## Pre-generated extension methods
If you wish to use pre-generated package instead, with the limitation that you will have to update the package every time Actipro adds new properties to their ViewModels and a new version of this package is available.
```
dotnet add package Bars.Mvvm.FluidApi.Wpf
```

# Usage

## Basic Usage
To use the fluid API, you need to register the generated extension methods in your project. This is typically done in a static class that implements `IBarViewModelProvider`.

```csharp
iewModels.Register(BarControlKeys.AlignCenter, key
    => new BarToggleButtonViewModel(key)
        .WithCommand(setTextAlignmentCommand, TextAlignment.Center)
        .WithKeyTipText("AC")
        .WithDescription("Center content with the page."));
```

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
d in Avalonia support, please let me know by creating an issue or a discussion in the repository.