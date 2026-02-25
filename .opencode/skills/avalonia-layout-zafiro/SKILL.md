---
name: avalonia-layout-zafiro
description: Guidelines for modern Avalonia UI layout using Zafiro.Avalonia, emphasizing shared styles, generic components, and avoiding XAML redundancy.
---

# Avalonia Layout with Zafiro.Avalonia

> Master modern, clean, and maintainable Avalonia UI layouts.
> **Focus on semantic containers, shared styles, and minimal XAML.**
> Source: https://github.com/SuperJMN/skills/tree/master/avalonia-layout-zafiro

## Selective Reading Rule

**Read ONLY files relevant to the layout challenge!**

---

## Checklist for Clean Layouts

- [ ] **Used semantic containers?** (e.g., `HeaderedContainer` instead of `Border` with manual header)
- [ ] **Avoided redundant properties?** Use shared styles in `axaml` files.
- [ ] **Minimized nesting?** Flatten layouts using `EdgePanel` or generic components.
- [ ] **Icons via extension?** Use `{Icon fa-name}` and `IconOptions` for styling.
- [ ] **Behaviors over code-behind?** Use `Interaction.Behaviors` for UI-logic.
- [ ] **Avoided Converters?** Prefer ViewModel properties or Behaviors unless necessary.

---

## Anti-Patterns

**DON'T:**

- Use hardcoded colors or sizes (literals) in views.
- Create deep nesting of `Grid` and `StackPanel`.
- Add local properties before checking what styles already apply.
- Repeat visual properties across multiple elements (use Styles).
- Use `IValueConverter` for simple logic that belongs in the ViewModel.
- When a button has an icon, don't do this:

  ```xml
  <EnhancedButton Command="{Binding Invest}" DockPanel.Dock="Right" Classes="Outline">
      <StackPanel Orientation="Horizontal" Spacing="10">
          <TextBlock Text="icon" />
          <TextBlock Text="Invest Now" />
      </StackPanel>
  </EnhancedButton>
  ```
  Instead, go with:
  ```xml
  <EnhancedButton Icon="{Icon fa-chart-line}" Content="Invest Now" />
  ```

**DO:**
- Use `DynamicResource` for colors and brushes.
- Extract repeated layouts into generic components.
- Leverage `Zafiro.Avalonia` specific panels like `EdgePanel` for common UI patterns.

---

## Method: Discovering & Applying Styles

**Principle:** Never invent class names (e.g., `H1`, `Card`, `Caption`). Instead, **scan the project resources** to find the definitions.

### 1. Locate the Theme/Style Definitions
Search for the theme configuration. Common locations:
- `App.axaml`: Often contains `<StyleInclude>` for the main theme.
- `Themes/` or `Styles/` directories: Look for `.axaml` files.
- `Styles.axaml`: In library projects.

### 2. Scan for Defined Classes
Open the relevant `.axaml` files (e.g., `Typography.axaml`, `Buttons.axaml`, `Containers.axaml`) and search for `Style Selector`.

**Pattern to look for:**
```xml
<Style Selector="TextBlock.Title"> ... </Style>
<Style Selector="Border.Panel"> ... </Style>
<Style Selector="HeaderedContainer.WizardSection"> ... </Style>
```
*The part after the dot (e.g., `Title`, `Panel`) is the Class name.*

### 3. Verify & Apply
Once you confirm the class exists, apply it using the `Classes` property.

```xml
<!-- CORRECT: Verified class 'Title' exists in Typography.axaml -->
<TextBlock Classes="Title" Text="My Header" />

<!-- INCORRECT: Guessing a class name -->
<TextBlock Classes="H1" Text="My Header" />
```

### 4. Check for Resources (Keys)
If you can't find a class, look for `ControlTheme` or `StaticResource` keys in `ResourceDictionary`.

```xml
<!-- Finding the key -->
<ControlTheme x:Key="TransparentButton" ... />

<!-- Applying it -->
<EnhancedButton Theme="{StaticResource TransparentButton}" ... />
```

### 5. Apply TextBlock Classes to Headers (No Style Chaining)
When a header is just a string (e.g., `Header="Project Statistics"`), you cannot "apply a style to another style".
Wrap the header with a `HeaderTemplate` and use atomic classes on a `TextBlock`.

