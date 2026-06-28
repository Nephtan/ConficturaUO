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

New-Item -ItemType Directory -Force -Path $OutputDir | Out-Null

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

function Test-GeneratedPath
{
    param([string]$Path)

    $relativePath = Convert-ToRepoPath $Path
    $parts = $relativePath -split '/'

    foreach ($part in $parts)
    {
        if ($part -in @('bin', 'obj', '.vs', 'Logs', 'Saves', 'Backups', '__pycache__'))
        {
            return $true
        }
    }

    return $false
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
        '^Data/System/Source/' { return 'ServerCore' }
        '^docs/' { return 'Documentation' }
        default { return 'Unknown' }
    }
}

function Get-FilesUnderRoots
{
    param(
        [string[]]$Roots,
        [string[]]$Extensions
    )

    $seen = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
    $files = New-Object System.Collections.Generic.List[object]

    foreach ($root in $Roots)
    {
        $literalRoot = Join-Path $RepoRoot $root

        if (-not (Test-Path -LiteralPath $literalRoot -PathType Container))
        {
            continue
        }

        foreach ($file in Get-ChildItem -LiteralPath $literalRoot -Recurse -File)
        {
            if ($Extensions.Count -gt 0 -and ($file.Extension.ToLowerInvariant() -notin $Extensions))
            {
                continue
            }

            if (Test-GeneratedPath $file.FullName)
            {
                continue
            }

            $repoPath = Convert-ToRepoPath $file.FullName

            if ($seen.Add($repoPath))
            {
                $files.Add($file) | Out-Null
            }
        }
    }

    return $files
}

function Export-AuditCsv
{
    param(
        [object[]]$Rows,
        [string]$FileName
    )

    $path = Join-Path $OutputDir $FileName
    if ($null -eq $Rows -or @($Rows).Count -eq 0)
    {
        Set-Content -LiteralPath $path -Value $null -Encoding UTF8
        return $path
    }

    $Rows | Export-Csv -LiteralPath $path -NoTypeInformation -Encoding UTF8
    return $path
}

function Get-ShortLine
{
    param([string]$Line)

    $short = ($Line.Trim() -replace '\s+', ' ')

    if ($short.Length -gt 240)
    {
        return $short.Substring(0, 240)
    }

    return $short
}

$scriptRoots = @(
    'Data/Scripts/Custom',
    'Data/Scripts/Items',
    'Data/Scripts/Magic',
    'Data/Scripts/Mobiles',
    'Data/Scripts/Quests',
    'Data/Scripts/System',
    'Data/Scripts/Trades'
)

$sourceRoots = $scriptRoots + @('Data/System/Source')
$scriptProjectPath = Join-Path $RepoRoot 'Data/Scripts/Scripts.csproj'
$scriptsRoot = Join-Path $RepoRoot 'Data/Scripts'
$wikiIndexPath = Join-Path $RepoRoot 'docs/wiki/INDEX.md'

$allSourceFiles = @(Get-FilesUnderRoots -Roots $sourceRoots -Extensions @('.cs'))
$scriptSourceFiles = @(Get-FilesUnderRoots -Roots @('Data/Scripts') -Extensions @('.cs'))
$projectFiles = @(Get-FilesUnderRoots -Roots @('.') -Extensions @('.csproj', '.sln'))
$docFiles = @(Get-FilesUnderRoots -Roots @('docs') -Extensions @('.md'))
$configFiles = @(Get-FilesUnderRoots -Roots @('Data') -Extensions @('.xml', '.config', '.cfg', '.ini', '.json', '.txt'))
$agentFiles = @(Get-FilesUnderRoots -Roots @('.') -Extensions @('.md') | Where-Object { $_.Name -eq 'AGENTS.md' })

$sourceRows = foreach ($file in $allSourceFiles)
{
    $repoPath = Convert-ToRepoPath $file.FullName

    [pscustomobject]@{
        Path = $repoPath
        Root = ($repoPath -split '/')[0..([Math]::Min(2, (($repoPath -split '/').Length - 1)))] -join '/'
        Extension = $file.Extension
        GeneratedOrOutput = $false
        LikelySystem = Get-LikelySystem $repoPath
    }
}

