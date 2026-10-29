param([Parameter(Mandatory=$true)][string]$Root,[switch]$FailedServer,[switch]$StopObserver)
$ErrorActionPreference = 'Stop'
$copyRoot = (Resolve-Path -LiteralPath $Root).Path.TrimEnd('\')
if (-not $copyRoot.StartsWith('D:\ConficturaUOTestTools\', [StringComparison]::OrdinalIgnoreCase)) { throw 'Outside disposable test root' }
$evidence = Join-Path $copyRoot 'evidence'
if ((Get-Content -LiteralPath (Join-Path $evidence 'native-private-root.txt') -Raw).Trim() -ne (Join-Path $copyRoot 'shard')) { throw 'Private root mismatch' }
$acknowledged=Test-Path -LiteralPath (Join-Path $evidence 'native-stopped.txt')
if (-not $acknowledged -and -not $FailedServer) { throw 'No no-save stop acknowledgement' }
if ($StopObserver -and -not $FailedServer) { throw 'Observer stop requires explicit failed-server cleanup' }
$prepared = Get-Content -LiteralPath (Join-Path $evidence 'prepared.json') -Raw | ConvertFrom-Json
$allowed = @((Join-Path $copyRoot 'client\ClassicUO.exe'), (Join-Path $copyRoot 'shard\ConficturaServer.exe'))
$owned = @(Get-CimInstance Win32_Process | Where-Object { $_.ExecutablePath -and $_.ExecutablePath.StartsWith($copyRoot + '\', [StringComparison]::OrdinalIgnoreCase) })
foreach ($process in $owned) { if ($process.ExecutablePath -notin $allowed) { throw 'Unexpected owned process; inspect before stopping' } }
if ($FailedServer) {
    if (@($owned | Where-Object {$_.ExecutablePath -eq $allowed[1]}).Count -ne 0) { throw 'Failed-server cleanup requires an already exited server' }
    $console=Get-Content -LiteralPath (Join-Path $copyRoot 'shard\Data\Logs\Console.log') -Raw
    if ($console -notmatch 'Crash: Generating report|Scripts: One or more scripts failed') { throw 'No retained failed-server evidence' }
}
$listeners = @(Get-NetTCPConnection -State Listen -LocalPort $prepared.port -ErrorAction SilentlyContinue)
foreach ($listener in $listeners) { if ($listener.LocalAddress -ne '127.0.0.1' -or $listener.OwningProcess -notin $owned.ProcessId) { throw 'Private listener ownership mismatch' } }
foreach ($process in $owned) { Stop-Process -Id $process.ProcessId -Force; Wait-Process -Id $process.ProcessId -Timeout 5 -ErrorAction SilentlyContinue }
if ($StopObserver) {
    $observerRootPattern='(?i)(?:^|\s|")'+[regex]::Escape($copyRoot)+'(?:"|\s|$)'
    $observerSlashPattern='(?i)(?:^|\s|")'+[regex]::Escape($copyRoot.Replace('\','/'))+'(?:"|\s|$)'
    $observers=@(Get-CimInstance Win32_Process | Where-Object {
        $_.ExecutablePath -and [IO.Path]::GetFileName($_.ExecutablePath) -like 'python*.exe' -and $_.CommandLine -and
        $_.CommandLine.Contains('RunArchmageCase.py') -and
        ($_.CommandLine -match $observerRootPattern -or $_.CommandLine -match $observerSlashPattern)
    })
    if ($observers.Count -gt 1) { throw 'Ambiguous failed-case observer' }
    foreach ($observer in $observers) { Stop-Process -Id $observer.ProcessId -Force; Wait-Process -Id $observer.ProcessId -Timeout 5 -ErrorAction SilentlyContinue }
}
$remaining = @(Get-CimInstance Win32_Process | Where-Object { $_.ExecutablePath -and $_.ExecutablePath.StartsWith($copyRoot + '\', [StringComparison]::OrdinalIgnoreCase) })
$listeners = @(Get-NetTCPConnection -State Listen -LocalPort $prepared.port -ErrorAction SilentlyContinue)
if ($remaining.Count -ne 0 -or $listeners.Count -ne 0) { throw 'Owned teardown incomplete' }
@{ utc=[DateTime]::UtcNow.ToString('o'); forced_no_save=[bool]$acknowledged; native_stop_acknowledged=[bool]$acknowledged; failed_server_cleanup=[bool]$FailedServer; graceful_shutdown_claimed=$false; remaining_owned_processes=0; remaining_private_listeners=0 } |
    ConvertTo-Json | Set-Content -LiteralPath (Join-Path $evidence 'teardown.json') -Encoding UTF8
if ($acknowledged) { 'Owned processes and private listener stopped; forced no-save exit, not graceful shutdown.' }
else { 'Failed-server copy cleaned; no native stop acknowledgement or graceful shutdown claimed.' }
