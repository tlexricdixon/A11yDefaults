# A11yDefaults.Mvc

`A11yDefaults.Mvc` is an ASP.NET Core MVC package that nudges everyday UI toward accessible defaults without replacing native HTML.

## What it does

- Adds minimum target-size classes to `<button>` and button-like `<input>` elements.
- Makes `target="_blank"` links safer and clearer by adding `rel="noopener"` and a user-facing hint.
- Lets you opt specific anchors into minimum or enhanced target sizes.
- Keeps the authoring model native: you still write `<a>`, `<button>`, and `<input>`.

## Quick start

Install the package:

```dotnetcli
dotnet add package A11yDefaults.Mvc
```

Register the options:

```csharp
builder.Services.AddA11yDefaults(options =>
{
    options.ButtonTargetSize = A11yTargetSize.Minimum;
});
```

Enable the Tag Helpers:

```cshtml
@addTagHelper *, A11yDefaults.Mvc
```

Load the bundled CSS:

```cshtml
<link rel="stylesheet"
      href="~/_content/A11yDefaults.Mvc/a11y-defaults.css"
      asp-append-version="true" />
```

Then use native elements:

```cshtml
<button class="icon-button" aria-label="Refresh"></button>

<a href="https://www.w3.org/WAI/"
   target="_blank">
    W3C WAI guidance
</a>

<a href="/reports/q2.pdf"
   download
   a11y-hit-target="Enhanced">
    Download report
</a>
```

## WCAG defaults baked in

- WCAG 2.2 SC 2.5.8 Target Size (Minimum): 24 by 24 CSS pixels for AA.
- WCAG 2.2 SC 2.5.5 Target Size (Enhanced): 44 by 44 CSS pixels for AAA.
- WAI guidance for links that open a new browsing context.

This package is a useful baseline, not a substitute for testing real pages with keyboard, zoom, screen readers, and touch.

## Package contents

- Tag Helpers for anchors, buttons, and button-like inputs.
- Static web asset stylesheet at `_content/A11yDefaults.Mvc/a11y-defaults.css`.
- Configurable class names and hint text through `A11yDefaultsOptions`.

## License

`A11yDefaults.Mvc` is available under the MIT License.