$projectRows = foreach ($file in $projectFiles)
{
    $repoPath = Convert-ToRepoPath $file.FullName

    [pscustomobject]@{
        Path = $repoPath
        Kind = if ($file.Extension -eq '.sln') { 'Solution' } else { 'Project' }
    }
}

$configRows = foreach ($file in $configFiles)
{
    $repoPath = Convert-ToRepoPath $file.FullName

    [pscustomobject]@{
        Path = $repoPath
        Extension = $file.Extension
        LikelySystem = Get-LikelySystem $repoPath
    }
}

$agentRows = foreach ($file in $agentFiles)
{
    $repoPath = Convert-ToRepoPath $file.FullName
    $appliesTo = Split-Path $repoPath -Parent

    if ([string]::IsNullOrWhiteSpace($appliesTo))
    {
        $appliesTo = '.'
    }

    [pscustomobject]@{
        Path = $repoPath
        AppliesTo = $appliesTo
    }
}

$decodedIncludeRows = New-Object System.Collections.Generic.List[object]
$includeSet = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)

[xml]$scriptsProject = Get-Content -LiteralPath $scriptProjectPath
$namespaceManager = New-Object System.Xml.XmlNamespaceManager($scriptsProject.NameTable)
$namespaceManager.AddNamespace('msb', 'http://schemas.microsoft.com/developer/msbuild/2003')
$compileNodes = $scriptsProject.SelectNodes('//msb:Compile[@Include]', $namespaceManager)

foreach ($node in $compileNodes)
{
    $includePath = $node.Include
    $decodedIncludePath = [System.Uri]::UnescapeDataString($includePath)
    $resolvedPath = [System.IO.Path]::GetFullPath((Join-Path $scriptsRoot $decodedIncludePath))
    $repoPath = Convert-ToRepoPath $resolvedPath
    $exists = Test-Path -LiteralPath $resolvedPath -PathType Leaf
    $issue = if ($exists) { '' } else { 'MissingCompileTarget' }

    $includeSet.Add($repoPath) | Out-Null

    $decodedIncludeRows.Add([pscustomobject]@{
        IncludePath = $includePath
        DecodedIncludePath = $decodedIncludePath
        ResolvedPath = $repoPath
        Exists = $exists
        FilesystemMatch = $exists
        Issue = $issue
        LikelySystem = Get-LikelySystem $repoPath
    }) | Out-Null
}

$missingCompileTargetRows = @($decodedIncludeRows | Where-Object { -not $_.Exists })

$unincludedSourceRows = foreach ($file in $scriptSourceFiles)
{
    $repoPath = Convert-ToRepoPath $file.FullName

    if (-not $includeSet.Contains($repoPath))
    {
        [pscustomobject]@{
            Path = $repoPath
            IncludedInScriptsProject = $false
            Issue = 'UnincludedSource'
            LikelySystem = Get-LikelySystem $repoPath
        }
    }
}

$namespaceTypeRows = foreach ($file in $allSourceFiles)
{
    $repoPath = Convert-ToRepoPath $file.FullName
    $text = Get-Content -Raw -LiteralPath $file.FullName
    $namespaceMatch = [regex]::Match($text, '(?m)^\s*namespace\s+([A-Za-z0-9_.]+)')
    $typeMatches = [regex]::Matches($text, '(?m)^\s*(?:public|private|internal|protected|sealed|abstract|static|partial|\s)*\s*(class|struct|enum|interface)\s+([A-Za-z_][A-Za-z0-9_]*)')
    $types = @()

    foreach ($match in $typeMatches)
    {
        $types += "$($match.Groups[1].Value):$($match.Groups[2].Value)"
    }

    [pscustomobject]@{
        Path = $repoPath
        Namespace = if ($namespaceMatch.Success) { $namespaceMatch.Groups[1].Value } else { '' }
        Types = ($types -join ';')
        LikelySystem = Get-LikelySystem $repoPath
    }
}

