param(
    [string]$RepositoryRoot = (Split-Path -Parent $PSScriptRoot),
    [string]$OutputDirectory = "output/stabling-verification",
    [string]$MSBuildPath
)

$ErrorActionPreference = "Stop"
$RepositoryRoot = (Resolve-Path -LiteralPath $RepositoryRoot).Path
if (-not [IO.Path]::IsPathRooted($OutputDirectory)) {
    $OutputDirectory = Join-Path $RepositoryRoot $OutputDirectory
}
$runDirectory = Join-Path $OutputDirectory ([Guid]::NewGuid().ToString("N"))
New-Item -ItemType Directory -Path $runDirectory -Force | Out-Null
Write-Host "Verification output: $runDirectory"

function Invoke-VerificationProcess {
    param([string]$File, [string]$Arguments, [string]$Name,
        [int]$TimeoutSeconds = 180, [int]$MaximumExitCode = 0,
        [string]$WorkingDirectory = $runDirectory)

    $stdout = Join-Path $runDirectory "$Name.log"
    $stderr = Join-Path $runDirectory "$Name.stderr.log"
    $process = New-Object Diagnostics.Process
    $process.StartInfo.FileName = $File
    $process.StartInfo.Arguments = $Arguments
    $process.StartInfo.WorkingDirectory = $WorkingDirectory
    $process.StartInfo.UseShellExecute = $false
    $process.StartInfo.CreateNoWindow = $true
    $process.StartInfo.RedirectStandardOutput = $true
    $process.StartInfo.RedirectStandardError = $true
    try {
        $null = $process.Start()
        $outputTask = $process.StandardOutput.ReadToEndAsync()
        $errorTask = $process.StandardError.ReadToEndAsync()
        $watch = [Diagnostics.Stopwatch]::StartNew()
        $timedOut = $false
        while (-not $process.WaitForExit(1000)) {
            if ($watch.Elapsed.TotalSeconds -ge $TimeoutSeconds) {
                $process.Kill()
                $timedOut = $true
                break
            }
        }
        $process.WaitForExit()
        [IO.File]::WriteAllText($stdout, $outputTask.Result)
        [IO.File]::WriteAllText($stderr, $errorTask.Result)
        if ($timedOut) {
            throw "$Name exceeded $TimeoutSeconds seconds. See $stdout"
        }
        if ($process.ExitCode -lt 0 -or $process.ExitCode -gt $MaximumExitCode) {
            Get-Content -LiteralPath $stdout -Tail 35 | Write-Host
            Get-Content -LiteralPath $stderr -Tail 35 | Write-Host
            throw "$Name failed with exit code $($process.ExitCode)."
        }
        Write-Host "$Name passed (exit $($process.ExitCode))."
    }
    finally {
        $process.Dispose()
    }
}