```xml
<Style Selector="HeaderedContainer.Big">
  <Setter Property="HeaderTemplate">
    <DataTemplate>
      <TextBlock Classes="Size-XL Weight-Bold" Text="{Binding}" />
    </DataTemplate>
  </Setter>
</Style>
```

---

## Theme Organization and Shared Styles

Efficient theme organization is key to avoiding redundant XAML and ensuring visual consistency.

### Structure

Follow this pattern:

1. **Colors & Brushes**: Define in a dedicated `Colors.axaml`. Use `DynamicResource` to support theme switching.
2. **Styles**: Group styles by category (e.g., `Buttons.axaml`, `Containers.axaml`, `Typography.axaml`).
3. **App-wide Theme**: Aggregate all styles in a main `Theme.axaml`.

### Avoiding Redundancy

Instead of setting properties directly on elements:

```xml
<!-- BAD: Redundant properties -->
<HeaderedContainer CornerRadius="10" BorderThickness="1" BorderBrush="Blue" Background="LightBlue" />
<HeaderedContainer CornerRadius="10" BorderThickness="1" BorderBrush="Blue" Background="LightBlue" />

<!-- GOOD: Use Classes and Styles -->
<HeaderedContainer Classes="BlueSection" />
<HeaderedContainer Classes="BlueSection" />
```

Define the style in a shared `axaml` file:

```xml
<Style Selector="HeaderedContainer.BlueSection">
    <Setter Property="CornerRadius" Value="10" />
    <Setter Property="BorderThickness" Value="1" />
    <Setter Property="BorderBrush" Value="{DynamicResource Accent}" />
    <Setter Property="Background" Value="{DynamicResource SurfaceSubtle}" />
</Style>
```

### Shared Icons and Resources

Centralize icon definitions and other shared resources in `Icons.axaml` and include them in the `MergedDictionaries` of your theme or `App.axaml`.

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <MergeResourceInclude Source="UI/Themes/Styles/Containers.axaml" />
            <MergeResourceInclude Source="UI/Shared/Resources/Icons.axaml" />
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

---

## Semantic Containers

Using the right container for the data type simplifies XAML and improves maintainability. `Zafiro.Avalonia` provides specialized controls for common layout patterns.

### HeaderedContainer

Prefer `HeaderedContainer` over a `Border` or `Grid` when a section needs a title or header.

```xml
<HeaderedContainer Header="Security Settings" Classes="WizardSection">
    <StackPanel>
        <!-- Content here -->
    </StackPanel>
</HeaderedContainer>
```

Key Properties:
- `Header`: The content or string for the header.
- `HeaderBackground`: Brush for the header area.
- `ContentPadding`: Padding for the content area.

### EdgePanel

Use `EdgePanel` to position elements at the edges of a container without complex `Grid` definitions.

```xml
<EdgePanel StartContent="{Icon fa-wallet}"
           Content="Wallet Balance"
           EndContent="$1,234.00" />
```

Slots:
- `StartContent`: Aligned to the left (or beginning).
- `Content`: Fills the remaining space in the middle.
- `EndContent`: Aligned to the right (or end).

### Card

A simple container for grouping related information, often used inside `HeaderedContainer` or as a standalone element in a list.

```xml
<Card Header="Enter recipient address:">
    <TextBox Text="{Binding Address}" />
</Card>
```

### Best Practices

- Use `Classes` to apply themed variants (e.g., `Classes="Section"`, `Classes="Highlight"`).
- Customize internal parts of the containers using templates in your styles when necessary, rather than nesting more controls.

---

## Icon Usage

`Zafiro.Avalonia` simplifies icon management using a specialized markup extension and styling options.

### IconExtension

Use the `{Icon}` markup extension to easily include icons from libraries like FontAwesome.

```xml
<!-- Positional parameter -->
<Button Content="{Icon fa-wallet}" />

<!-- Named parameter -->
<ContentControl Content="{Icon Source=fa-gear}" />
```

### IconOptions

`IconOptions` allows you to customize icons without manually wrapping them in other controls. It's often used in styles to provide a consistent look.

