param([Parameter(Mandatory=$true)][string]$Root)
$ErrorActionPreference = 'Stop'
$copyRoot = (Resolve-Path -LiteralPath $Root).Path.TrimEnd('\')
if (-not $copyRoot.StartsWith('D:\ConficturaUOTestTools\', [StringComparison]::OrdinalIgnoreCase)) { throw 'Outside disposable test root' }
$evidence = Join-Path $copyRoot 'evidence'
if ((Get-Content -LiteralPath (Join-Path $evidence 'native-private-root.txt') -Raw).Trim() -ne (Join-Path $copyRoot 'shard')) { throw 'Private root mismatch' }
if (-not (Test-Path -LiteralPath (Join-Path $evidence 'native-stopped.txt'))) { throw 'No no-save stop acknowledgement' }
$prepared = Get-Content -LiteralPath (Join-Path $evidence 'prepared.json') -Raw | ConvertFrom-Json
$allowed = @((Join-Path $copyRoot 'client\ClassicUO.exe'), (Join-Path $copyRoot 'shard\ConficturaServer.exe'))
$owned = @(Get-CimInstance Win32_Process | Where-Object { $_.ExecutablePath -and $_.ExecutablePath.StartsWith($copyRoot + '\', [StringComparison]::OrdinalIgnoreCase) })
foreach ($process in $owned) { if ($process.ExecutablePath -notin $allowed) { throw 'Unexpected owned process; inspect before stopping' } }
$listeners = @(Get-NetTCPConnection -State Listen -LocalPort $prepared.port -ErrorAction SilentlyContinue)
foreach ($listener in $listeners) { if ($listener.LocalAddress -ne '127.0.0.1' -or $listener.OwningProcess -notin $owned.ProcessId) { throw 'Private listener ownership mismatch' } }
foreach ($process in $owned) { Stop-Process -Id $process.ProcessId -Force; Wait-Process -Id $process.ProcessId -Timeout 5 -ErrorAction SilentlyContinue }
$remaining = @(Get-CimInstance Win32_Process | Where-Object { $_.ExecutablePath -and $_.ExecutablePath.StartsWith($copyRoot + '\', [StringComparison]::OrdinalIgnoreCase) })
$listeners = @(Get-NetTCPConnection -State Listen -LocalPort $prepared.port -ErrorAction SilentlyContinue)
if ($remaining.Count -ne 0 -or $listeners.Count -ne 0) { throw 'Owned teardown incomplete' }
@{ utc=[DateTime]::UtcNow.ToString('o'); forced_no_save=$true; graceful_shutdown_claimed=$false; remaining_owned_processes=0; remaining_private_listeners=0 } |
    ConvertTo-Json | Set-Content -LiteralPath (Join-Path $evidence 'teardown.json') -Encoding UTF8
'Owned processes and private listener stopped; forced no-save exit, not graceful shutdown.'
