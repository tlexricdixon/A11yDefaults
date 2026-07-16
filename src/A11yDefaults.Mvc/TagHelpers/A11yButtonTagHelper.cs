// Copyright (c) 2026 tlex.dev
// SPDX-License-Identifier: MIT

using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace A11yDefaults.Mvc.TagHelpers;

/// <summary>
/// Renders the package's <c>a11y-button</c> authoring element as an accessible
/// native button with self-contained A11yDefaults styling.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="A11yButtonTagHelper"/> class.
/// </remarks>
/// <param name="defaults">The configured package defaults.</param>
[HtmlTargetElement("a11y-button")]
[OutputElementHint("button")]
public sealed class A11yButtonTagHelper(IOptions<A11yDefaultsOptions> defaults) : HelperTagsBase(defaults)
{
    /// <summary>
    /// Optional per-button target size override. When omitted, the configured
    /// <see cref="A11yDefaultsOptions.ButtonTargetSize"/> is used.
    /// </summary>
    [HtmlAttributeName("a11y-hit-target")]
    public A11yTargetSize? HitTarget { get; set; }

    /// <summary>
    /// Optional accessible label to write as <c>aria-label</c>, useful for icon-only
    /// buttons.
    /// </summary>
    [HtmlAttributeName("a11y-label")]
    public string? Label { get; set; }

    /// <summary>
    /// A11yDefaults button variant (primary, secondary, success, danger, warning, info, light, dark).
    /// Defaults to "primary" if not specified.
    /// </summary>
    [HtmlAttributeName("a11y-variant")]
    public string Variant { get; set; } = "primary";

    /// <summary>
    /// A11yDefaults button size (sm, lg). When omitted, standard size is used.
    /// </summary>
    [HtmlAttributeName("a11y-size")]
    public string? Size { get; set; }

    /// <summary>
    /// When true, renders the button in a disabled state with appropriate ARIA attributes.
    /// </summary>
    [HtmlAttributeName("a11y-disabled")]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// When true, renders the button in a loading state and disables interaction.
    /// Useful for async operations.
    /// </summary>
    [HtmlAttributeName("a11y-loading")]
    public bool IsLoading { get; set; }

    /// <summary>
    /// Optional loading indicator text to append when <c>a11y-loading</c> is true.
    /// </summary>
    [HtmlAttributeName("a11y-loading-text")]
    public string? LoadingText { get; set; }

    /// <summary>
    /// When true, applies the A11yDefaults outline treatment.
    /// </summary>
    [HtmlAttributeName("a11y-outline")]
    public bool IsOutline { get; set; }

    /// <summary>
    /// Removes authoring-only attributes and applies the configured accessibility
    /// and component defaults to the rendered button.
    /// </summary>
    /// <param name="context">Contains information associated with the current HTML tag.</param>
    /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "button";

        // Remove authoring-only attributes
        output.Attributes.RemoveAll("a11y-hit-target");
        output.Attributes.RemoveAll("a11y-label");
        output.Attributes.RemoveAll("a11y-variant");
        output.Attributes.RemoveAll("a11y-size");
        output.Attributes.RemoveAll("a11y-disabled");
        output.Attributes.RemoveAll("a11y-loading");
        output.Attributes.RemoveAll("a11y-loading-text");
        output.Attributes.RemoveAll("a11y-outline");

        // Apply component-owned button classes. Native button elements are handled
        // separately and never receive these classes automatically.
        var variant = IsOutline ? $"a11y-btn-outline-{Variant}" : $"a11y-btn-{Variant}";
        A11yTagHelperUtilities.AddCssClass(output, "a11y-btn");
        A11yTagHelperUtilities.AddCssClass(output, variant);

        // Apply size class if specified
        if (!string.IsNullOrWhiteSpace(Size))
        {
            A11yTagHelperUtilities.AddCssClass(output, $"a11y-btn-{Size}");
        }

        // Apply accessibility classes
        A11yTagHelperUtilities.ApplyTargetSizeClass(output, HitTarget ?? Options.ButtonTargetSize, Options);
        A11yTagHelperUtilities.ApplyFocusRingClass(output, Options);

        // Apply disabled state
        if (IsDisabled || IsLoading)
        {
            output.Attributes.SetAttribute("disabled", "disabled");
            output.Attributes.SetAttribute("aria-disabled", "true");
        }

        // Apply loading state
        if (IsLoading)
        {
            output.Attributes.SetAttribute("aria-busy", "true");

            if (!string.IsNullOrWhiteSpace(LoadingText))
            {
                output.Attributes.SetAttribute("data-loading-text", LoadingText);
            }
        }

        // Apply accessible label
        if (!string.IsNullOrWhiteSpace(Label))
        {
            output.Attributes.SetAttribute("aria-label", Label);
        }
    }
}
