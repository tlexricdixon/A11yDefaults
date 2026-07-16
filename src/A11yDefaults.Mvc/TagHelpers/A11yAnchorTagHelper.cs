// Copyright (c) 2026 tlex.dev
// SPDX-License-Identifier: MIT

using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace A11yDefaults.Mvc.TagHelpers;

/// <summary>
/// Enhances anchor elements with accessible target sizing and contextual hints for
/// links that open new browsing contexts or trigger downloads.
/// </summary>
[HtmlTargetElement("a")]
public sealed class A11yAnchorTagHelper : TagHelper
{
    /// <summary>
    /// Package-wide defaults used when an anchor does not override behavior with
    /// an <c>a11y-*</c> attribute.
    /// </summary>
    private readonly A11yDefaultsOptions _defaults;

    /// <summary>
    /// Initializes a new instance of the <see cref="A11yAnchorTagHelper"/> class.
    /// </summary>
    /// <param name="defaults">The configured package defaults.</param>
    public A11yAnchorTagHelper(IOptions<A11yDefaultsOptions> defaults)
    {
        _defaults = defaults.Value;
    }

    /// <summary>
    /// Runs late so framework anchor generation and most consumer tag helpers have
    /// already produced final attributes before accessibility hints are calculated.
    /// </summary>
    public override int Order => 1000;

    /// <summary>
    /// Optional per-link target size override. When omitted, the configured
    /// <see cref="A11yDefaultsOptions.AnchorTargetSize"/> is used.
    /// </summary>
    [HtmlAttributeName("a11y-hit-target")]
    public A11yTargetSize? HitTarget { get; set; }

    /// <summary>
    /// Optional accessible label to write as <c>aria-label</c>. New-tab and
    /// download hints are appended to this label when applicable.
    /// </summary>
    [HtmlAttributeName("a11y-label")]
    public string? Label { get; set; }

    /// <summary>
    /// Renders hint text with the configured screen-reader-only class instead of
    /// the visible hint class.
    /// </summary>
    [HtmlAttributeName("a11y-sr-only-hint")]
    public bool ScreenReaderOnlyHint { get; set; }

    /// <summary>
    /// Optional per-link hint visibility override. When omitted, the configured
    /// <see cref="A11yDefaultsOptions.LinkHintVisibility"/> is used.
    /// </summary>
    [HtmlAttributeName("a11y-hint-visibility")]
    public A11yLinkHintVisibility? HintVisibility { get; set; }

    /// <summary>
    /// Suppresses the appended hint element while still allowing hints to be folded
    /// into <c>aria-label</c> when an accessible label is present.
    /// </summary>
    [HtmlAttributeName("a11y-suppress-hint")]
    public bool SuppressHint { get; set; }

    /// <summary>
    /// Optional per-link text used when <c>target="_blank"</c> is present.
    /// </summary>
    [HtmlAttributeName("a11y-new-tab-text")]
    public string? NewTabText { get; set; }

    /// <summary>
    /// Optional per-link text used when the <c>download</c> attribute is present.
    /// </summary>
    [HtmlAttributeName("a11y-download-text")]
    public string? DownloadText { get; set; }

    /// <summary>
    /// Applies target-size classes, normalizes package attributes out of the final
    /// markup, and appends contextual hint text when the anchor needs it.
    /// </summary>
    /// <param name="context">Contains information associated with the current HTML tag.</param>
    /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        RemovePackageAttributes(output);

        if (!string.IsNullOrWhiteSpace(Label))
        {
            output.Attributes.SetAttribute("aria-label", Label);
        }

        if (!HasHref(output))
        {
            return;
        }

        A11yTagHelperUtilities.ApplyTargetSizeClass(output, HitTarget ?? _defaults.AnchorTargetSize, _defaults);
        A11yTagHelperUtilities.ApplyFocusRingClass(output, _defaults);

        var hints = BuildHints(output);
        if (hints.Count == 0)
        {
            return;
        }

