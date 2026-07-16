// Copyright (c) 2026 tlex.dev
// SPDX-License-Identifier: MIT

namespace A11yDefaults.Mvc;

/// <summary>
/// Controls whether generated link hint text is shown visually or only exposed
/// to assistive technology.
/// </summary>
public enum A11yLinkHintVisibility
{
    /// <summary>
    /// Render hint text visibly after the link text.
    /// </summary>
    Visible = 0,

    /// <summary>
    /// Render hint text with the configured screen-reader-only class.
    /// </summary>
    ScreenReaderOnly = 1
}
