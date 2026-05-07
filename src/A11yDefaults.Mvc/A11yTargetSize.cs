// Copyright (c) 2026 tlex.dev
// SPDX-License-Identifier: MIT

namespace A11yDefaults.Mvc;

/// <summary>
/// Target-size levels that map to the package's WCAG-oriented CSS classes.
/// </summary>
public enum A11yTargetSize
{
    /// <summary>
    /// Do not add a target-size class.
    /// </summary>
    None = 0,

    /// <summary>
    /// Add the class configured for the WCAG 2.2 Target Size (Minimum) threshold.
    /// </summary>
    Minimum = 1,

    /// <summary>
    /// Add the class configured for the larger enhanced target-size threshold.
    /// </summary>
    Enhanced = 2
}
