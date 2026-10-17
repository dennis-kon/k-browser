#requires -Version 5.1
<#
.SYNOPSIS
    Publishes K-Browser and builds the Inno Setup installer into .\installer.
.PARAMETER Configuration
    Build configuration (default: Release).
.PARAMETER FrameworkDependent
    Skip bundling the .NET runtime (smaller installer, requires .NET 8 Desktop Runtime on the target).
#>
[CmdletBinding()]
param(
    [string]$Configuration = 'Release',
    [switch]$FrameworkDependent
)

$ErrorActionPreference = 'Stop'
Set-Location $PSScriptRoot

$publishDir = Join-Path $PSScriptRoot 'publish'
if (Test-Path $publishDir) { Remove-Item $publishDir -Recurse -Force }

$selfContained = if ($FrameworkDependent) { 'false' } else { 'true' }
dotnet publish k-browser.vbproj -c $Configuration -r win-x64 --self-contained $selfContained -o $publishDir
if ($LASTEXITCODE -ne 0) { throw 'dotnet publish failed.' }

$version = (Get-Item (Join-Path $publishDir 'K-Browser.exe')).VersionInfo.FileVersion
if (-not $version) { $version = '1.0.0.0' }

$candidates = @(
    (Get-Command iscc -ErrorAction SilentlyContinue).Source,
    "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe",
    "${env:ProgramFiles(x86)}\Inno Setup 6\ISCC.exe",
    "$env:ProgramFiles\Inno Setup 6\ISCC.exe"
) | Where-Object { $_ -and (Test-Path $_) }

if (-not $candidates) {
    throw 'Inno Setup 6 not found. Install it with: winget install JRSoftware.InnoSetup'
}

& ($candidates | Select-Object -First 1) "/DAppVersion=$version" "/DPublishDir=$publishDir" installer.iss
if ($LASTEXITCODE -ne 0) { throw 'Inno Setup compilation failed.' }

Get-ChildItem (Join-Path $PSScriptRoot 'installer') -Filter *.exe |
    ForEach-Object { Write-Host ("Built {0} ({1:N0} MB)" -f $_.FullName, ($_.Length / 1MB)) }