$runtimePatterns = @(
    @{ Marker = 'Initialize'; Pattern = 'public\s+static\s+void\s+Initialize\s*\(' },
    @{ Marker = 'CommandSystem.Register'; Pattern = 'CommandSystem\.Register\s*\(' },
    @{ Marker = 'EventSink'; Pattern = 'EventSink\.' },
    @{ Marker = 'PacketHandlers.Register'; Pattern = 'PacketHandlers\.Register\s*\(' },
    @{ Marker = 'Timer.DelayCall'; Pattern = 'Timer\.DelayCall\s*\(' },
    @{ Marker = 'CustomTimerSubclass'; Pattern = ':\s*Timer\b' },
    @{ Marker = 'OnSpeech'; Pattern = '\bOnSpeech\s*\(' },
    @{ Marker = 'OnMovement'; Pattern = '\bOnMovement\s*\(' },
    @{ Marker = 'OnLogin'; Pattern = '\bOnLogin\s*\(' },
    @{ Marker = 'OnLogout'; Pattern = '\bOnLogout\s*\(' },
    @{ Marker = 'WorldSave'; Pattern = '\bWorldSave\b|World\.Save|EventSink\.WorldSave' },
    @{ Marker = 'WorldLoad'; Pattern = '\bWorldLoad\b|World\.Load|EventSink\.WorldLoad' },
    @{ Marker = 'OnResponse'; Pattern = '\bOnResponse\s*\(' },
    @{ Marker = 'RegionOverride'; Pattern = ':\s*Region\b|override\s+.*\b(OnEnter|OnExit|OnMoveInto|AllowHarmful|AllowBeneficial)\b' },
    @{ Marker = 'SendGump'; Pattern = '\.SendGump\s*\(' }
)

$sourceLiteralPaths = @($allSourceFiles | ForEach-Object { $_.FullName })
$runtimeMarkerRows = New-Object System.Collections.Generic.List[object]

foreach ($marker in $runtimePatterns)
{
    $matches = Select-String -LiteralPath $sourceLiteralPaths -Pattern $marker.Pattern -AllMatches

    foreach ($match in $matches)
    {
        $repoPath = Convert-ToRepoPath $match.Path

        $runtimeMarkerRows.Add([pscustomobject]@{
            Marker = $marker.Marker
            File = $repoPath
            Line = $match.LineNumber
            LikelySystem = Get-LikelySystem $repoPath
            Notes = Get-ShortLine $match.Line
        }) | Out-Null
    }
}

$commandRows = foreach ($match in ($runtimeMarkerRows | Where-Object { $_.Marker -eq 'CommandSystem.Register' }))
{
    $lineText = $match.Notes
    $commandName = ''
    $accessLevel = ''
    $handler = ''

    if ($lineText -match 'Register\s*\(\s*"([^"]+)"')
    {
        $commandName = $Matches[1]
    }

    if ($lineText -match 'AccessLevel\.([A-Za-z0-9_]+)')
    {
        $accessLevel = $Matches[1]
    }

    if ($lineText -match 'CommandEventHandler\s*\(\s*([A-Za-z0-9_.]+)\s*\)')
    {
        $handler = $Matches[1]
    }

    [pscustomobject]@{
        Command = $commandName
        AccessLevel = $accessLevel
        Handler = $handler
        File = $match.File
        Line = $match.Line
        LikelySystem = $match.LikelySystem
        RegistrationLine = $lineText
    }
}

$eventPacketRows = foreach ($match in ($runtimeMarkerRows | Where-Object { $_.Marker -in @('EventSink', 'PacketHandlers.Register', 'Timer.DelayCall', 'CustomTimerSubclass', 'WorldSave', 'WorldLoad') }))
{
    [pscustomobject]@{
        HookType = $match.Marker
        File = $match.File
        Line = $match.Line
        LikelySystem = $match.LikelySystem
        Evidence = $match.Notes
    }
}

$gumpRows = foreach ($match in ($runtimeMarkerRows | Where-Object { $_.Marker -in @('OnResponse', 'SendGump') }))
{
    [pscustomobject]@{
        GumpMarker = $match.Marker
        File = $match.File
        Line = $match.Line
        LikelySystem = $match.LikelySystem
        Evidence = $match.Notes
    }
}

