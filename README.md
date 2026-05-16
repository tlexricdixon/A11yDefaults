# MVC Package Direction

This folder contains the ASP.NET Core MVC version of the accessibility-defaults idea.

## Supported frameworks

The MVC package targets .NET 8, .NET 9, and .NET 10.

## Projects

- `src/A11yDefaults.Mvc`: the Razor Class Library / NuGet package
- `samples/A11yDefaults.Mvc.Sample`: a small MVC app showing the package in use
- `tests/A11yDefaults.Mvc.Tests`: a no-dependencies smoke-test console app

## Releases

Release notes and package history are tracked in [CHANGELOG.md](CHANGELOG.md).

## Why this shape

For MVC work, the most natural package is:

1. Tag Helpers that enhance native HTML.
2. A small CSS asset shipped through static web assets.
3. Options you can register once in `Program.cs`.

That lets the package feel closer to a Telerik or Syncfusion baseline: install it, add the Tag Helpers, pull in the stylesheet, and get saner defaults without retraining everyone on a custom syntax.

Current MVC defaults cover:

- Target-size classes for buttons, button-like inputs, and opt-in anchors.
- Focus-ring classes for keyboard-visible interactive states.
- Safer `target="_blank"` links with visible or screen-reader-only hints.
- Skip links for bypassing repeated navigation with keyboard focus.

## AI usage disclosure

This project was developed with help from AI-based coding tools during implementation and review.

That is a development-time detail only. The package itself does not include AI models, does not call AI services, does not process user data with AI, and does not require any AI functionality at runtime. The shipped behavior is deterministic .NET code, Razor Tag Helpers, and static CSS.

No application data, production data, credentials, secrets, or private user records are required by this package or transmitted to AI services by this package.

## License

This repository is available under the MIT License.
