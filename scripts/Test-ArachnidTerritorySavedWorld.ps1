param(
    [Parameter(Mandatory=$true)][string]$CheckpointDirectory,
    [Parameter(Mandatory=$true)][string]$VerifiedBuild,
    [string]$OutputDirectory='D:\ConficturaUOTestTools\arachnid-saved-world-verification',
    [string]$GameFiles='D:\ConficturaUOTestTools\shard\Data\Files',
    [int]$RetiredPatchId=-1,
    [ValidateRange(0,2147483647)][int]$MinimumResponseBirths=0,
    [switch]$ExpectNoviceDead
)
$ErrorActionPreference='Stop'
$savedRepository=Split-Path -Parent $PSScriptRoot
$savedCheckpoint=(Resolve-Path -LiteralPath $CheckpointDirectory).Path
$savedBuild=(Resolve-Path -LiteralPath $VerifiedBuild).Path
$savedOutput=[IO.Path]::GetFullPath($OutputDirectory)
foreach($savedPath in @($savedCheckpoint,$savedBuild,$savedOutput)){
    if(!$savedPath.StartsWith('D:\ConficturaUOTestTools\',[StringComparison]::OrdinalIgnoreCase)){throw 'Use private TestTools copies only'}
}
if($savedOutput.StartsWith($savedCheckpoint+'\',[StringComparison]::OrdinalIgnoreCase)){throw 'Do not write evidence inside a retained checkpoint'}
$savedManifestPath=Join-Path $savedCheckpoint 'checkpoint.json'
$savedManifest=Get-Content -LiteralPath $savedManifestPath -Raw | ConvertFrom-Json
if(!$savedManifest.saveFiles){throw 'Ordinary copied World.Save checkpoint manifest required'}
foreach($savedEntry in $savedManifest.saveFiles.psobject.Properties){
    $savedFile=[IO.Path]::GetFullPath((Join-Path $savedCheckpoint $savedEntry.Name))
    if(!$savedFile.StartsWith($savedCheckpoint+'\Saves\',[StringComparison]::OrdinalIgnoreCase)){throw 'Checkpoint hash entry escapes its save directory'}
    if((Get-FileHash -LiteralPath $savedFile -Algorithm SHA256).Hash -ine $savedEntry.Value){throw 'Checkpoint hash changed'}
}
if(!(Get-Content -LiteralPath (Join-Path $savedBuild 'runtime-compile.log') -Raw).Contains('Compile-only verification completed successfully')){throw 'Matching forced runtime compile proof required'}
foreach($savedSource in (& rg --files (Join-Path $savedRepository 'Data/Scripts') -g '*.cs' -g '!**/bin/**' -g '!**/obj/**')){
    $savedRelative=$savedSource.Substring(($savedRepository+'\').Length)
    if([IO.File]::ReadAllText($savedSource).Replace("`r`n","`n") -cne [IO.File]::ReadAllText((Join-Path $savedBuild $savedRelative)).Replace("`r`n","`n")){throw "Compiled snapshot is stale: $savedRelative"}
}
$savedAttempt=Join-Path $savedOutput ([Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $savedAttempt -Force | Out-Null
Copy-Item -LiteralPath (Join-Path $savedCheckpoint 'Saves') -Destination (Join-Path $savedAttempt 'Saves') -Recurse
Copy-Item -LiteralPath $savedManifestPath -Destination $savedAttempt
Copy-Item -LiteralPath (Join-Path $savedBuild 'ConficturaServer.exe') -Destination $savedAttempt
Get-ChildItem -LiteralPath $savedBuild -Filter '*.dll' -File | Copy-Item -Destination $savedAttempt
New-Item -ItemType Directory -Path (Join-Path $savedAttempt 'Data/System') -Force | Out-Null
Copy-Item -LiteralPath (Join-Path $savedBuild 'Data/System/CFG') -Destination (Join-Path $savedAttempt 'Data/System/CFG') -Recurse
Copy-Item -LiteralPath (Join-Path $savedBuild 'Data/Bounds.bin') -Destination (Join-Path $savedAttempt 'Data/Bounds.bin')
$savedTest=Join-Path $savedRepository 'tests/ArachnidSavedWorldRoundTrip.cs'
Copy-Item -LiteralPath $savedTest -Destination $savedAttempt
$savedExe=Join-Path $savedAttempt 'ArachnidSavedWorldRoundTrip.exe'
& "$env:WINDIR\Microsoft.NET\Framework\v4.0.30319\csc.exe" /nologo /target:exe /platform:x86 "/out:$savedExe" "/reference:$savedBuild\ConficturaServer.exe" "/reference:$savedBuild\Data\Data.bin" /reference:System.Core.dll $savedTest *> (Join-Path $savedAttempt 'compile.log')
if($LASTEXITCODE -ne 0){throw "Copied-save test compile failed; evidence $savedAttempt"}
$savedLife=if($ExpectNoviceDead){'dead'}else{'alive'}
Push-Location -LiteralPath $savedAttempt
try { & $savedExe $GameFiles $RetiredPatchId $MinimumResponseBirths '-' $savedLife *> (Join-Path $savedAttempt 'load.log'); $savedExit=$LASTEXITCODE } finally {Pop-Location}
@{exitCode=$savedExit;checkpoint=$savedCheckpoint;coreSHA256=(Get-FileHash -LiteralPath (Join-Path $savedBuild 'ConficturaServer.exe') -Algorithm SHA256).Hash;testSHA256=(Get-FileHash -LiteralPath $savedTest -Algorithm SHA256).Hash;scope='Offline native World.Load and explicit recovery; no global Initialize, listener, gameplay resume or save'} | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $savedAttempt 'result.json') -Encoding UTF8
Get-Content -LiteralPath (Join-Path $savedAttempt 'load.log') -Tail 6
Write-Output "Copied-save verification evidence: $savedAttempt"
if($savedExit -ne 0){throw 'Copied-save verification failed; retained attempt'}
