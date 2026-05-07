// Copyright (c) 2026 tlex.dev
// SPDX-License-Identifier: MIT

using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace A11yDefaults.Mvc.TagHelpers;

/// <summary>
/// Adds accessible defaults to native <c>button</c> elements without changing the
/// element type or requiring a custom component model.
/// </summary>
[HtmlTargetElement("button")]
public sealed class A11yButtonTagHelper : TagHelper
{
    /// <summary>
    /// Package-wide defaults used when a button does not provide an explicit
    /// <c>a11y-hit-target</c> value.
    /// </summary>
    private readonly A11yDefaultsOptions _defaults;

    /// <summary>
    /// Initializes a new instance of the <see cref="A11yButtonTagHelper"/> class.
    /// </summary>
    /// <param name="defaults">The configured package defaults.</param>
    public A11yButtonTagHelper(IOptions<A11yDefaultsOptions> defaults)
    {
        _defaults = defaults.Value;
    }

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
    /// Removes authoring-only attributes and applies the configured accessibility
    /// defaults to the rendered button.
    /// </summary>
    /// <param name="context">Contains information associated with the current HTML tag.</param>
    /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.RemoveAll("a11y-hit-target");
        output.Attributes.RemoveAll("a11y-label");

        A11yTagHelperUtilities.ApplyTargetSizeClass(output, HitTarget ?? _defaults.ButtonTargetSize, _defaults);

        if (!string.IsNullOrWhiteSpace(Label))
        {
            output.Attributes.SetAttribute("aria-label", Label);
        }
    }
}
