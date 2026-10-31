param([Parameter(Mandatory=$true)][string]$Root,[Parameter(Mandatory=$true)][ValidateSet('server','client','partner')][string]$Role)
$ErrorActionPreference = 'Stop'
$copyRoot = (Resolve-Path -LiteralPath $Root).Path.TrimEnd('\')
if (-not $copyRoot.StartsWith('D:\ConficturaUOTestTools\', [StringComparison]::OrdinalIgnoreCase)) { throw 'Outside disposable test root' }
$evidence = Join-Path $copyRoot 'evidence'
$shardPath = Join-Path $copyRoot 'shard'
if ((Get-Content -LiteralPath (Join-Path $evidence 'native-private-root.txt') -Raw).Trim() -ne $shardPath) { throw 'Guard mismatch' }
$prepared = Get-Content -LiteralPath (Join-Path $evidence 'prepared.json') -Raw | ConvertFrom-Json
$freeze = Get-Content -LiteralPath (Join-Path $evidence 'saved-build-input-freeze.private.json') -Raw | ConvertFrom-Json
if ($prepared.fixture -ne 'archmage-preparation' -or (Test-Path -LiteralPath (Join-Path $evidence 'native-stopped.txt'))) { throw 'Fresh representative fixture required' }
foreach ($entry in $freeze.files.PSObject.Properties) {
    $ownFolder = if ($Role -eq 'partner') { 'ally-client/' } else { 'client/' }
    if ($Role -ne 'server' -and $entry.Name.EndsWith('/settings.json') -and -not $entry.Name.StartsWith($ownFolder)) { continue }
    if ((Get-FileHash -LiteralPath (Join-Path $copyRoot $entry.Name) -Algorithm SHA256).Hash.ToLowerInvariant() -ne $entry.Value) { throw 'Frozen input mismatch' }
}
if ($Role -eq 'server') {
    $directory = $shardPath
    $executable = Join-Path $directory 'ConficturaServer.exe'
    if ((Get-FileHash -LiteralPath $executable -Algorithm SHA256).Hash.ToLowerInvariant() -ne $prepared.core_sha256) { throw 'Core mismatch' }
    if (Get-NetTCPConnection -State Listen -LocalPort $prepared.port -ErrorAction SilentlyContinue) { throw 'Private port occupied' }
    $arguments = @('-service','-nocache')
} else {
    if (-not (Test-Path -LiteralPath (Join-Path $evidence 'archmage-original.private.xml'))) { throw 'Native actor preparation not ready' }
    if (-not (Test-Path -LiteralPath (Join-Path $evidence 'serializer-selfcheck-passed.txt'))) { throw 'Actual diagnostic writer selfcheck must pass before clients connect' }
    $directory = Join-Path $copyRoot $(if ($Role -eq 'partner') { 'ally-client' } else { 'client' })
    $executable = Join-Path $directory 'ClassicUO.exe'
    $arguments = @('-skiploginscreen','true','-autologin','true')
    if ($Role -eq 'partner') {
        $protection = Get-Content -LiteralPath (Join-Path $evidence 'protection-inputs.private.json') -Raw | ConvertFrom-Json
        if ($protection.actor_provenance -ne 'actual_saved_players' -or $protection.expected_clients -ne 2) { throw 'Actual saved partner provenance required' }
        $testLogin = Get-Content -LiteralPath (Join-Path $evidence 'protection-account.private')
        if ($testLogin.Count -ne 2 -or $testLogin[0] -notmatch '^infest-probe-[a-f0-9]{12}$' -or $testLogin[1] -notmatch '^[a-f0-9]{24}$') { throw 'Generated private login invalid' }
        $arguments += @('-username',$testLogin[0],'-password',$testLogin[1])
    }
}
$record = Join-Path $evidence ($Role + '-process.json')
if (Test-Path -LiteralPath $record) { throw 'Role already launched; preserve failed cases' }
$ownedProcess = Start-Process -FilePath $executable -WorkingDirectory $directory -ArgumentList $arguments -WindowStyle Hidden -PassThru
@{processId=$ownedProcess.Id;executable=$executable;startedUtc=[DateTime]::UtcNow.ToString('o');scope='Guarded normal-user host launch; private no-save acceptance only'} | ConvertTo-Json | Set-Content -LiteralPath $record -Encoding UTF8
Write-Output "Owned $Role PID $($ownedProcess.Id); launch record is not readiness proof."
