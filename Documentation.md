# WPFComponents

- [1 Introduction](#1-introduction)
- [2 API](#2-api)
  - [2.1 ImageButton](#21-imagebutton)
  - [2.2 NumericUpDown](#22-numericupdown)

## 1 Introduction

The `WPFComponents` library offers utility user controls for WPF projects.

## 2 API

### 2.1 ImageButton

> A button that displays an icon instead of text.
>
> The `ImageButton` can be enabled and disabled. When enabled the `EnabledIconSource` will be used. The `DisabledIconSource` will be used otherwise.
>
> Also a `Tooltip` and the `Size` of the `ImageButton` can be configured.

Accessible Interface:

```c#
public event RoutedEventHandler Click

public object Tooltip
public double Size
public ImageSource EnabledIconSource
public ImageSource DisabledIconSource

public ImageButton()
```

### 2.2 NumericUpDown

> An input control for integer values.
>
> Optionally a `Min` and `Max` can be configured as well as a `MaxLength` for the string length of the current `Value`.
>
> The value can be changed by either inserting a string into the control, by clicking the up or down buttons or by scrolling above the input field, which will change the `Value` by `StepPerScroll`.

Accessible Interface:

```c#
public event ValueChangeHandler ValueChanged

public int Value
public int? Min
public int? Max
public uint? MaxLength
public uint StepPerScroll

public NumericUpDown()

public void SetValue
public void SetMin
public void SetMax
```