$serializationRows = foreach ($file in $allSourceFiles)
{
    $repoPath = Convert-ToRepoPath $file.FullName
    $serializeLines = @(Select-String -LiteralPath $file.FullName -Pattern 'Serialize\s*\(\s*GenericWriter' | ForEach-Object { $_.LineNumber })
    $deserializeLines = @(Select-String -LiteralPath $file.FullName -Pattern 'Deserialize\s*\(\s*GenericReader' | ForEach-Object { $_.LineNumber })
    $serialConstructorLines = @(Select-String -LiteralPath $file.FullName -Pattern '\(\s*Serial\s+serial\s*\)' | ForEach-Object { $_.LineNumber })
    $writerWriteLines = @(Select-String -LiteralPath $file.FullName -Pattern 'writer\.Write\s*\(' | ForEach-Object { $_.LineNumber })
    $readerReadLines = @(Select-String -LiteralPath $file.FullName -Pattern 'reader\.Read[A-Za-z0-9_]*\s*\(' | ForEach-Object { $_.LineNumber })
    $versionWriteLines = @(Select-String -LiteralPath $file.FullName -Pattern 'writer\.Write\s*\(\s*\(int\)\s*[0-9]+' | ForEach-Object { $_.LineNumber })
    $versionReadLines = @(Select-String -LiteralPath $file.FullName -Pattern '\bversion\s*=\s*reader\.ReadInt\s*\(' | ForEach-Object { $_.LineNumber })

    if ($serializeLines.Count -gt 0 -or $deserializeLines.Count -gt 0 -or $serialConstructorLines.Count -gt 0 -or $writerWriteLines.Count -gt 0 -or $readerReadLines.Count -gt 0)
    {
        $versionPattern = if ($versionWriteLines.Count -gt 0 -and $versionReadLines.Count -gt 0) { 'WriteReadVersion' } elseif ($versionWriteLines.Count -gt 0) { 'WriteVersionOnly' } elseif ($versionReadLines.Count -gt 0) { 'ReadVersionOnly' } else { 'NoVersionMarker' }

        [pscustomobject]@{
            File = $repoPath
            LikelySystem = Get-LikelySystem $repoPath
            SerializeLine = ($serializeLines -join ';')
            DeserializeLine = ($deserializeLines -join ';')
            SerialConstructorLine = ($serialConstructorLines -join ';')
            WriterWriteCount = $writerWriteLines.Count
            ReaderReadCount = $readerReadLines.Count
            VersionWriteLine = ($versionWriteLines -join ';')
            VersionReadLine = ($versionReadLines -join ';')
            VersionPattern = $versionPattern
        }
    }
}

$configReferenceRows = foreach ($file in $allSourceFiles)
{
    $repoPath = Convert-ToRepoPath $file.FullName
    $matches = Select-String -LiteralPath $file.FullName -Pattern '"([^"]+\.(xml|config|cfg|ini|json|txt))"' -AllMatches

    foreach ($match in $matches)
    {
        foreach ($capture in $match.Matches)
        {
            [pscustomobject]@{
                File = $repoPath
                Line = $match.LineNumber
                LikelySystem = Get-LikelySystem $repoPath
                Reference = $capture.Groups[1].Value
                Evidence = Get-ShortLine $match.Line
            }
        }
    }
}

$wikiIndexText = ''

if (Test-Path -LiteralPath $wikiIndexPath -PathType Leaf)
{
    $wikiIndexText = Get-Content -Raw -LiteralPath $wikiIndexPath
}

