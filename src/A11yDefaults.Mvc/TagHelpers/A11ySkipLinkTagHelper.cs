// Copyright (c) 2026 tlex.dev
// SPDX-License-Identifier: MIT

using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace A11yDefaults.Mvc.TagHelpers;

/// <summary>
/// Enhances native anchors into keyboard-friendly skip links for bypassing
/// repeated page chrome.
/// </summary>
[HtmlTargetElement("a", Attributes = "a11y-skip-link")]
public sealed class A11ySkipLinkTagHelper : TagHelper
{
    /// <summary>
    /// Package-wide defaults used when the skip link does not provide an explicit
    /// class, target, or text.
    /// </summary>
    private readonly A11yDefaultsOptions _defaults;

    /// <summary>
    /// Initializes a new instance of the <see cref="A11ySkipLinkTagHelper"/> class.
    /// </summary>
    /// <param name="defaults">The configured package defaults.</param>
    public A11ySkipLinkTagHelper(IOptions<A11yDefaultsOptions> defaults)
    {
        _defaults = defaults.Value;
    }

    /// <summary>
    /// Runs before the anchor helper so a skip link without an explicit
    /// <c>href</c> still renders as a normal anchor.
    /// </summary>
    public override int Order => -1000;

    /// <summary>
    /// Optional per-link target override. Values may be supplied with or without
    /// the leading hash.
    /// </summary>
    [HtmlAttributeName("a11y-skip-target")]
    public string? Target { get; set; }

    /// <summary>
    /// Optional per-link text rendered when the anchor has no child content.
    /// </summary>
    [HtmlAttributeName("a11y-skip-text")]
    public string? Text { get; set; }

    /// <summary>
    /// Removes authoring-only attributes, applies the configured skip-link class,
    /// and supplies a default <c>href</c> and label text when needed.
    /// </summary>
    /// <param name="context">Contains information associated with the current HTML tag.</param>
    /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.RemoveAll("a11y-skip-link");
        output.Attributes.RemoveAll("a11y-skip-target");
        output.Attributes.RemoveAll("a11y-skip-text");

        A11yTagHelperUtilities.AddCssClass(output, _defaults.SkipLinkClass);

        if (!output.Attributes.ContainsName("href"))
        {
            output.Attributes.SetAttribute("href", NormalizeFragment(Target ?? _defaults.SkipLinkTarget));
        }

        var childContent = await output.GetChildContentAsync();
        output.Content.SetHtmlContent(childContent);
        if (childContent.IsEmptyOrWhiteSpace)
        {
            output.Content.SetContent(Text ?? _defaults.SkipLinkText);
        }
    }

    /// <summary>
    /// Ensures a configured target is emitted as an HTML fragment identifier.
    /// </summary>
    /// <param name="target">The configured target value.</param>
    /// <returns>The target with a leading hash, or <c>#main</c> when empty.</returns>
    private static string NormalizeFragment(string? target)
    {
        if (string.IsNullOrWhiteSpace(target))
        {
            return "#main";
        }

        return target.StartsWith('#') ? target : $"#{target}";
    }
}
