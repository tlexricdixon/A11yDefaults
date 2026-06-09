// Copyright (c) 2026 tlex.dev
// SPDX-License-Identifier: MIT

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace A11yDefaults.Mvc.TagHelpers;

/// <summary>
/// Shared helpers for keeping tag-helper output consistent across anchors,
/// buttons, and button-like inputs.
/// </summary>
internal static class A11yTagHelperUtilities
{
    /// <summary>
    /// Adds the configured CSS class for the requested target size.
    /// </summary>
    /// <param name="output">The tag-helper output being modified.</param>
    /// <param name="targetSize">The target-size level requested by options or an authoring attribute.</param>
    /// <param name="options">Package options containing the CSS class names.</param>
    public static void ApplyTargetSizeClass(
        TagHelperOutput output,
        A11yTargetSize targetSize,
        A11yDefaultsOptions options)
    {
        switch (targetSize)
        {
            case A11yTargetSize.Minimum:
                AddCssClass(output, options.MinimumTargetClass);
                break;
            case A11yTargetSize.Enhanced:
                AddCssClass(output, options.EnhancedTargetClass);
                break;
        }
    }

    /// <summary>
    /// Adds the configured focus-ring class when automatic focus styling is
    /// enabled.
    /// </summary>
    /// <param name="output">The tag-helper output being modified.</param>
    /// <param name="options">Package options containing the CSS class name and opt-in flag.</param>
    public static void ApplyFocusRingClass(TagHelperOutput output, A11yDefaultsOptions options)
    {
        if (options.AddFocusRingToInteractiveElements)
        {
            AddCssClass(output, options.FocusRingClass);
        }
    }

    /// <summary>
    /// Builds the parenthetical hint appended to links for new-tab and download
    /// affordances.
    /// </summary>
    /// <param name="hintText">The already-combined hint text to render.</param>
    /// <param name="options">Package options containing the CSS class names.</param>
    /// <param name="srOnly">Whether to use the configured screen-reader-only class.</param>
    /// <param name="hideFromAccessibilityTree">Whether the hint should be hidden from assistive technology because it is already present in <c>aria-label</c>.</param>
    /// <returns>HTML content for the hint span.</returns>
    public static IHtmlContent BuildHint(
        string hintText,
        A11yDefaultsOptions options,
        bool srOnly,
        bool hideFromAccessibilityTree)
    {
        var span = new TagBuilder("span");
        span.AddCssClass(srOnly ? options.ScreenReaderClass : options.HintClass);

        if (hideFromAccessibilityTree)
        {
            span.Attributes["aria-hidden"] = "true";
        }

        span.InnerHtml.Append($" ({hintText})");
        return span;
    }

    /// <summary>
    /// Appends hint text to an accessible label unless the label already includes it.
    /// </summary>
    /// <param name="label">The current accessible label.</param>
    /// <param name="hintText">The hint text to append.</param>
    /// <returns>The label with the hint included once.</returns>
    public static string AppendHint(string label, string hintText)
    {
        return label.Contains(hintText, StringComparison.OrdinalIgnoreCase)
            ? label
            : $"{label} ({hintText})";
    }

    /// <summary>
    /// Combines multiple hints into the phrase used in visible text and
    /// <c>aria-label</c> values.
    /// </summary>
    /// <param name="hints">The hint strings to combine.</param>
    /// <returns>A comma-separated hint phrase.</returns>
    public static string CombineHints(IEnumerable<string> hints)
    {
        return string.Join(", ", hints);
    }

    /// <summary>
    /// Splits a space-separated HTML token list, such as <c>rel</c> or
    /// <c>class</c>, into unique tokens.
    /// </summary>
    /// <param name="value">The attribute value to tokenize.</param>
    /// <returns>A case-insensitive, de-duplicated token list.</returns>
    public static List<string> Tokenize(string? value)
    {
        return value?
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList()
            ?? [];
    }

    /// <summary>
    /// Adds a CSS class without duplicating existing classes or removing author
    /// supplied classes.
    /// </summary>
    /// <param name="output">The tag-helper output being modified.</param>
    /// <param name="className">The CSS class to add.</param>
    public static void AddCssClass(TagHelperOutput output, string className)
    {
        if (string.IsNullOrWhiteSpace(className))
        {
            return;
        }

        var classes = Tokenize(output.Attributes["class"]?.Value?.ToString());

        if (!classes.Contains(className, StringComparer.OrdinalIgnoreCase))
        {
            classes.Add(className);
        }

        output.Attributes.SetAttribute("class", string.Join(" ", classes));
    }
}