$documentationRows = foreach ($file in $docFiles)
{
    $repoPath = Convert-ToRepoPath $file.FullName
    $text = Get-Content -Raw -LiteralPath $file.FullName
    $normalizedSlug = ([System.IO.Path]::GetFileNameWithoutExtension($file.Name).ToLowerInvariant() -replace '[^a-z0-9]+', '-').Trim('-')
    $localLinks = [regex]::Matches($text, '\[[^\]]+\]\((?!https?:|mailto:|#)([^)]+\.md(?:#[^)]+)?)\)').Count
    $canonicalOrAlias = 'Unknown'

    if ($text -match '(?i)\balias\b|legacy slug|canonical target|canonical page')
    {
        $canonicalOrAlias = 'AliasOrCanonicalMention'
    }

    [pscustomobject]@{
        DocPath = $repoPath
        Indexed = ($wikiIndexText -like "*$($file.Name)*")
        SourceTrace = ($text -match '(?im)^##\s+Source Trace\b')
        CodeVerified = ($text -match '(?i)Code-Verified|Code Verified')
        NeedsRework = ($text -match '(?i)Needs Rework')
        LegacySlug = ($text -match '(?i)legacy slug')
        LocalMarkdownLinkCount = $localLinks
        NormalizedSlug = $normalizedSlug
        CanonicalOrAlias = $canonicalOrAlias
    }
}

$docSlugGroups = $documentationRows | Group-Object -Property NormalizedSlug | Where-Object { $_.Count -gt 1 }
$duplicateSlugRows = foreach ($group in $docSlugGroups)
{
    [pscustomobject]@{
        NormalizedSlug = $group.Name
        Count = $group.Count
        Paths = (($group.Group | ForEach-Object { $_.DocPath }) -join ';')
    }
}

$outputPaths = @()
$outputPaths += Export-AuditCsv -Rows $sourceRows -FileName 'phase-01-source-files.csv'
$outputPaths += Export-AuditCsv -Rows $projectRows -FileName 'phase-01-project-files.csv'
$outputPaths += Export-AuditCsv -Rows $configRows -FileName 'phase-01-config-files.csv'
$outputPaths += Export-AuditCsv -Rows $agentRows -FileName 'phase-01-agents.csv'
$outputPaths += Export-AuditCsv -Rows $decodedIncludeRows -FileName 'phase-01-project-includes.csv'
$outputPaths += Export-AuditCsv -Rows $missingCompileTargetRows -FileName 'phase-01-missing-compile-targets.csv'
$outputPaths += Export-AuditCsv -Rows $unincludedSourceRows -FileName 'phase-01-unincluded-source-files.csv'
$outputPaths += Export-AuditCsv -Rows $namespaceTypeRows -FileName 'phase-01-namespace-type-inventory.csv'
$outputPaths += Export-AuditCsv -Rows $runtimeMarkerRows -FileName 'phase-01-runtime-marker-inventory.csv'
$outputPaths += Export-AuditCsv -Rows $commandRows -FileName 'phase-01-command-registration-inventory.csv'
$outputPaths += Export-AuditCsv -Rows $eventPacketRows -FileName 'phase-01-event-packet-hook-inventory.csv'
$outputPaths += Export-AuditCsv -Rows $serializationRows -FileName 'phase-01-serialization-marker-inventory.csv'
$outputPaths += Export-AuditCsv -Rows $gumpRows -FileName 'phase-01-gump-inventory.csv'
$outputPaths += Export-AuditCsv -Rows $configReferenceRows -FileName 'phase-01-config-reference-inventory.csv'
$outputPaths += Export-AuditCsv -Rows $documentationRows -FileName 'phase-01-documentation-inventory.csv'
$outputPaths += Export-AuditCsv -Rows $duplicateSlugRows -FileName 'phase-01-duplicate-doc-slugs.csv'

