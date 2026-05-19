# A11yDefaults

Accessibility-first defaults for ASP.NET Core MVC.

## Projects

- `src/A11yDefaults.Mvc`: the Razor Class Library / NuGet package
- `samples/A11yDefaults.Mvc.Sample`: a small MVC app showing the package in use
- `tests/A11yDefaults.Mvc.Tests`: a no-dependencies smoke-test console app

## Install

```dotnetcli
dotnet add package A11yDefaults.Mvc
```

## Why MVC Tag Helpers

For MVC work, the most natural package is:

1. Tag Helpers that enhance native HTML.
2. A small CSS asset shipped through static web assets.
3. Options you can register once in `Program.cs`.

That lets the package feel closer to a Telerik or Syncfusion baseline: install it, add the Tag Helpers, pull in the stylesheet, and get saner defaults without retraining everyone on a custom syntax.

## Build

```dotnetcli
dotnet run -c Release --project tests/A11yDefaults.Mvc.Tests/A11yDefaults.Mvc.Tests.csproj
dotnet pack src/A11yDefaults.Mvc/A11yDefaults.Mvc.csproj -c Release -o artifacts/packages
```

Or run the release helper:

```powershell
.\scripts\Publish-NuGet.ps1
```

To publish to NuGet.org, create a NuGet API key with Push scope, store it in
`NUGET_API_KEY`, and run:

```powershell
$env:NUGET_API_KEY = "<your key>"
.\scripts\Publish-NuGet.ps1 -Publish
```

## License

This repository is available under the MIT License.