if (-not $MSBuildPath) {
    $vswhere = Join-Path ${env:ProgramFiles(x86)} "Microsoft Visual Studio/Installer/vswhere.exe"
    $MSBuildPath = & $vswhere -latest -products * -requires Microsoft.Component.MSBuild `
        -find "MSBuild\**\Bin\MSBuild.exe" | Select-Object -First 1
}
if (-not $MSBuildPath -or -not (Test-Path -LiteralPath $MSBuildPath)) {
    throw "Visual Studio MSBuild was not found; supply -MSBuildPath."
}

# Compile a snapshot in a fresh directory containing no saves or service configuration.
$scriptSource = Join-Path $RepositoryRoot "Data/Scripts"
$scriptDestination = Join-Path $runDirectory "Data/Scripts"
Invoke-VerificationProcess -File "robocopy.exe" -Name "source-copy" -MaximumExitCode 7 `
    -Arguments ('"{0}" "{1}" *.cs /S /XD bin obj .vs /R:0 /W:0 /NFL /NDL /NJH /NJS /NP' -f $scriptSource, $scriptDestination)
$configDirectory = Join-Path $runDirectory "Data/System/CFG"
New-Item -ItemType Directory -Path $configDirectory -Force | Out-Null
Copy-Item -LiteralPath (Join-Path $RepositoryRoot "Data/System/CFG/Assemblies.cfg") -Destination $configDirectory
Copy-Item -LiteralPath (Join-Path $RepositoryRoot "Data/System/CFG/body.cfg") -Destination $configDirectory
Copy-Item -LiteralPath (Join-Path $RepositoryRoot "Data/Bounds.bin") -Destination (Join-Path $runDirectory "Data")
foreach ($dependency in @("OrbServerSDK.dll", "UOArchitectInterface.dll")) {
    Copy-Item -LiteralPath (Join-Path $RepositoryRoot $dependency) -Destination $runDirectory
}

$project = Join-Path $RepositoryRoot "Data/System/Source/Server.csproj"
$intermediate = (Join-Path $runDirectory "server-obj").Replace('\', '/') + '/'
Invoke-VerificationProcess -File $MSBuildPath -Name "server-build" `
    -Arguments ('"{0}" /nologo /verbosity:minimal /p:Configuration=Release /p:Platform=x86 /p:OutputPath="{1}/" /p:IntermediateOutputPath="{2}"' -f $project, $runDirectory, $intermediate)
$server = Join-Path $runDirectory "ConficturaServer.exe"
Invoke-VerificationProcess -File $server -Arguments "-compileonly -nocache" -Name "runtime-compile"
$compileOutput = Get-Content -LiteralPath (Join-Path $runDirectory "runtime-compile.log") -Raw
if (-not $compileOutput.Contains("Scripts: Compile-only verification completed successfully.")) {
    throw "The compile-only success milestone was not reported."
}

$scriptAssembly = Join-Path $runDirectory "Data/Data.bin"
$assemblyName = [Reflection.AssemblyName]::GetAssemblyName($scriptAssembly).Name
Copy-Item -LiteralPath $scriptAssembly -Destination (Join-Path $runDirectory "$assemblyName.dll")
$testSource = Join-Path $RepositoryRoot "tests/StableRegression.cs"
$testExe = Join-Path $runDirectory "StableRegression.exe"
$compiler = Join-Path $env:WINDIR "Microsoft.NET/Framework/v4.0.30319/csc.exe"
Invoke-VerificationProcess -File $compiler -Name "fixture-build" `
    -Arguments ('/nologo /target:exe /platform:x86 /out:"{0}" /reference:"{1}" /reference:"{2}" "{3}"' -f $testExe, $server, $scriptAssembly, $testSource)
$clientData = Join-Path $RepositoryRoot "Data/Files"
Invoke-VerificationProcess -File $testExe -Arguments ('"{0}"' -f $clientData) -Name "regression" -TimeoutSeconds 60
Get-Content -LiteralPath (Join-Path $runDirectory "regression.log") | Write-Host

# The full input saves are private and ignored. Tests write only disposable copies.
$saveSource = Join-Path $RepositoryRoot "Saves"
if (Test-Path -LiteralPath (Join-Path $saveSource "Mobiles/Mobiles.bin")) {
    $before = @(Get-ChildItem -LiteralPath $saveSource -File -Recurse | Get-FileHash -Algorithm SHA256)
    $world = Join-Path $runDirectory "private-world"
    New-Item -ItemType Directory -Path $world -Force | Out-Null
    Invoke-VerificationProcess -File "robocopy.exe" -Name "private-save-copy" -MaximumExitCode 7 -Arguments ('"{0}" "{1}" /E /R:0 /W:0 /NFL /NDL /NJH /NJS /NP' -f $saveSource, (Join-Path $world "Saves"))
    Copy-Item -LiteralPath $server -Destination $world
    Get-ChildItem -LiteralPath $runDirectory -Filter "*.dll" -File | Copy-Item -Destination $world
    $worldCfg = Join-Path $world "Data/System/CFG"
    New-Item -ItemType Directory -Path $worldCfg -Force | Out-Null
    Copy-Item -LiteralPath (Join-Path $configDirectory "body.cfg") -Destination $worldCfg
    $worldExe = Join-Path $world "StableWorldRoundtrip.exe"
    Invoke-VerificationProcess -File $compiler -Name "world-fixture-build" -Arguments ('/nologo /target:exe /platform:x86 /out:"{0}" /reference:"{1}" /reference:"{2}" "{3}"' -f $worldExe, $server, $scriptAssembly, (Join-Path $RepositoryRoot "tests/StableWorldRoundtrip.cs"))
    foreach ($mode in @("write", "read")) {
        Invoke-VerificationProcess -File $worldExe -Name "world-$mode" -WorkingDirectory $world -Arguments ('"{0}" {1}' -f $clientData, $mode) -TimeoutSeconds 180
        Get-Content -LiteralPath (Join-Path $runDirectory "world-$mode.log") -Tail 3 | Write-Host
    }
    $after = @(Get-ChildItem -LiteralPath $saveSource -File -Recurse | Get-FileHash -Algorithm SHA256)
    if (Compare-Object $before $after -Property Path,Hash) { throw "Original save hashes changed." }
    Write-Host "Original save hashes unchanged."

    # Suppress the unrelated Orb listener and exit after Initialize, before game listeners.
    Invoke-VerificationProcess -File "robocopy.exe" -Name "smoke-data-copy" -MaximumExitCode 7 -Arguments ('"{0}" "{1}" /E /XD Source /XF *.local.cfg /R:0 /W:0 /NFL /NDL /NJH /NJS /NP' -f (Join-Path $RepositoryRoot "Data/System"), (Join-Path $runDirectory "Data/System"))
    Invoke-VerificationProcess -File "robocopy.exe" -Name "smoke-runtime-data-copy" -MaximumExitCode 7 -Arguments ('"{0}" "{1}" /E /XD Files Tools Scripts Logs Source /XF Data.bin *.local.cfg *.local.json /R:0 /W:0 /NFL /NDL /NJH /NJS /NP' -f (Join-Path $RepositoryRoot "Data"), (Join-Path $runDirectory "Data"))
    Invoke-VerificationProcess -File "robocopy.exe" -Name "smoke-script-data-copy" -MaximumExitCode 7 -Arguments ('"{0}" "{1}" /E /XD bin obj .vs /XF *.cs *.csproj *.local.cfg *.local.json /R:0 /W:0 /NFL /NDL /NJH /NJS /NP' -f $scriptSource, $scriptDestination)
    Copy-Item -LiteralPath (Join-Path $RepositoryRoot "Info") -Destination $runDirectory -Recurse -Force
    Copy-Item -LiteralPath (Join-Path $RepositoryRoot "Spawns") -Destination $runDirectory -Recurse -Force
    Invoke-VerificationProcess -File "robocopy.exe" -Name "smoke-world-copy" -MaximumExitCode 7 -Arguments ('"{0}" "{1}" /E /R:0 /W:0 /NFL /NDL /NJH /NJS /NP' -f (Join-Path $world "Saves"), (Join-Path $runDirectory "Saves"))
    $orb = Join-Path $scriptDestination "Custom/OrbRemoteServer/OrbServer.cs"
    [IO.File]::WriteAllText($orb, ([IO.File]::ReadAllText($orb).Replace("StartServer();", "/* Fixture: external Orb listener disabled. */")))
    $overlay = @'
using System;
using Server;
public static class StableStartupSmoke
{
    [CallPriority(Int32.MinValue)]
    public static void Configure()
    {
        Core.DataDirectories.Clear();
        Core.DataDirectories.Add(Environment.GetEnvironmentVariable("CONFICTURA_STABLE_TEST_DATA"));
    }
    [CallPriority(Int32.MaxValue)]
    public static void Initialize()
    {
        Console.WriteLine("STABLE SMOKE: production script initialization completed; fixture exits before game listeners.");
        Environment.Exit(0);
    }
}
'@
    [IO.File]::WriteAllText((Join-Path $scriptDestination "StableStartupSmoke.cs"), $overlay)
    $env:CONFICTURA_STABLE_TEST_DATA = $clientData
    try {
        Invoke-VerificationProcess -File $server -Name "startup-smoke" -Arguments "-service -nocache" -TimeoutSeconds 240
    } finally {
        Remove-Item Env:CONFICTURA_STABLE_TEST_DATA
    }
    if (-not (Get-Content -LiteralPath (Join-Path $runDirectory "Data/Logs/Console.log") -Raw).Contains("STABLE SMOKE: production script initialization completed")) {
        throw "Startup initialization milestone was not reached."
    }
    if ((Get-Content -LiteralPath (Join-Path $runDirectory "Data/Logs/Console.log") -Raw).Contains("is listening on port 2594")) {
        throw "The unrelated Orb listener was not suppressed."
    }
    Write-Host "Startup smoke passed; Orb listener suppressed, fixture exited before game listeners."
} else {
    Write-Host "Copied-world and startup verification unavailable: no local Saves/Mobiles/Mobiles.bin."
}
