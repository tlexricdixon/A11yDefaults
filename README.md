# MVC Package Direction

This folder contains the ASP.NET Core MVC version of the accessibility-defaults idea.

## Projects

- `src/A11yDefaults.Mvc`: the Razor Class Library / NuGet package
- `samples/A11yDefaults.Mvc.Sample`: a small MVC app showing the package in use
- `tests/A11yDefaults.Mvc.Tests`: a no-dependencies smoke-test console app

## Why this shape

For MVC work, the most natural package is:

1. Tag Helpers that enhance native HTML.
2. A small CSS asset shipped through static web assets.
3. Options you can register once in `Program.cs`.

That lets the package feel closer to a Telerik or Syncfusion baseline: install it, add the Tag Helpers, pull in the stylesheet, and get saner defaults without retraining everyone on a custom syntax.

## License

This repository is available under the MIT License.
