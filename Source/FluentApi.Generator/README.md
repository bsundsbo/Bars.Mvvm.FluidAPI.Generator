# Bars.Mvvm.FluentApi.Generator
[![NuGet](https://img.shields.io/nuget/v/Bars.Mvvm.FluentApi.Generator.svg)](https://www.nuget.org/packages/Bars.Mvvm.FluentApi.Generator/)
[![Downloads](https://img.shields.io/nuget/dt/Bars.Mvvm.FluentApi.Generator?label=Downloads)](https://www.nuget.org/packages/Bars.Mvvm.FluentApi.Generator/)

This package generates extension methods for Actipro's WPF Bars ViewModels for building bars menu and configuring properties, providing a fluid API for creating bar menus in a more declarative way.

# Features
* Extension methods for fluent property assignment.
* Convenience methods to reduce boilerplate when building bar menus.

# [Release notes](./Package-README.md)

# Getting started
```
dotnet add package Bars.Mvvm.FluentApi.Generator
```

## Basic Usage

```csharp
new BarToggleButtonViewModel(key)
    .WithCommand(setTextAlignmentCommand, TextAlignment.Center)
    .WithKeyTipText("AC")
    .WithDescription("Center content with the page.");
```

## Convenience methods
Providing fluid API for convenience methods to set properties and construct new viewmodels.

### WithImages
**Target**: `IHasVariantImages`

**Parameter**: `IBarImageProvider`

**Effect**: Sets all image sizes for a bar item using a registered image.
```csharp
return new BarButtonViewModel(key)
    .WithImages(imageProvider);
```

### WithCommand
**Target**: `ICommand` property with a matching `CommandParameter` property.

**Parameter**: `ICommand`, `CommandParameter`

**Effect**: Assigns a command and its parameter in a single call.

**Examples**: WithCommand, WithPopupOpeningCommand, WithUnmatchedTextCommand
```csharp
return new BarToggleButtonViewModel(key)
    .WithCommand(setTextAlignmentCommand, TextAlignment.Center);
```

### ObservableCollection<T>
**Target**: read-only `ObservableCollection<T>` properties on ViewModels.

**Example**: `RibbonTabViewModel.Groups`, `RibbonGroupViewModel.Items`, `BarComboBoxViewModel.AboveMenuItems`.

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

### RibbonViewModel.Footer
Use `WithFooter` has multiple overloads for ease of use.

Simple footer with text and optional image/kind:

```csharp
.WithFooter("I have a warning for you!", warningImageSource, RibbonFooterKind.Warning)
```
Info bar footer without having to declare the RibbonFooterViewModel explicitly:
```csharp
.WithFooter(new RibbonFooterInfoBarContentViewModel()
    .WithSeverity(InfoBarSeverity.Error)
    .WithTitle("Footer Title")
    .WithMessage("This is a footer message!")
```

Info bar footer with a shorthand parameters:
```csharp
.WithFooter("Title", "Message", severity: InfoBarSeverity.Error, canClose: true)
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

## Avalonia support
Avalonia support is partially implemented, though not tested. Since I work with WPF, I want to extend to support what I need for that, and can later extend to support Avalonia if there is any demand for it.

Please let me know by creating an issue or a discussion in the project.