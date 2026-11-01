param([string]$OutputDirectory = 'D:\ConficturaUOTestTools\infestation-polish-20261101-001\verification', [string]$VerifiedBuild)
$ErrorActionPreference = 'Stop'
$territoryRepository = Split-Path -Parent $PSScriptRoot
$territoryOutput = [IO.Path]::GetFullPath($OutputDirectory)
if (!$territoryOutput.StartsWith('D:\ConficturaUOTestTools\',[StringComparison]::OrdinalIgnoreCase)) { throw 'Use a disposable TestTools output directory' }
New-Item -ItemType Directory -Path $territoryOutput -Force | Out-Null
if ($VerifiedBuild) {
    $territoryRun = (Resolve-Path -LiteralPath $VerifiedBuild).Path
    if (!$territoryRun.StartsWith($territoryOutput + '\',[StringComparison]::OrdinalIgnoreCase)) { throw 'Reuse only a verified child build snapshot' }
    if (!(Get-Content -LiteralPath (Join-Path $territoryRun 'runtime-compile.log') -Raw).Contains('Compile-only verification completed successfully')) { throw 'Missing forced compile milestone' }
    foreach ($territoryRelative in (& rg --files Data/Scripts -g '*.cs' -g '!**/bin/**' -g '!**/obj/**')) {
        $territoryOriginal = [IO.File]::ReadAllText((Join-Path $territoryRepository $territoryRelative)).Replace("`r`n","`n")
        $territoryCopied = [IO.File]::ReadAllText((Join-Path $territoryRun $territoryRelative)).Replace("`r`n","`n")
        if ($territoryOriginal -cne $territoryCopied) { throw "Verified snapshot is stale: $territoryRelative" }
    }
} else {
    $territoryBefore = @(Get-ChildItem -LiteralPath $territoryOutput -Directory | Select-Object -ExpandProperty FullName)
    & (Join-Path $PSScriptRoot 'Test-ChampionRewards.ps1') -OutputDirectory $territoryOutput
    $territoryCandidates = @(Get-ChildItem -LiteralPath $territoryOutput -Directory | Where-Object { $territoryBefore -notcontains $_.FullName })
    if ($territoryCandidates.Count -ne 1) { throw 'Expected one exact new verified build snapshot' }
    $territoryRun = $territoryCandidates[0].FullName
}
$territoryCompiler = Join-Path $env:WINDIR 'Microsoft.NET/Framework/v4.0.30319/csc.exe'
$territoryEvidence = Join-Path $territoryRun ('component-runs/' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $territoryEvidence -Force | Out-Null
Write-Output "Component attempt evidence: $territoryEvidence"
$territoryTests = @('ArachnidTerritoryRegression','PersistentInfestationRegression','ArachnidInfestationRegression','ArachnidInfestationTownRegression','ArachnidOutbreakControllerRegression','ArachnidOutbreakRewardRegression','ArachnidOutbreakWebRegression','ArachnidOutbreakBardRegression','ChampionRewardRegression','DropReagentRegression')
$territoryResults = @()
Push-Location -LiteralPath $territoryRun
try {
    foreach ($territoryTest in $territoryTests) {
        $territorySource = Join-Path $territoryRepository ('tests/' + $territoryTest + '.cs')
        $territoryExe = Join-Path $territoryRun ($territoryTest + '.exe')
        Copy-Item -LiteralPath $territorySource -Destination $territoryEvidence
        & $territoryCompiler /nologo /target:exe /platform:x86 "/out:$territoryExe" "/reference:$territoryRun\ConficturaServer.exe" "/reference:$territoryRun\Data\Data.bin" /reference:System.Core.dll $territorySource *> (Join-Path $territoryEvidence ($territoryTest + '-compile.log'))
        if ($LASTEXITCODE -ne 0) { throw "$territoryTest compilation failed; retained log in $territoryRun" }
        Copy-Item -LiteralPath $territoryExe -Destination $territoryEvidence
        $territoryWorking = Join-Path $territoryEvidence $territoryTest
        New-Item -ItemType Directory -Path $territoryWorking -Force | Out-Null
        Copy-Item -LiteralPath $territoryExe -Destination $territoryWorking
        Copy-Item -LiteralPath (Join-Path $territoryRun 'ConficturaServer.exe') -Destination $territoryWorking
        foreach ($territoryDependency in Get-ChildItem -LiteralPath $territoryRun -Filter '*.dll' -File) {
            Copy-Item -LiteralPath $territoryDependency.FullName -Destination $territoryWorking
        }
        New-Item -ItemType Directory -Path (Join-Path $territoryWorking 'Data/System') -Force | Out-Null
        Copy-Item -LiteralPath (Join-Path $territoryRun 'Data/System/CFG') -Destination (Join-Path $territoryWorking 'Data/System/CFG') -Recurse
        Copy-Item -LiteralPath (Join-Path $territoryRun 'Data/Bounds.bin') -Destination (Join-Path $territoryWorking 'Data/Bounds.bin')
        Push-Location -LiteralPath $territoryWorking
        try {
            & (Join-Path $territoryWorking ($territoryTest + '.exe')) (Join-Path $territoryRepository 'Data/Files') (Join-Path $territoryRepository 'Data/TurnBasedCombat') *> (Join-Path $territoryEvidence ($territoryTest + '.log'))
            $territoryExit = $LASTEXITCODE
        } finally { Pop-Location }
        $territoryResults += @{ name=$territoryTest; exitCode=$territoryExit }
        $territoryResults | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $territoryEvidence 'territory-results.json') -Encoding UTF8
        Get-Content -LiteralPath (Join-Path $territoryEvidence ($territoryTest + '.log')) -Tail 5
        if ($territoryExit -ne 0) { throw "$territoryTest failed; retained snapshot and logs in $territoryRun" }
    }
} finally { Pop-Location }
Write-Output "Territory source-method verification complete: $territoryRun. Native client acceptance remains separate."
