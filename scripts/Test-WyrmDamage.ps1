param(
    [string]$RepositoryRoot = (Split-Path -Parent $PSScriptRoot),
    [string]$OutputDirectory = "output/wyrm-damage-verification",
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
        [int]$TimeoutSeconds = 180, [int]$MaximumExitCode = 0)

    $stdout = Join-Path $runDirectory "$Name.log"
    $stderr = Join-Path $runDirectory "$Name.stderr.log"
    $process = New-Object Diagnostics.Process
    $process.StartInfo.FileName = $File
    $process.StartInfo.Arguments = $Arguments
    $process.StartInfo.WorkingDirectory = $runDirectory
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
$testSource = Join-Path $RepositoryRoot "tests/WyrmDamageRegression.cs"
$testExe = Join-Path $runDirectory "WyrmDamageRegression.exe"
$compiler = Join-Path $env:WINDIR "Microsoft.NET/Framework/v4.0.30319/csc.exe"
Invoke-VerificationProcess -File $compiler -Name "fixture-build" `
    -Arguments ('/nologo /target:exe /platform:x86 /out:"{0}" /reference:"{1}" /reference:"{2}" "{3}"' -f $testExe, $server, $scriptAssembly, $testSource)
$clientData = Join-Path $RepositoryRoot "Data/Files"
Invoke-VerificationProcess -File $testExe -Arguments ('"{0}"' -f $clientData) -Name "regression" -TimeoutSeconds 60
Get-Content -LiteralPath (Join-Path $runDirectory "regression.log") | Write-Host
