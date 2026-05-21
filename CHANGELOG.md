# Changelog

All notable changes to this project will be documented in this file.

This project follows the spirit of [Keep a Changelog](https://keepachangelog.com/) and uses semantic versioning for package releases.

## [Unreleased]

These changes are in the repository and are intended for the next package release.

### Added

- Added skip-link support through the `a11y-skip-link` Tag Helper.
- Added configurable skip-link defaults:
  - `SkipLinkText`
  - `SkipLinkTarget`
  - `SkipLinkClass`
- Added `.a11y-skip-link` CSS that stays hidden until keyboard focus reaches it.
- Added focus-ring support through the `.a11y-focus-ring` CSS utility.
- Added automatic focus-ring classes for anchors, buttons, and button-like inputs.
- Added `a11y-focus-ring` Tag Helper support for custom focusable elements.
- Added configurable focus-ring defaults:
  - `FocusRingClass`
  - `AddFocusRingToInteractiveElements`
- Added .NET 9 and .NET 10 target frameworks while keeping .NET 8 support.
- Added a Generic Government Agency sample page showing the package in a more realistic public-sector layout.
- Added AI usage disclosure language to clarify that AI tools helped during development, but no AI functionality ships in the package.

### Changed

- Updated package and sample project files to multi-target `net8.0`, `net9.0`, and `net10.0`.
- Updated README examples to use Razor enum expressions for `a11y-hit-target`, such as `@A11yTargetSize.Enhanced`.
- Updated the sample layout to demonstrate a skip link targeting the main content region.
- Updated smoke tests to cover skip links, focus rings, and multi-target execution.

### Fixed

- Fixed README sample markup for the enhanced target-size anchor example.
- Fixed visited-link contrast in the sample project navigation, footer, and action links.
- Fixed sample header and navigation link contrast by keeping generated links tied to the sample header foreground color.

## [1.0.0] - 2026-05-08

Initial public release of `A11yDefaults.Mvc`.

### Added

- Added ASP.NET Core MVC Tag Helpers for native anchors, buttons, and button-like inputs.
- Added target-size defaults for buttons and button-like inputs.
- Added opt-in target-size support for anchors with `a11y-hit-target`.
- Added new-tab link handling for anchors with `target="_blank"`.
- Added automatic `rel="noopener"` support for links that open a new browsing context.
- Added optional `rel="noreferrer"` support through package options.
- Added visible and screen-reader-only hint support for new-tab and download links.
- Added `a11y-label` support for anchors, buttons, and supported inputs.
- Added static CSS utilities for:
  - minimum target size
  - enhanced target size
  - link hints
  - screen-reader-only content
- Added a sample MVC application.
- Added smoke tests for core Tag Helper behavior.

### Notes

- The package had 89 downloads during its first 8 days live. That is a pretty good signal for a tiny accessibility helper package.