```xml
<Style Selector="HeaderedContainer /template/ ContentPresenter#Header EdgePanel /template/ ContentControl#StartContent">
    <Setter Property="IconOptions.Size" Value="20" />
    <Setter Property="IconOptions.Fill" Value="{DynamicResource Accent}" />
    <Setter Property="IconOptions.Padding" Value="10" />
    <Setter Property="IconOptions.CornerRadius" Value="10" />
</Style>
```

Common Properties:
- `IconOptions.Size`: Sets the width and height of the icon.
- `IconOptions.Fill`: The color/brush of the icon.
- `IconOptions.Background`: Background brush for the icon container.
- `IconOptions.Padding`: Padding inside the icon container.
- `IconOptions.CornerRadius`: Corner radius if a background is used.

### Shared Icon Resources

Define icons as resources for reuse across the application.

```xml
<ResourceDictionary xmlns="https://github.com/avaloniaui">
    <Icon x:Key="fa-wallet" Source="fa-wallet" />
</ResourceDictionary>
```

The `{Icon ...}` extension is usually preferred for its brevity and ability to create new icon instances on the fly.

---

## Interactions and Logic

To keep XAML clean and maintainable, minimize logic in views and avoid excessive use of converters.

### Xaml.Interaction.Behaviors

Use `Interaction.Behaviors` to handle UI-related logic that doesn't belong in the ViewModel, such as focus management, animations, or specialized event handling.

```xml
<TextBox Text="{Binding Address}">
    <Interaction.Behaviors>
        <UntouchedClassBehavior />
    </Interaction.Behaviors>
</TextBox>
```

Why use Behaviors?
- **Encapsulation**: UI logic is contained in a reusable behavior class.
- **Clean XAML**: Avoids code-behind and complex XAML triggers.
- **Testability**: Behaviors can be tested independently of the View.

### Avoiding Converters

Converters often lead to "magical" logic hidden in XAML. Whenever possible, prefer:

1. **ViewModel Properties**: Let the ViewModel provide the final data format (e.g., a `string` formatted for display).
2. **MultiBinding**: Use for simple logic combinations (And/Or) directly in XAML.
3. **Behaviors**: For more complex interactions that involve state or events.

When to use Converters? Only when the conversion is purely visual and highly reusable across different contexts (e.g., `BoolToOpacityConverter`).

---

## Building Generic Components

Reducing nesting and complexity is achieved by breaking down views into generic, reusable components.

### Generic Components

Instead of building large, complex views, extract recurring patterns into small `UserControl`s.

Instead of repeating a `Grid` with labels and values:

```xml
<!-- BAD: Repeated Grid -->
<Grid ColumnDefinitions="*,Auto">
   <TextBlock Text="Total:" />
   <TextBlock Grid.Column="1" Text="{Binding Total}" />
</Grid>
```

Create a generic component (or use `EdgePanel` with a Style):

```xml
<!-- GOOD: Use a specialized control or style -->
<EdgePanel StartContent="Total:" EndContent="{Binding Total}" Classes="SummaryItem" />
```

### Flattening Layouts

Avoid deep nesting. Deeply nested XAML is hard to read and can impact performance.

- **StackPanel vs Grid**: Use `StackPanel` (with `Spacing`) for simple linear layouts.
- **EdgePanel**: Great for "Label - Value" or "Icon - Text - Action" rows.
- **UniformGrid**: Use for grids where all cells are the same size.

### Component Granularity

- **Atomical**: Small controls like custom buttons or icons.
- **Molecular**: Groups of atoms like a `HeaderedContainer` with specific content.
- **Organisms**: Higher-level sections of a page.

Aim for components that are generic enough to be reused but specific enough to simplify the parent view significantly.

---

## Common "Zafiro-like" Patterns (Verify first!)

While every project is different, Zafiro-based projects often use these semantic patterns. **Scan files to confirm**:

* **Typography**: Look for `Title`, `Subtitle`, `Header`, `Caption`, or atomic sizes (`Size-S`, `Size-L`) and weights (`Weight-Bold`).
* **Containers**: Look for `Panel` (often a Border with shadow/bg) or `Section` styles for `HeaderedContainer`.
* **Buttons**: Look for semantic roles (`Primary`, `Secondary`) or visual styles (`Outline`, `Ghost`/`Transparent`).
