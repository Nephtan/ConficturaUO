param(
    [string]$RepoRoot,
    [string]$OutputDir
)

Set-StrictMode -Version 2.0
$ErrorActionPreference = 'Stop'

if ([string]::IsNullOrWhiteSpace($RepoRoot))
{
    $RepoRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..\..'))
}
else
{
    $RepoRoot = [System.IO.Path]::GetFullPath($RepoRoot)
}

if ([string]::IsNullOrWhiteSpace($OutputDir))
{
    $OutputDir = Join-Path $RepoRoot 'docs\codebase-audit\outputs'
}

$RepoRoot = $RepoRoot.TrimEnd([System.IO.Path]::DirectorySeparatorChar, [System.IO.Path]::AltDirectorySeparatorChar)
$OutputDir = [System.IO.Path]::GetFullPath($OutputDir)

function Convert-ToRepoPath
{
    param([string]$Path)

    $fullPath = [System.IO.Path]::GetFullPath($Path)

    if ($fullPath.StartsWith($RepoRoot, [System.StringComparison]::OrdinalIgnoreCase))
    {
        return ($fullPath.Substring($RepoRoot.Length).TrimStart('\', '/') -replace '\\', '/')
    }

    return ($Path -replace '\\', '/')
}

function Get-LikelySystem
{
    param([string]$RepoPath)

    $normalized = $RepoPath -replace '\\', '/'

    switch -Regex ($normalized)
    {
        '^Data/Scripts/Custom/([^/]+)' { return "Custom:$($Matches[1])" }
        '^Data/Scripts/Items/([^/]+)' { return "Items:$($Matches[1])" }
        '^Data/Scripts/Magic/([^/]+)' { return "Magic:$($Matches[1])" }
        '^Data/Scripts/Mobiles/([^/]+)' { return "Mobiles:$($Matches[1])" }
        '^Data/Scripts/Quests/([^/]+)' { return "Quests:$($Matches[1])" }
        '^Data/Scripts/System/([^/]+)' { return "System:$($Matches[1])" }
        '^Data/Scripts/Trades/([^/]+)' { return "Trades:$($Matches[1])" }
        default { return 'Unknown' }
    }
}

function Export-AuditCsv
{
    param(
        [object[]]$Rows,
        [string]$FileName
    )

    $path = Join-Path $OutputDir $FileName
    $Rows | Export-Csv -LiteralPath $path -NoTypeInformation -Encoding UTF8
    return $path
}

function Export-HeaderOnlyCsv
{
    param(
        [string]$FileName,
        [string[]]$Columns
    )

    $path = Join-Path $OutputDir $FileName
    $header = (($Columns | ForEach-Object { '"' + ($_ -replace '"', '""') + '"' }) -join ',')
    Set-Content -LiteralPath $path -Value $header -Encoding UTF8
    return $path
}

function Test-GeneratedOrOutputPath
{
    param([string]$RepoPath)

    $parts = ($RepoPath -replace '\\', '/') -split '/'

    foreach ($part in $parts)
    {
        if ($part -in @('bin', 'obj', '.vs', 'Logs', 'Saves', 'Backups', '__pycache__'))
        {
            return $true
        }
    }

    return $false
}

function Get-MissingDriftClass
{
    param(
        [object]$IncludeRow,
        [hashtable]$ExistingBasenames
    )

    $path = $IncludeRow.ResolvedPath
    $decoded = $IncludeRow.DecodedIncludePath
    $include = $IncludeRow.IncludePath
    $fileName = [System.IO.Path]::GetFileName($path)

    if ($path -match '(?i)obsolete|legacy')
    {
        return 'LegacyObsoleteInclude'
    }

    if ($path -match '(?i)gump' -and $ExistingBasenames.ContainsKey($fileName))
    {
        return 'MovedGumpFolder'
    }

    if ($include -ne $decoded -and $ExistingBasenames.ContainsKey($fileName))
    {
        return 'EscapedPathMismatch'
    }

    if ($decoded -match "[\[\]']" -or $path -match '(?i)Vhaerun|Dracana|Crystal')
    {
        return 'RenamedPackagePath'
    }

    if ($ExistingBasenames.ContainsKey($fileName))
    {
        return 'RenamedPackagePath'
    }

    return 'StaleMissingCompileTarget'
}

function Get-UnincludedDriftClass
{
    param([string]$Path)

    if (Test-GeneratedOrOutputPath $Path)
    {
        return 'GeneratedOutput'
    }

    if ($Path -match '(?i)(^|[\/_. -])(backup|bak|old|unused|sample|template)([\/_. -]|$)')
    {
        return 'BackupOrOldSource'
    }

    return 'ActiveSourceMissingInclude'
}

New-Item -ItemType Directory -Force -Path $OutputDir | Out-Null

$sourceInventoryPath = Join-Path $OutputDir 'phase-01-source-files.csv'
$projectIncludesPath = Join-Path $OutputDir 'phase-01-project-includes.csv'
$runtimeMarkersPath = Join-Path $OutputDir 'phase-01-runtime-marker-inventory.csv'
$namespaceTypesPath = Join-Path $OutputDir 'phase-01-namespace-type-inventory.csv'
$solutionPath = Join-Path $RepoRoot 'ConficturaUO.sln'

$sourceRows = @(Import-Csv -LiteralPath $sourceInventoryPath | Where-Object { $_.Path -like 'Data/Scripts/*' })
$projectIncludeRows = @(Import-Csv -LiteralPath $projectIncludesPath)
$runtimeRows = @(Import-Csv -LiteralPath $runtimeMarkersPath)
$namespaceTypeRows = @(Import-Csv -LiteralPath $namespaceTypesPath | Where-Object { $_.Path -like 'Data/Scripts/*' })

$sourceSet = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
$runtimeMarkerByFile = @{}
$typeByFile = @{}
$basenameMap = @{}

foreach ($source in $sourceRows)
{
    $sourceSet.Add($source.Path) | Out-Null
    $fileName = [System.IO.Path]::GetFileName($source.Path)

    if (-not $basenameMap.ContainsKey($fileName))
    {
        $basenameMap[$fileName] = New-Object System.Collections.Generic.List[string]
    }

    $basenameMap[$fileName].Add($source.Path) | Out-Null
}

foreach ($runtime in $runtimeRows)
{
    if ($runtime.File -like 'Data/Scripts/*')
    {
        if (-not $runtimeMarkerByFile.ContainsKey($runtime.File))
        {
            $runtimeMarkerByFile[$runtime.File] = New-Object System.Collections.Generic.List[string]
        }

        $runtimeMarkerByFile[$runtime.File].Add($runtime.Marker) | Out-Null
    }
}

foreach ($typeRow in $namespaceTypeRows)
{
    $typeByFile[$typeRow.Path] = $typeRow.Types
}

$includeSet = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
$truthRows = New-Object System.Collections.Generic.List[object]
$missingRows = New-Object System.Collections.Generic.List[object]
$unincludedRows = New-Object System.Collections.Generic.List[object]

foreach ($include in $projectIncludeRows)
{
    $includeSet.Add($include.ResolvedPath) | Out-Null
    $exists = [System.Convert]::ToBoolean($include.Exists)
    $driftClass = if ($exists) { '' } else { Get-MissingDriftClass -IncludeRow $include -ExistingBasenames $basenameMap }
    $action = if ($exists) { 'Keep' } else { 'Backlog: verify owner, then remove stale include or update include to current path.' }

    $truthRows.Add([pscustomobject]@{
        RecordType = 'ProjectInclude'
        Path = $include.ResolvedPath
        IncludePath = $include.IncludePath
        ExistsOnDisk = $exists
        IncludedInScriptsProject = $true
        MissingCompileTarget = (-not $exists)
        UnincludedSource = $false
        GeneratedOrOutput = Test-GeneratedOrOutputPath $include.ResolvedPath
        DriftClass = $driftClass
        LikelySystem = $include.LikelySystem
        RuntimeMarkers = if ($runtimeMarkerByFile.ContainsKey($include.ResolvedPath)) { ($runtimeMarkerByFile[$include.ResolvedPath] | Sort-Object -Unique) -join ';' } else { '' }
        Types = if ($typeByFile.ContainsKey($include.ResolvedPath)) { $typeByFile[$include.ResolvedPath] } else { '' }
        Action = $action
    }) | Out-Null

    if (-not $exists)
    {
        $missingRows.Add([pscustomobject]@{
            Path = $include.ResolvedPath
            IncludePath = $include.IncludePath
            DriftClass = $driftClass
            LikelySystem = $include.LikelySystem
            ProposedFix = 'Verify whether the file moved or was intentionally removed; update or remove the Compile Include in a focused project-file repair batch.'
            Verification = 'Re-run New-ProjectTruthRegister.ps1, then build Data/Scripts/Scripts.csproj Debug|AnyCPU.'
        }) | Out-Null
    }
}

foreach ($source in $sourceRows)
{
    $included = $includeSet.Contains($source.Path)
    $runtimeMarkers = if ($runtimeMarkerByFile.ContainsKey($source.Path)) { ($runtimeMarkerByFile[$source.Path] | Sort-Object -Unique) -join ';' } else { '' }
    $driftClass = if ($included) { '' } else { Get-UnincludedDriftClass $source.Path }
    $action = if ($included) { 'Keep' } elseif ($driftClass -eq 'BackupOrOldSource') { 'Backlog: confirm intentional non-compiled status or remove from source tree.' } else { 'Backlog: review as active source; add Compile Include or document intentional non-compiled status.' }

    $truthRows.Add([pscustomobject]@{
        RecordType = 'SourceFile'
        Path = $source.Path
        IncludePath = ''
        ExistsOnDisk = $true
        IncludedInScriptsProject = $included
        MissingCompileTarget = $false
        UnincludedSource = (-not $included)
        GeneratedOrOutput = [System.Convert]::ToBoolean($source.GeneratedOrOutput)
        DriftClass = $driftClass
        LikelySystem = $source.LikelySystem
        RuntimeMarkers = $runtimeMarkers
        Types = if ($typeByFile.ContainsKey($source.Path)) { $typeByFile[$source.Path] } else { '' }
        Action = $action
    }) | Out-Null

    if (-not $included)
    {
        $priority = if ($runtimeMarkers -match 'CommandSystem\.Register|Initialize|EventSink|PacketHandlers|OnResponse|SendGump') { 'P1' } else { 'P2' }

        $unincludedRows.Add([pscustomobject]@{
            Path = $source.Path
            DriftClass = $driftClass
            Priority = $priority
            LikelySystem = $source.LikelySystem
            RuntimeMarkers = $runtimeMarkers
            ProposedFix = $action
            Verification = 'Re-run New-ProjectTruthRegister.ps1, then build Data/Scripts/Scripts.csproj Debug|AnyCPU if project includes change.'
        }) | Out-Null
    }
}

$backlogRows = New-Object System.Collections.Generic.List[object]
$backlogId = 1

foreach ($group in ($missingRows | Group-Object -Property DriftClass, LikelySystem | Sort-Object Count -Descending))
{
    $parts = $group.Name -split ', '
    $driftClass = $parts[0]
    $system = if ($parts.Count -gt 1) { $parts[1] } else { 'Unknown' }

    $backlogRows.Add([pscustomobject]@{
        Id = ('PT-{0:000}' -f $backlogId)
        Priority = 'P0'
        Status = 'Open'
        Category = 'MissingCompileTarget'
        DriftClass = $driftClass
        System = $system
        Count = $group.Count
        Evidence = 'phase-02-missing-compile-targets-classified.csv'
        Risk = 'Build may fail or stale project paths may hide moved or removed source files.'
        RecommendedFix = 'Review each include in the group, then remove stale entries or correct paths in a focused Scripts.csproj batch.'
        Verification = 'Re-run New-ProjectTruthRegister.ps1; run MSBuild Data/Scripts/Scripts.csproj Debug|AnyCPU.'
        Notes = ''
    }) | Out-Null

    $backlogId++
}

foreach ($group in ($unincludedRows | Group-Object -Property DriftClass, LikelySystem | Sort-Object Count -Descending))
{
    $parts = $group.Name -split ', '
    $driftClass = $parts[0]
    $system = if ($parts.Count -gt 1) { $parts[1] } else { 'Unknown' }
    $priority = if (($group.Group | Where-Object { $_.Priority -eq 'P1' } | Measure-Object).Count -gt 0) { 'P1' } else { 'P2' }

    $backlogRows.Add([pscustomobject]@{
        Id = ('PT-{0:000}' -f $backlogId)
        Priority = $priority
        Status = 'Open'
        Category = 'UnincludedSource'
        DriftClass = $driftClass
        System = $system
        Count = $group.Count
        Evidence = 'phase-02-unincluded-source-classified.csv'
        Risk = 'Active source can be silently absent from the maintained Scripts build.'
        RecommendedFix = 'Review active status and either add Compile Include entries or document intentional non-compiled status.'
        Verification = 'Re-run New-ProjectTruthRegister.ps1; run MSBuild Data/Scripts/Scripts.csproj Debug|AnyCPU if project includes change.'
        Notes = ''
    }) | Out-Null

    $backlogId++
}

$solutionLines = Get-Content -LiteralPath $solutionPath
$serverGuid = ''
$scriptsGuid = ''

foreach ($line in $solutionLines)
{
    if ($line -match 'Project\("\{[^}]+\}"\)\s*=\s*"Server",\s*"Data\\System\\Source\\Server\.csproj",\s*"\{([^}]+)\}"')
    {
        $serverGuid = $Matches[1]
    }
    elseif ($line -match 'Project\("\{[^}]+\}"\)\s*=\s*"Scripts",\s*"Data\\Scripts\\Scripts\.csproj",\s*"\{([^}]+)\}"')
    {
        $scriptsGuid = $Matches[1]
    }
}

$solutionConfigs = @('Debug|Any CPU', 'Debug|x86', 'Release|Any CPU', 'Release|x86')
$solutionRows = foreach ($config in $solutionConfigs)
{
    $serverActive = ($solutionLines | Where-Object { $_ -match "\{$serverGuid\}\.$([regex]::Escape($config))\.ActiveCfg\s*=\s*(.+)$" } | Select-Object -First 1)
    $scriptsActive = ($solutionLines | Where-Object { $_ -match "\{$scriptsGuid\}\.$([regex]::Escape($config))\.ActiveCfg\s*=\s*(.+)$" } | Select-Object -First 1)
    $serverBuild = ($solutionLines | Where-Object { $_ -match "\{$serverGuid\}\.$([regex]::Escape($config))\.Build\.0" } | Select-Object -First 1)
    $scriptsBuild = ($solutionLines | Where-Object { $_ -match "\{$scriptsGuid\}\.$([regex]::Escape($config))\.Build\.0" } | Select-Object -First 1)

    $serverConfig = if ($serverActive -match '=\s*(.+)$') { $Matches[1].Trim() } else { '' }
    $scriptsConfig = if ($scriptsActive -match '=\s*(.+)$') { $Matches[1].Trim() } else { '' }

    [pscustomobject]@{
        SolutionConfig = $config
        ServerConfig = $serverConfig
        ServerBuilds = [bool]$serverBuild
        ScriptsConfig = $scriptsConfig
        ScriptsBuilds = [bool]$scriptsBuild
        BuildsBoth = ([bool]$serverBuild -and [bool]$scriptsBuild)
    }
}

$outputPaths = @()
$outputPaths += Export-AuditCsv -Rows $truthRows -FileName 'phase-02-project-truth-register.csv'
$outputPaths += Export-AuditCsv -Rows $missingRows -FileName 'phase-02-missing-compile-targets-classified.csv'
$outputPaths += Export-AuditCsv -Rows $unincludedRows -FileName 'phase-02-unincluded-source-classified.csv'
$outputPaths += Export-AuditCsv -Rows $backlogRows -FileName 'phase-02-project-cleanup-backlog.csv'
$outputPaths += Export-AuditCsv -Rows $solutionRows -FileName 'phase-02-solution-configurations.csv'

$intentionalRows = @($unincludedRows | Where-Object { $_.DriftClass -in @('BackupOrOldSource', 'IntentionalNotCompiled', 'GeneratedOutput') })
$intentionalColumns = @('Path', 'DriftClass', 'Priority', 'LikelySystem', 'RuntimeMarkers', 'ProposedFix', 'Verification')

if ($intentionalRows.Count -gt 0)
{
    $outputPaths += Export-AuditCsv -Rows $intentionalRows -FileName 'phase-02-intentional-noncompiled-source.csv'
}
else
{
    $outputPaths += Export-HeaderOnlyCsv -FileName 'phase-02-intentional-noncompiled-source.csv' -Columns $intentionalColumns
}

$outputPaths += Export-AuditCsv -Rows $truthRows -FileName 'project-truth-register.csv'
$outputPaths += Export-AuditCsv -Rows $missingRows -FileName 'missing-compile-targets.csv'
$outputPaths += Export-AuditCsv -Rows $unincludedRows -FileName 'unincluded-source-files.csv'

if ($intentionalRows.Count -gt 0)
{
    $outputPaths += Export-AuditCsv -Rows $intentionalRows -FileName 'intentional-noncompiled-source.csv'
}
else
{
    $outputPaths += Export-HeaderOnlyCsv -FileName 'intentional-noncompiled-source.csv' -Columns $intentionalColumns
}

$outputPaths += Export-AuditCsv -Rows $backlogRows -FileName 'project-cleanup-backlog.csv'
$outputPaths += Export-AuditCsv -Rows $solutionRows -FileName 'solution-configurations.csv'

$summaryPath = Join-Path $OutputDir 'phase-02-summary.md'
$summaryLines = @(
    '# Phase 2 Build And Project Truth Summary',
    '',
    "Generated: $(Get-Date -Format o)",
    '',
    '## Required Inputs',
    '',
    '| Input | Status |',
    '| --- | --- |',
    '| Phase 1 project include parser | Present: `docs/codebase-audit/tools/Invoke-CodebaseAuditInventory.ps1` and `phase-01-project-includes.csv` |',
    '| `ConficturaUO.sln` | Present and solution configurations parsed |',
    '| `Data/Scripts/Scripts.csproj` | Present; compile includes imported from Phase 1 parser output |',
    '| `Data/System/Source/Server.csproj` | Present; referenced by solution and Scripts project |',
    '| Full source file inventory | Present: `phase-01-source-files.csv` |',
    '',
    '## Generated Outputs',
    '',
    '| Output | Rows | Purpose |',
    '| --- | ---: | --- |',
    "| `phase-02-project-truth-register.csv` | $($truthRows.Count) | One row per Scripts project include and one row per script source file. |",
    "| `phase-02-missing-compile-targets-classified.csv` | $($missingRows.Count) | Missing compile targets with drift classifications and repair verification. |",
    "| `phase-02-unincluded-source-classified.csv` | $($unincludedRows.Count) | Real script source files absent from `Scripts.csproj`, classified by likely build impact. |",
    "| `phase-02-intentional-noncompiled-source.csv` | $($intentionalRows.Count) | Sources currently classified as generated, backup, old, or intentionally not compiled. |",
    "| `phase-02-project-cleanup-backlog.csv` | $($backlogRows.Count) | Grouped project truth repair backlog. |",
    "| `phase-02-solution-configurations.csv` | $($solutionRows.Count) | Solution configuration to project configuration mapping. |",
    '',
    '## Counts',
    '',
    "| Scripts project compile includes | $($projectIncludeRows.Count) |",
    "| Script source files on disk | $($sourceRows.Count) |",
    "| Missing compile targets | $($missingRows.Count) |",
    "| Unincluded script sources | $($unincludedRows.Count) |",
    "| Backlog groups | $($backlogRows.Count) |",
    '',
    '## Build Verification',
    '',
    'Build verification is recorded separately in `phase-02-build-verification.md` after running the narrow Scripts build command.',
    '',
    '## Exit Criteria',
    '',
    '- Project truth table explains every project/source mismatch with `DriftClass`, `LikelySystem`, and `Action`.',
    '- Remaining discrepancies are grouped into `phase-02-project-cleanup-backlog.csv`.',
    '- Solution configuration mapping identifies which solution builds cover both projects.',
    '- Project repair is deferred to focused batches; no `Scripts.csproj` changes are made by Phase 2.'
)

Set-Content -LiteralPath $summaryPath -Value $summaryLines -Encoding UTF8
$outputPaths += $summaryPath

[pscustomobject]@{
    RepoRoot = $RepoRoot
    OutputDir = $OutputDir
    ProjectTruthRows = $truthRows.Count
    ScriptCompileIncludeCount = $projectIncludeRows.Count
    ScriptSourceCount = $sourceRows.Count
    MissingCompileTargetCount = $missingRows.Count
    UnincludedSourceCount = $unincludedRows.Count
    BacklogGroupCount = $backlogRows.Count
}
