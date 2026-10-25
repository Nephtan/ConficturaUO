param(
    [Parameter(Mandatory=$true)][string]$Snapshot,
    [string[]]$Fixtures = @('payments'),
    [switch]$Prepare,
    [switch]$AllowHistorical,
    [switch]$SelfTest
)
$ErrorActionPreference = 'Stop'
$arguments = @((Join-Path $PSScriptRoot 'foundation_fixture_runner.py'), '--snapshot', $Snapshot)
if ($Prepare) { $arguments += '--prepare' }
if ($AllowHistorical) { $arguments += '--allow-historical' }
if ($SelfTest) { $arguments += '--selftest' }
if ($Fixtures.Count -gt 0) { $arguments += '--fixtures'; $arguments += $Fixtures }
& python @arguments
exit $LASTEXITCODE