        var accessibleLabel = output.Attributes["aria-label"]?.Value?.ToString();
        if (!string.IsNullOrWhiteSpace(accessibleLabel))
        {
            output.Attributes.SetAttribute(
                "aria-label",
                A11yTagHelperUtilities.AppendHint(accessibleLabel, A11yTagHelperUtilities.CombineHints(hints)));
        }

        if (SuppressHint)
        {
            return;
        }

        var childContent = await output.GetChildContentAsync();
        output.Content.SetHtmlContent(childContent);
        output.Content.AppendHtml(
            A11yTagHelperUtilities.BuildHint(
                hintText: A11yTagHelperUtilities.CombineHints(hints),
                options: _defaults,
                srOnly: ResolveHintVisibility() == A11yLinkHintVisibility.ScreenReaderOnly,
                hideFromAccessibilityTree: !string.IsNullOrWhiteSpace(accessibleLabel)));
    }

    /// <summary>
    /// Resolves the effective hint visibility, preserving the existing
    /// <c>a11y-sr-only-hint</c> shorthand as a per-link override.
    /// </summary>
    /// <returns>The visibility to use for generated hint text.</returns>
    private A11yLinkHintVisibility ResolveHintVisibility()
    {
        if (HintVisibility.HasValue)
        {
            return HintVisibility.Value;
        }

        return ScreenReaderOnlyHint
            ? A11yLinkHintVisibility.ScreenReaderOnly
            : _defaults.LinkHintVisibility;
    }

    /// <summary>
    /// Builds human-readable hint text and applies the security-related
    /// <c>rel</c> tokens for links that open in a new browsing context.
    /// </summary>
    /// <param name="output">The anchor output being inspected and updated.</param>
    /// <returns>The link hints that should be announced or displayed.</returns>
    private List<string> BuildHints(TagHelperOutput output)
    {
        var hints = new List<string>();
        var target = output.Attributes["target"]?.Value?.ToString();
        var isBlankTarget = string.Equals(target, "_blank", StringComparison.OrdinalIgnoreCase);

        if (isBlankTarget)
        {
            var rel = A11yTagHelperUtilities.Tokenize(output.Attributes["rel"]?.Value?.ToString());
            rel.RemoveAll(static token => string.Equals(token, "opener", StringComparison.OrdinalIgnoreCase));

            if (_defaults.AddNoopenerToBlankTargets)
            {
                rel.Add("noopener");
            }

            if (_defaults.AddNoreferrerToBlankTargets)
            {
                rel.Add("noreferrer");
            }

            if (rel.Count > 0)
            {
                output.Attributes.SetAttribute("rel", string.Join(" ", rel.Distinct(StringComparer.OrdinalIgnoreCase)));
            }

            hints.Add(NewTabText ?? _defaults.NewTabText);
        }

        if (output.Attributes.ContainsName("download"))
        {
            hints.Add(DownloadText ?? _defaults.DownloadText);
        }

        return hints
            .Where(static hint => !string.IsNullOrWhiteSpace(hint))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    /// <summary>
    /// Removes authoring-only package attributes so rendered HTML stays valid and
    /// does not expose implementation details.
    /// </summary>
    /// <param name="output">The anchor output being cleaned.</param>
    private static void RemovePackageAttributes(TagHelperOutput output)
    {
        output.Attributes.RemoveAll("a11y-hit-target");
        output.Attributes.RemoveAll("a11y-label");
        output.Attributes.RemoveAll("a11y-sr-only-hint");
        output.Attributes.RemoveAll("a11y-hint-visibility");
        output.Attributes.RemoveAll("a11y-suppress-hint");
        output.Attributes.RemoveAll("a11y-new-tab-text");
        output.Attributes.RemoveAll("a11y-download-text");
    }

    /// <summary>
    /// Determines whether the final rendered anchor has an <c>href</c>, including
    /// values generated earlier by MVC's built-in anchor tag helper.
    /// </summary>
    /// <param name="output">The anchor output being inspected.</param>
    /// <returns><c>true</c> when the anchor should be treated as an interactive link.</returns>
    private static bool HasHref(TagHelperOutput output)
    {
        return !string.IsNullOrWhiteSpace(output.Attributes["href"]?.Value?.ToString());
    }
}
