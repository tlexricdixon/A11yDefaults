// Copyright (c) 2026 tlex.dev
// SPDX-License-Identifier: MIT

using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace A11yDefaults.Mvc.TagHelpers;

/// <summary>
/// Applies the package focus indicator class to custom focusable elements.
/// </summary>
[HtmlTargetElement(Attributes = "a11y-focus-ring")]
public sealed class A11yFocusRingTagHelper : TagHelper
{
    /// <summary>
    /// Package-wide defaults containing the focus-ring class name.
    /// </summary>
    private readonly A11yDefaultsOptions _defaults;

    /// <summary>
    /// Initializes a new instance of the <see cref="A11yFocusRingTagHelper"/> class.
    /// </summary>
    /// <param name="defaults">The configured package defaults.</param>
    public A11yFocusRingTagHelper(IOptions<A11yDefaultsOptions> defaults)
    {
        _defaults = defaults.Value;
    }

    /// <summary>
    /// Removes the authoring-only attribute and applies the configured focus-ring
    /// class.
    /// </summary>
    /// <param name="context">Contains information associated with the current HTML tag.</param>
    /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.RemoveAll("a11y-focus-ring");
        A11yTagHelperUtilities.AddCssClass(output, _defaults.FocusRingClass);
    }
}
