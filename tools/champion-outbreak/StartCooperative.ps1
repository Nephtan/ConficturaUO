param(
    [Parameter(Mandatory=$true)][string]$Root,
    [Parameter(Mandatory=$true)][ValidateSet('server','patient','helper')][string]$Role
)
$ErrorActionPreference = 'Stop'
$copyRoot = (Resolve-Path -LiteralPath $Root).Path.TrimEnd('\')
if (-not $copyRoot.StartsWith('D:\ConficturaUOTestTools\', [StringComparison]::OrdinalIgnoreCase)) { throw 'Outside disposable test root' }
$evidence = Join-Path $copyRoot 'evidence'
$shardPath = Join-Path $copyRoot 'shard'
if ((Get-Content -LiteralPath (Join-Path $evidence 'native-private-root.txt') -Raw).Trim() -ne $shardPath) { throw 'Private root mismatch' }
$prepared = Get-Content -LiteralPath (Join-Path $evidence 'prepared.json') -Raw | ConvertFrom-Json
$frozen = Get-Content -LiteralPath (Join-Path $evidence 'cooperative-inputs.json') -Raw | ConvertFrom-Json
if ($prepared.fixture -ne 'acceptance' -or -not $frozen.synthetic_roles -or $frozen.expected_clients -ne 2 -or
    (Test-Path -LiteralPath (Join-Path $evidence 'native-stopped.txt'))) { throw 'Not a fresh cooperative copy' }
$arguments = @('-skiploginscreen','true','-autologin','true')
if ($Role -eq 'server') {
    $proof = Get-Content -LiteralPath (Join-Path $evidence 'compile-log-verification.json') -Raw | ConvertFrom-Json
    if (-not $proof.passed) { throw 'Forced compile proof required' }
    if (Get-NetTCPConnection -State Listen -LocalPort $prepared.port -ErrorAction SilentlyContinue) { throw 'Private port already occupied' }
    $directory = $shardPath
    $executable = Join-Path $directory 'ConficturaServer.exe'
    $expectedHash = $prepared.core_sha256
    $pidFile = Join-Path $evidence 'native-server-pid.txt'
    $arguments = @('-service','-nocache')
} else {
    if (-not (Test-Path -LiteralPath (Join-Path $evidence 'cooperative-ready.txt'))) { throw 'Private server setup not ready' }
    $folder = if ($Role -eq 'patient') { 'client' } else { 'ally-client' }
    $directory = Join-Path $copyRoot $folder
    $executable = Join-Path $directory 'ClassicUO.exe'
    $expectedHash = $frozen.files.PSObject.Properties[$folder + '\ClassicUO.exe'].Value
    $pidFile = Join-Path $evidence ($Role + '-client-pid.txt')
    if ($Role -eq 'helper') {
        $credentialPath = Join-Path $evidence 'cooperative-account.private'
        if ((Get-FileHash -LiteralPath $credentialPath -Algorithm SHA256).Hash.ToLowerInvariant() -ne $frozen.files.'evidence\cooperative-account.private') { throw 'Private generated credentials changed' }
        $testLogin = Get-Content -LiteralPath $credentialPath
        if ($testLogin.Count -ne 2 -or $testLogin[0] -notmatch '^outbreak-coop-[a-f0-9]{12}$' -or $testLogin[1] -notmatch '^[a-f0-9]{24}$') { throw 'Generated legacy-safe test credentials required' }
        $arguments += @('-username',$testLogin[0],'-password',$testLogin[1])
    }
}
if ((Test-Path -LiteralPath $pidFile) -or -not $expectedHash -or
    (Get-FileHash -LiteralPath $executable -Algorithm SHA256).Hash.ToLowerInvariant() -ne $expectedHash) { throw 'Launch provenance mismatch or previously launched role' }
$ownedProcess = Start-Process -FilePath $executable -WorkingDirectory $directory -ArgumentList $arguments -WindowStyle Hidden -PassThru
$ownedProcess.Id | Set-Content -LiteralPath $pidFile -Encoding UTF8
@{role=$Role;pid=$ownedProcess.Id;utc=[DateTime]::UtcNow.ToString('o');helper_login='Generated private CLI credentials; no saved-password encoding assumption; values withheld'} |
    ConvertTo-Json | Set-Content -LiteralPath (Join-Path $evidence ($Role + '-launch.json')) -Encoding UTF8
Write-Output "Owned $Role PID $($ownedProcess.Id)"
$ownedProcess.WaitForExit()
Write-Output "Owned $Role exit $($ownedProcess.ExitCode)"
