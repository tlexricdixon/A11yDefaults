// Copyright (c) 2026 tlex.dev
// SPDX-License-Identifier: MIT

namespace A11yDefaults.Mvc;

/// <summary>
/// Configures the CSS classes, target-size defaults, and link hint text used by
/// the package tag helpers.
/// </summary>
public sealed class A11yDefaultsOptions
{
    /// <summary>
    /// Default CSS class for controls that should meet the minimum target-size
    /// threshold.
    /// </summary>
    public const string MinimumTargetClassName = "a11y-target-min";

    /// <summary>
    /// Default CSS class for controls that should use the larger enhanced target
    /// size.
    /// </summary>
    public const string EnhancedTargetClassName = "a11y-target-enhanced";

    /// <summary>
    /// Default CSS class for visible link hint text.
    /// </summary>
    public const string HintClassName = "a11y-link-hint";

    /// <summary>
    /// Default CSS class for content that should remain available to assistive
    /// technology while being visually hidden.
    /// </summary>
    public const string ScreenReaderClassName = "a11y-sr-only";

    /// <summary>
    /// Default CSS class for skip links that become visible when focused.
    /// </summary>
    public const string SkipLinkClassName = "a11y-skip-link";

    /// <summary>
    /// Default CSS class for controls that should receive the package focus
    /// indicator.
    /// </summary>
    public const string FocusRingClassName = "a11y-focus-ring";

    /// <summary>
    /// Gets or sets the default target-size treatment for <c>button</c> elements.
    /// </summary>
    public A11yTargetSize ButtonTargetSize { get; set; } = A11yTargetSize.Minimum;

    /// <summary>
    /// Gets or sets the default target-size treatment for button-like
    /// <c>input</c> elements.
    /// </summary>
    public A11yTargetSize InputTargetSize { get; set; } = A11yTargetSize.Minimum;

    /// <summary>
    /// Gets or sets the default target-size treatment for anchors.
    /// </summary>
    public A11yTargetSize AnchorTargetSize { get; set; } = A11yTargetSize.None;

    /// <summary>
    /// Gets or sets the hint text appended to links that open in a new tab or
    /// window.
    /// </summary>
    public string NewTabText { get; set; } = "opens in new tab";

    /// <summary>
    /// Gets or sets the hint text appended to links with the <c>download</c>
    /// attribute.
    /// </summary>
    public string DownloadText { get; set; } = "downloads file";

    /// <summary>
    /// Gets or sets the default text rendered by skip links when no child content
    /// is supplied.
    /// </summary>
    public string SkipLinkText { get; set; } = "Skip to main content";

    /// <summary>
    /// Gets or sets the default fragment target used by skip links when no
    /// <c>href</c> is supplied.
    /// </summary>
    public string SkipLinkTarget { get; set; } = "#main";

    /// <summary>
    /// Gets or sets whether <c>rel="noopener"</c> is added to
    /// <c>target="_blank"</c> links.
    /// </summary>
    public bool AddNoopenerToBlankTargets { get; set; } = true;

    /// <summary>
    /// Gets or sets whether <c>rel="noreferrer"</c> is added to
    /// <c>target="_blank"</c> links.
    /// </summary>
    public bool AddNoreferrerToBlankTargets { get; set; }

    /// <summary>
    /// Gets or sets the CSS class used for minimum target-size treatment.
    /// </summary>
    public string MinimumTargetClass { get; set; } = MinimumTargetClassName;

    /// <summary>
    /// Gets or sets the CSS class used for enhanced target-size treatment.
    /// </summary>
    public string EnhancedTargetClass { get; set; } = EnhancedTargetClassName;

    /// <summary>
    /// Gets or sets the CSS class used for visible link hint text.
    /// </summary>
    public string HintClass { get; set; } = HintClassName;

    /// <summary>
    /// Gets or sets the CSS class used for visually hidden screen-reader text.
    /// </summary>
    public string ScreenReaderClass { get; set; } = ScreenReaderClassName;

    /// <summary>
    /// Gets or sets the CSS class used for skip links.
    /// </summary>
    public string SkipLinkClass { get; set; } = SkipLinkClassName;

    /// <summary>
    /// Gets or sets the CSS class used for the package focus indicator.
    /// </summary>
    public string FocusRingClass { get; set; } = FocusRingClassName;

    /// <summary>
    /// Gets or sets whether anchors, buttons, and button-like inputs receive the
    /// package focus indicator class automatically.
    /// </summary>
    public bool AddFocusRingToInteractiveElements { get; set; } = true;
}
