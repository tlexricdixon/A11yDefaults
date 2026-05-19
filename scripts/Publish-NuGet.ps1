param(
    [string] $Configuration = "Release",
    [string] $PackageProject = "src/A11yDefaults.Mvc/A11yDefaults.Mvc.csproj",
    [string] $TestProject = "tests/A11yDefaults.Mvc.Tests/A11yDefaults.Mvc.Tests.csproj",
    [string] $OutputDirectory = "artifacts/packages",
    [string] $Source = "https://api.nuget.org/v3/index.json",
    [string] $ApiKey = $env:NUGET_API_KEY,
    [switch] $Publish,
    [switch] $SkipTests
)

$ErrorActionPreference = "Stop"

function Write-Step {
    param([string] $Message)
    Write-Host ""
    Write-Host "==> $Message" -ForegroundColor Cyan
}

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
Set-Location $repoRoot

Write-Step "Cleaning package output"
New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null
Remove-Item -Path (Join-Path $OutputDirectory "*.nupkg") -Force -ErrorAction SilentlyContinue
Remove-Item -Path (Join-Path $OutputDirectory "*.snupkg") -Force -ErrorAction SilentlyContinue

if (-not $SkipTests) {
    Write-Step "Running smoke tests"
    dotnet run -c $Configuration --project $TestProject
}

Write-Step "Packing NuGet package"
dotnet pack $PackageProject -c $Configuration -o $OutputDirectory

$nupkg = Get-ChildItem -Path $OutputDirectory -Filter "*.nupkg" |
    Where-Object { $_.Name -notlike "*.symbols.nupkg" } |
    Sort-Object LastWriteTime -Descending |
    Select-Object -First 1

$snupkg = Get-ChildItem -Path $OutputDirectory -Filter "*.snupkg" |
    Sort-Object LastWriteTime -Descending |
    Select-Object -First 1

if ($null -eq $nupkg) {
    throw "No .nupkg file was created in $OutputDirectory."
}

Write-Step "Package ready"
Write-Host $nupkg.FullName
if ($snupkg) {
    Write-Host $snupkg.FullName
}

if (-not $Publish) {
    Write-Host ""
    Write-Host "Dry run complete. Re-run with -Publish when you are ready to push to NuGet.org." -ForegroundColor Yellow
    exit 0
}

if ([string]::IsNullOrWhiteSpace($ApiKey)) {
    throw "Set NUGET_API_KEY or pass -ApiKey before publishing."
}

Write-Step "Publishing package to NuGet.org"
dotnet nuget push $nupkg.FullName --api-key $ApiKey --source $Source --skip-duplicate

if ($snupkg) {
    Write-Step "Publishing symbols to NuGet.org"
    dotnet nuget push $snupkg.FullName --api-key $ApiKey --source $Source --skip-duplicate
}

Write-Host ""
Write-Host "Published. Nice work." -ForegroundColor Green