$summaryPath = Join-Path $OutputDir 'phase-01-summary.md'
$summaryLines = @(
    '# Phase 1 Reproducible Inventory Summary',
    '',
    "Generated: $(Get-Date -Format o)",
    '',
    '## Required Inputs',
    '',
    '| Input | Status |',
    '| --- | --- |',
    "| Phase 0 baseline | Present: `docs/codebase-audit/outputs/phase-00-baseline.md` |",
    "| `ConficturaUO.sln` | Present |",
    "| `Data/Scripts/Scripts.csproj` | Present and parsed as XML |",
    "| `Data/System/Source/Server.csproj` | Present |",
    "| Script roots under `Data/Scripts` | Present and scanned with literal paths |",
    "| Documentation roots under `docs` | Present and scanned |",
    '',
    '## Generated Outputs',
    '',
    '| Output | Rows | Purpose |',
    '| --- | ---: | --- |',
    "| `phase-01-source-files.csv` | $($sourceRows.Count) | All `.cs` source files under audit roots, excluding generated output folders. |",
    "| `phase-01-project-files.csv` | $($projectRows.Count) | `.sln` and `.csproj` files. |",
    "| `phase-01-config-files.csv` | $($configRows.Count) | XML/config/data files under `Data`, excluding generated output folders. |",
    "| `phase-01-agents.csv` | $($agentRows.Count) | Instruction scope files. |",
    "| `phase-01-project-includes.csv` | $($decodedIncludeRows.Count) | `Scripts.csproj` compile includes decoded and resolved with literal path checks. |",
    "| `phase-01-missing-compile-targets.csv` | $($missingCompileTargetRows.Count) | Compile includes whose target file does not exist. |",
    "| `phase-01-unincluded-source-files.csv` | $(@($unincludedSourceRows).Count) | Real script `.cs` files absent from `Scripts.csproj`. |",
    "| `phase-01-namespace-type-inventory.csv` | $($namespaceTypeRows.Count) | Namespace and declared type markers. |",
    "| `phase-01-runtime-marker-inventory.csv` | $($runtimeMarkerRows.Count) | Runtime entry marker hits for commands, hooks, timers, gumps, and regions. |",
    "| `phase-01-command-registration-inventory.csv` | $(@($commandRows).Count) | Command registration marker hits. |",
    "| `phase-01-event-packet-hook-inventory.csv` | $(@($eventPacketRows).Count) | Event, packet, timer, and world hook marker hits. |",
    "| `phase-01-serialization-marker-inventory.csv` | $(@($serializationRows).Count) | Serializer-related marker summary by file. |",
    "| `phase-01-gump-inventory.csv` | $(@($gumpRows).Count) | Gump open and response marker hits. |",
    "| `phase-01-config-reference-inventory.csv` | $(@($configReferenceRows).Count) | String references to XML/config/text/json data files in source. |",
    "| `phase-01-documentation-inventory.csv` | $($documentationRows.Count) | Markdown inventory with source-trace, code-verified, needs-rework, slug, and link markers. |",
    "| `phase-01-duplicate-doc-slugs.csv` | $(@($duplicateSlugRows).Count) | Duplicate normalized documentation slugs. |",
    '',
    '## Reproduction Command',
    '',
    'Run from the repository root:',
    '',
    '```powershell',
    '.\docs\codebase-audit\tools\Invoke-CodebaseAuditInventory.ps1',
    '```',
    '',
    'The script uses PowerShell XML parsing for `Scripts.csproj`, `Test-Path -LiteralPath` for include existence checks, and generated-output filtering for `bin`, `obj`, `.vs`, `Logs`, `Saves`, `Backups`, and `__pycache__` path segments.',
    '',
    '## Storage Policy',
    '',
    'The script is committed under `docs/codebase-audit/tools/`. Curated Phase 1 inventories are committed under `docs/codebase-audit/outputs/` even though that directory is ignored by local Git exclude rules, so they must be force-staged intentionally.',
    '',
    '## Exit Criteria',
    '',
    '- Inventory can be regenerated from a clean checkout with one script command.',
    '- Source files are distinguished from generated `bin` and `obj` outputs.',
    '- Project include matching uses URI decoding and literal path checks, including paths with spaces, apostrophes, and brackets.',
    '- Runtime, serialization, gump, command, documentation, and config reference marker inventories exist for later phases.'
)

Set-Content -LiteralPath $summaryPath -Value $summaryLines -Encoding UTF8
$outputPaths += $summaryPath

[pscustomobject]@{
    RepoRoot = $RepoRoot
    OutputDir = $OutputDir
    OutputCount = $outputPaths.Count
    SourceFileCount = $sourceRows.Count
    ScriptCompileIncludeCount = $decodedIncludeRows.Count
    MissingCompileTargetCount = $missingCompileTargetRows.Count
    UnincludedSourceCount = @($unincludedSourceRows).Count
    RuntimeMarkerCount = $runtimeMarkerRows.Count
    SerializationMarkerFileCount = @($serializationRows).Count
    DocumentationFileCount = $documentationRows.Count
}
