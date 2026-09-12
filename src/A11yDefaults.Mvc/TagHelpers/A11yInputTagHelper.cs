// Copyright (c) 2026 tlex.dev
// SPDX-License-Identifier: MIT

using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace A11yDefaults.Mvc.TagHelpers;


/// <summary>
/// Adds accessible defaults to button-like <c>input</c> elements while leaving
/// text fields and other form controls untouched.
/// </summary>
[HtmlTargetElement("input", Attributes = "type")]
public sealed class A11yInputTagHelper : TagHelper
{
    /// <summary>
    /// Input types that behave like buttons and should receive hit-target
    /// treatment. Other input types have different accessibility requirements.
    /// </summary>
    private static readonly HashSet<string> SupportedTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "button",
        "submit",
        "reset"
    };

    /// <summary>
    /// Package-wide defaults used when an input does not provide an explicit
    /// <c>a11y-hit-target</c> value.
    /// </summary>
    private readonly A11yDefaultsOptions _defaults;

    /// <summary>
    /// Initializes a new instance of the <see cref="A11yInputTagHelper"/> class.
    /// </summary>
    /// <param name="defaults">The configured package defaults.</param>
    public A11yInputTagHelper(IOptions<A11yDefaultsOptions> defaults)
    {
        _defaults = defaults.Value;
    }

    /// <summary>
    /// Optional per-input target size override. When omitted, the configured
    /// <see cref="A11yDefaultsOptions.InputTargetSize"/> is used.
    /// </summary>
    [HtmlAttributeName("a11y-hit-target")]
    public A11yTargetSize? HitTarget { get; set; }

    /// <summary>
    /// Optional accessible label to write as <c>aria-label</c>.
    /// </summary>
    [HtmlAttributeName("a11y-label")]
    public string? Label { get; set; }

    /// <summary>
    /// Removes authoring-only attributes and applies defaults only when the
    /// rendered input type is button-like.
    /// </summary>
    /// <param name="context">Contains information associated with the current HTML tag.</param>
    /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.RemoveAll("a11y-hit-target");
        output.Attributes.RemoveAll("a11y-label");

        var type = output.Attributes["type"]?.Value?.ToString();
        if (string.IsNullOrWhiteSpace(type) || !SupportedTypes.Contains(type))
        {
            return;
        }

        A11yTagHelperUtilities.ApplyTargetSizeClass(output, HitTarget ?? _defaults.InputTargetSize, _defaults);
        A11yTagHelperUtilities.ApplyFocusRingClass(output, _defaults);

        if (!string.IsNullOrWhiteSpace(Label))
        {
            output.Attributes.SetAttribute("aria-label", Label);
        }
    }
}
