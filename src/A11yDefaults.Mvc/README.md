# A11yDefaults.Mvc

`A11yDefaults.Mvc` is an ASP.NET Core MVC package that nudges everyday UI toward accessible defaults without replacing native HTML.

## Supported frameworks

- .NET 8
- .NET 9
- .NET 10

## Releases

Release notes and package history are tracked in the repository [CHANGELOG.md](https://github.com/tlexricdixon/A11yDefaults/blob/master/CHANGELOG.md).

## What it does

- Adds minimum target-size classes to `<button>` and button-like `<input>` elements.
- Enhances MVC-generated anchors after `asp-controller`, `asp-action`, and related attributes produce the final `href`.
- Renders keyboard-friendly skip links that reveal themselves on focus.
- Adds a consistent keyboard focus indicator to enhanced interactive elements.
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
    options.AddFocusRingToInteractiveElements = true;
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
<a a11y-skip-link></a>

<main id="main" tabindex="-1">
    ...
</main>

<button class="icon-button" aria-label="Refresh"></button>

<a href="https://www.w3.org/WAI/"
   target="_blank">
    W3C WAI guidance
</a>

<a href="/reports/q2.pdf"
   download
   a11y-hit-target="@A11yTargetSize.Enhanced">
    Download report
</a>

<a asp-controller="Reports"
   asp-action="Index"
   a11y-hit-target="@A11yTargetSize.Minimum">
    Reports
</a>
```

Use the Razor enum expression form for target-size overrides:
`a11y-hit-target="@A11yTargetSize.Enhanced"` or
`a11y-hit-target="@A11yTargetSize.Minimum"`.

Anchors, buttons, and button-like inputs receive the package focus-ring class by
default. For custom focusable elements, opt in directly:

```cshtml
<span tabindex="0" role="button" a11y-focus-ring>
    Custom focusable item
</span>
```

Skip links default to `href="#main"` and `Skip to main content`. Override either per link:

```cshtml
<a a11y-skip-link
   a11y-skip-target="content"
   a11y-skip-text="Jump to content"></a>
```

## WCAG defaults baked in

- WCAG 2.2 SC 2.5.8 Target Size (Minimum): 24 by 24 CSS pixels for AA.
- WCAG 2.2 SC 2.5.5 Target Size (Enhanced): 44 by 44 CSS pixels for AAA.
- WCAG 2.2 focus appearance guidance for visible keyboard focus indicators.
- WAI guidance for skip links that let keyboard users bypass repeated navigation.
- WAI guidance for links that open a new browsing context.

This package is a useful baseline, not a substitute for testing real pages with keyboard, zoom, screen readers, and touch.

## AI usage disclosure

This project was developed with help from AI-based coding tools during implementation and review.

That is a development-time detail only. The package itself does not include AI models, does not call AI services, does not process user data with AI, and does not require any AI functionality at runtime. The shipped behavior is deterministic .NET code, Razor Tag Helpers, and static CSS.

No application data, production data, credentials, secrets, or private user records are required by this package or transmitted to AI services by this package.

## License

`A11yDefaults.Mvc` is available under the MIT License.
