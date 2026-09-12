// Copyright (c) 2026 tlex.dev
// SPDX-License-Identifier: MIT

using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

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
public sealed class A11yButtonTagHelper : TagHelper
{
    [HtmlAttributeName("a11y-variant")]
    public string Variant { get; set; } = "primary";

    [HtmlAttributeName("a11y-outline")]
    public bool Outline { get; set; }

    public override void Process(
        TagHelperContext context,
        TagHelperOutput output)
    {
        var allowedVariants = new HashSet<string>(
            StringComparer.OrdinalIgnoreCase)
        {
            "primary",
            "secondary",
            "success",
            "danger",
            "warning",
            "info",
            "light",
            "dark"
        };

        var variant = allowedVariants.Contains(Variant)
            ? Variant.ToLowerInvariant()
            : "primary";

        var bootstrapClass = Outline
            ? $"btn-outline-{variant}"
            : $"btn-{variant}";

        output.TagName = "button";
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Attributes.SetAttribute("type", "button");

        output.AddClass("btn", HtmlEncoder.Default);
        output.AddClass(bootstrapClass, HtmlEncoder.Default);
        output.AddClass("a11y-focus-ring", HtmlEncoder.Default);
        output.AddClass("a11y-target-min", HtmlEncoder.Default);
    }
}