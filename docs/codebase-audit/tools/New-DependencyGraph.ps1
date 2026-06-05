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

function Escape-MarkdownCell
{
    param([string]$Value)

    if ($null -eq $Value)
    {
        return ''
    }

    return (($Value -replace '\|', '\|') -replace "`r?`n", ' ')
}

function Join-Unique
{
    param([object[]]$Values)

    return (($Values | Where-Object { -not [string]::IsNullOrWhiteSpace([string]$_) } | Sort-Object -Unique) -join ';')
}

function Trim-Text
{
    param(
        [string]$Value,
        [int]$MaxLength = 220
    )

    if ($null -eq $Value)
    {
        return ''
    }

    $collapsed = (($Value -replace '\s+', ' ') -replace "`r?`n", ' ').Trim()

    if ($collapsed.Length -le $MaxLength)
    {
        return $collapsed
    }

    return $collapsed.Substring(0, $MaxLength - 3) + '...'
}

function Add-Edge
{
    param(
        [System.Collections.Generic.List[object]]$Edges,
        [System.Collections.Generic.HashSet[string]]$Seen,
        [string]$SourceSystem,
        [string]$TargetSystem,
        [string]$EdgeType,
        [string]$Evidence,
        [string]$Strength,
        [string]$Impact,
        [string]$Notes,
        [string]$EvidenceSource
    )

    if ([string]::IsNullOrWhiteSpace($SourceSystem) -or [string]::IsNullOrWhiteSpace($TargetSystem) -or $SourceSystem -eq $TargetSystem)
    {
        return
    }

    $key = "$SourceSystem|$TargetSystem|$EdgeType|$Evidence"

    if (-not $Seen.Add($key))
    {
        return
    }

    $Edges.Add([pscustomobject]@{
        SourceSystem = $SourceSystem
        TargetSystem = $TargetSystem
        EdgeType = $EdgeType
        Evidence = $Evidence
        Strength = $Strength
        Impact = $Impact
        Notes = $Notes
        EvidenceSource = $EvidenceSource
    }) | Out-Null
}

function Get-OwnerSystemsForFile
{
    param(
        [string]$File,
        [hashtable]$OwnerByFile,
        [hashtable]$RuntimeByFile
    )

    if ($OwnerByFile.ContainsKey($File))
    {
        return @($OwnerByFile[$File].ToArray())
    }

    if ($RuntimeByFile.ContainsKey($File))
    {
        return @($RuntimeByFile[$File].System)
    }

    return @()
}

function Get-FrameworkTargetForHook
{
    param([string]$HookType)

    switch ($HookType)
    {
        'Command' { return 'RunUO Command System' }
        'Packet' { return 'RunUO Packet Handlers' }
        'Gump' { return 'Gump Framework' }
        'Region' { return 'Region Framework' }
        'Timer' { return 'Timer Scheduler' }
        'Initialize' { return 'Script Startup' }
        'Event' { return 'EventSink' }
        'Login' { return 'EventSink' }
        'Logout' { return 'EventSink' }
        'WorldSave' { return 'World Save EventSink' }
        'WorldLoad' { return 'World Load EventSink' }
        'Speech' { return 'Mobile Speech Hooks' }
        'Movement' { return 'Mobile Movement Hooks' }
        default { return 'Runtime Framework' }
    }
}

function Get-HookImpact
{
    param([string]$HookType)

    switch ($HookType)
    {
        'Packet' { return 'Runtime;Network' }
        'Command' { return 'Runtime;Staff;Player' }
        'Gump' { return 'Runtime;Player;Staff' }
        'Region' { return 'Runtime;Gameplay' }
        'WorldSave' { return 'Runtime;Save' }
        'WorldLoad' { return 'Runtime;Save' }
        default { return 'Runtime' }
    }
}

function Get-SerializationTargets
{
    param([object]$Row)

    $targets = New-Object System.Collections.Generic.List[string]
    $combined = "$($Row.Writes) $($Row.Reads) $($Row.Kind) $($Row.BaseType) $($Row.Class)"

    if ($combined -match 'ReadMobile|WriteMobile|\(Mobile\)|PlayerMobile|BaseCreature|Mobile')
    {
        $targets.Add('Mobile Object Graph') | Out-Null
    }

    if ($combined -match 'ReadItem|WriteItem|\(Item\)|BaseContainer|BaseWeapon|BaseArmor|Item')
    {
        $targets.Add('Item Object Graph') | Out-Null
    }

    if ($combined -match 'ReadMap|WriteMap|\(Map\)|Map')
    {
        $targets.Add('Map Object Graph') | Out-Null
    }

    if ($combined -match 'Region')
    {
        $targets.Add('Region Framework') | Out-Null
    }

    if ($targets.Count -eq 0)
    {
        $targets.Add('World Save Loader') | Out-Null
    }
    else
    {
        $targets.Add('World Save Loader') | Out-Null
    }

    return @($targets.ToArray() | Sort-Object -Unique)
}

function Get-MarkdownLinks
{
    param(
        [string]$DocPath,
        [string]$Content
    )

    $links = New-Object System.Collections.Generic.List[string]
    $baseDirectory = Split-Path -Path $DocPath -Parent

    foreach ($match in [regex]::Matches($Content, '\[([^\]]+)\]\(([^)]+\.md(?:#[^)]+)?)\)'))
    {
        $target = ($match.Groups[2].Value -split '#')[0]

        if ($target -match '^[a-zA-Z][a-zA-Z0-9+.-]*:')
        {
            continue
        }

        $combined = Join-Path (Join-Path $RepoRoot ($baseDirectory -replace '/', '\')) ([System.Uri]::UnescapeDataString($target) -replace '/', '\')
        $links.Add((Convert-ToRepoPath $combined)) | Out-Null
    }

    return @($links.ToArray() | Sort-Object -Unique)
}

New-Item -ItemType Directory -Force -Path $OutputDir | Out-Null

$systemCards = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'phase-04-system-card-index.csv'))
$ownerMap = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'system-owner-map.csv'))
$runtimeInventory = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'cross-tree-runtime-inventory.csv'))
$runtimeHookMap = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'runtime-hook-map.csv'))
$serializationRegister = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'serialization-register.csv'))
$documentationTruth = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'documentation-truth-table.csv'))
$projectTruth = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'project-truth-register.csv'))
$configReferences = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'phase-01-config-reference-inventory.csv'))

$edges = New-Object System.Collections.Generic.List[object]
$seenEdges = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)

$ownerByFile = @{}
foreach ($row in $ownerMap)
{
    if (-not $ownerByFile.ContainsKey($row.File))
    {
        $ownerByFile[$row.File] = New-Object System.Collections.Generic.List[string]
    }

    if (-not $ownerByFile[$row.File].Contains($row.System))
    {
        $ownerByFile[$row.File].Add($row.System) | Out-Null
    }
}

$runtimeByFile = @{}
foreach ($row in $runtimeInventory)
{
    $runtimeByFile[$row.Path] = $row
}

$targetPatterns = @{
    'Character Level' = @('CharacterLevelService', 'CharacterLevel')
    'Random Encounters' = @('RandomEncounter', 'EncounterEngine', 'EncounterSpawn', 'EncounterDefinition')
    'PvP Consent' = @('NONPK', 'PKNONPK', 'ChoosePvP', 'GoddessOfProtection', 'NONPKEvent')
    'Government' = @('PlayerGovernment', 'GovernmentSystem', 'CityTreasury', 'PlayerCity', 'MayorGump')
    'Homestead' = @('Homestead', 'Winecraft', 'HungerGump', 'CRL', 'BeeHive')
    'XMLSpawner' = @('XmlSpawner', 'XmlAttach', 'XmlQuest', 'XmlPoints', 'XmlFind')
    'Invasion' = @('InvasionSystem', 'Invasion')
    'Champions' = @('ChampionSpawn', 'CannedEvil', 'ChampionSkull')
    'Monster Nests' = @('MonsterNest')
    'Clone Offline Player Characters' = @('CloneOffline', 'CharacterClone', 'CheckClones')
    'Offline Skill Training' = @('OfflineSkill')
    'OmniAI' = @('OmniAI')
    'AI Overhaul' = @('AIOverhaul', 'AITactical')
    'Staff Toolbar' = @('StaffToolbar')
    'Static Gump Tool' = @('StaticGump', 'StaticsTool', 'STool')
    'Spell Framework' = @('SpellRegistry', 'Spellbook', 'BaseSpell', 'SpellInfo')
    'Magic Schools' = @('BardSpell', 'DeathKnight', 'Elementalism', 'HolyMan', 'JediSpell', 'MysticSpell', 'ResearchSpell', 'SythSpell', 'WitchSpell')
    'Crafting Core' = @('CraftSystem', 'CraftGump', 'CraftItem', 'DefBlacksmithy', 'DefTailoring')
    'Harvest System' = @('HarvestSystem', 'HarvestDefinition')
    'Bulk Orders' = @('BulkOrder', 'LargeBOD', 'SmallBOD')
    'Gardening' = @('PlantSystem', 'PlantBowl', 'PlantItem', 'Gardening')
    'Housing' = @('BaseHouse', 'HouseSign', 'HouseFoundation')
    'Boats' = @('BaseBoat', 'BoatNavigation', 'TillerMan', 'Shipwright')
    'Regions' = @('BaseRegion', 'GuardedRegion', 'Region')
    'PlayerMobile Core' = @('PlayerMobile')
    'Vendor Core' = @('BaseVendor', 'SBInfo', 'Vendor')
    'Obsolete Scripts' = @('Obsolete')
}

foreach ($sourceRow in $ownerMap)
{
    $sourceSystem = $sourceRow.System
    $literalFile = Join-Path $RepoRoot ($sourceRow.File -replace '/', '\')

    if (-not (Test-Path -LiteralPath $literalFile -PathType Leaf))
    {
        continue
    }

    $lines = [string[]](Get-Content -LiteralPath $literalFile)
    $contentLower = (($lines -join "`n").ToLowerInvariant())

    foreach ($targetSystem in $targetPatterns.Keys)
    {
        if ($targetSystem -eq $sourceSystem)
        {
            continue
        }

        $found = $false

        foreach ($pattern in $targetPatterns[$targetSystem])
        {
            if (-not $contentLower.Contains($pattern.ToLowerInvariant()))
            {
                continue
            }

            for ($i = 0; $i -lt $lines.Count; $i++)
            {
                if ($lines[$i].IndexOf($pattern, [System.StringComparison]::OrdinalIgnoreCase) -ge 0)
                {
                    Add-Edge -Edges $edges -Seen $seenEdges -SourceSystem $sourceSystem -TargetSystem $targetSystem -EdgeType 'DirectReference' -Evidence ("{0}:L{1}:{2}" -f $sourceRow.File, ($i + 1), $pattern) -Strength 'Hard' -Impact 'Build;Runtime' -Notes 'Source file owned by source system contains a target-system symbol or package marker.' -EvidenceSource 'Source'
                    $found = $true
                    break
                }
            }

            if ($found)
            {
                break
            }
        }
    }
}

foreach ($row in $runtimeHookMap)
{
    $target = Get-FrameworkTargetForHook $row.HookType
    $impact = Get-HookImpact $row.HookType
    $edgeType = if ($row.HookType -in @('Event', 'Login', 'Logout', 'WorldSave', 'WorldLoad')) { 'GlobalHook' } elseif ($row.HookType -eq 'Packet') { 'PacketHandler' } else { 'RuntimeHook' }
    $evidence = "{0}:L{1}:{2}" -f $row.File, $row.Line, $row.HookType

    Add-Edge -Edges $edges -Seen $seenEdges -SourceSystem $row.System -TargetSystem $target -EdgeType $edgeType -Evidence $evidence -Strength 'Hard' -Impact $impact -Notes "Runtime hook map row; trigger: $($row.Trigger)." -EvidenceSource 'RuntimeHookMap'
}

foreach ($row in $serializationRegister)
{
    $sourceSystems = New-Object System.Collections.Generic.List[string]

    if (-not [string]::IsNullOrWhiteSpace($row.CardSystems))
    {
        foreach ($system in ($row.CardSystems -split ';'))
        {
            if (-not [string]::IsNullOrWhiteSpace($system) -and -not $sourceSystems.Contains($system))
            {
                $sourceSystems.Add($system) | Out-Null
            }
        }
    }

    if ($sourceSystems.Count -eq 0 -and -not [string]::IsNullOrWhiteSpace($row.System))
    {
        $sourceSystems.Add($row.System) | Out-Null
    }

    foreach ($sourceSystem in $sourceSystems.ToArray())
    {
        foreach ($target in Get-SerializationTargets $row)
        {
            Add-Edge -Edges $edges -Seen $seenEdges -SourceSystem $sourceSystem -TargetSystem $target -EdgeType 'Serialization' -Evidence ("{0}:{1}:v{2}" -f $row.File, $row.Class, $row.CurrentVersion) -Strength 'Hard' -Impact 'Save' -Notes "Serialized kind $($row.Kind); move risk $($row.MoveRenameRisk); review status $($row.ReviewStatus)." -EvidenceSource 'SerializationRegister'
        }
    }
}

$projectGroups = @($projectTruth | Where-Object { $_.RecordType -eq 'ProjectInclude' } | Group-Object LikelySystem)
foreach ($group in $projectGroups)
{
    if ([string]::IsNullOrWhiteSpace($group.Name))
    {
        continue
    }

    $includedCount = @($group.Group | Where-Object { $_.IncludedInScriptsProject -eq 'True' }).Count
    Add-Edge -Edges $edges -Seen $seenEdges -SourceSystem 'Scripts.csproj' -TargetSystem $group.Name -EdgeType 'ProjectInclude' -Evidence ("{0} compile include rows" -f $includedCount) -Strength 'Hard' -Impact 'Build' -Notes 'Scripts.csproj explicitly includes source files for this likely system.' -EvidenceSource 'ProjectTruthRegister'
}

$driftGroups = @($projectTruth | Where-Object { $_.MissingCompileTarget -eq 'True' -or $_.UnincludedSource -eq 'True' } | Group-Object LikelySystem)
foreach ($group in $driftGroups)
{
    if ([string]::IsNullOrWhiteSpace($group.Name))
    {
        continue
    }

    $missingCount = @($group.Group | Where-Object { $_.MissingCompileTarget -eq 'True' }).Count
    $unincludedCount = @($group.Group | Where-Object { $_.UnincludedSource -eq 'True' }).Count
    Add-Edge -Edges $edges -Seen $seenEdges -SourceSystem 'Scripts.csproj' -TargetSystem $group.Name -EdgeType 'ProjectTruthConflict' -Evidence ("MissingCompileTargets={0};UnincludedSources={1}" -f $missingCount, $unincludedCount) -Strength 'Hard' -Impact 'Build' -Notes 'Project/source truth mismatch affects whether this system is compiled as expected.' -EvidenceSource 'ProjectTruthRegister'
}

foreach ($row in $configReferences)
{
    $sourceSystems = @(Get-OwnerSystemsForFile -File $row.File -OwnerByFile $ownerByFile -RuntimeByFile $runtimeByFile)

    if ($sourceSystems.Count -eq 0)
    {
        $sourceSystems = @($row.LikelySystem)
    }

    foreach ($sourceSystem in $sourceSystems)
    {
        $strength = if ($row.Reference -match '^\*') { 'Soft' } else { 'Hard' }
        Add-Edge -Edges $edges -Seen $seenEdges -SourceSystem $sourceSystem -TargetSystem ("DataFile:{0}" -f $row.Reference) -EdgeType 'XMLConfig' -Evidence ("{0}:L{1}:{2}" -f $row.File, $row.Line, (Trim-Text $row.Evidence 120)) -Strength $strength -Impact 'Runtime;Data' -Notes 'Source string references checked-in data/config path or wildcard.' -EvidenceSource 'ConfigReferenceInventory'
    }
}

foreach ($row in $documentationTruth)
{
    if ($row.Scope -eq 'Support' -or $row.Scope -eq 'WikiSupport')
    {
        continue
    }

    if ([string]::IsNullOrWhiteSpace($row.DocPath))
    {
        continue
    }

    $literal = Join-Path $RepoRoot ($row.DocPath -replace '/', '\')

    if (-not (Test-Path -LiteralPath $literal -PathType Leaf))
    {
        continue
    }

    $content = Get-Content -Raw -LiteralPath $literal
    $sourceName = if (-not [string]::IsNullOrWhiteSpace($row.Title)) { "Doc:$($row.Title)" } else { "Doc:$($row.DocPath)" }

    foreach ($linkedDoc in Get-MarkdownLinks -DocPath $row.DocPath -Content $content)
    {
        if ($linkedDoc -match '^docs/wiki/' -and $linkedDoc -ne $row.DocPath)
        {
            Add-Edge -Edges $edges -Seen $seenEdges -SourceSystem $sourceName -TargetSystem ("Doc:{0}" -f $linkedDoc) -EdgeType 'DocsOnly' -Evidence $row.DocPath -Strength 'Speculative' -Impact 'Docs' -Notes 'Markdown link suggests a documentation relationship only; runtime dependency requires source evidence.' -EvidenceSource 'DocumentationTruthTable'
        }
    }

    if ($row.SourceTracePresent -eq 'Yes' -and -not [string]::IsNullOrWhiteSpace($row.VerifiedSourceFiles))
    {
        foreach ($sourceFile in ($row.VerifiedSourceFiles -split ';'))
        {
            foreach ($targetSystem in @(Get-OwnerSystemsForFile -File $sourceFile -OwnerByFile $ownerByFile -RuntimeByFile $runtimeByFile))
            {
                Add-Edge -Edges $edges -Seen $seenEdges -SourceSystem $sourceName -TargetSystem $targetSystem -EdgeType 'DocsSourceTrace' -Evidence ("{0} -> {1}" -f $row.DocPath, $sourceFile) -Strength 'Soft' -Impact 'Docs' -Notes 'Documentation page source trace points at files owned by the target system.' -EvidenceSource 'DocumentationTruthTable'
            }
        }
    }
}

foreach ($row in $documentationTruth)
{
    if ($row.MissingClaims -match 'independent behavior claims')
    {
        Add-Edge -Edges $edges -Seen $seenEdges -SourceSystem 'Documentation' -TargetSystem $row.DocPath -EdgeType 'DocumentationConflict' -Evidence $row.DocPath -Strength 'Speculative' -Impact 'Docs' -Notes 'Alias page carries independent behavior claims and should be reconciled with canonical documentation.' -EvidenceSource 'DocumentationTruthTable'
    }
}

foreach ($row in $serializationRegister)
{
    if ($row.FieldAlignment -match 'Mismatch' -or $row.VersionHandling -in @('NoVersionFound', 'WriteVersionOnly', 'ReadVersionOnly', 'SuspiciousOrder'))
    {
        Add-Edge -Edges $edges -Seen $seenEdges -SourceSystem $row.System -TargetSystem 'World Save Loader' -EdgeType 'SerializationConflict' -Evidence ("{0}:{1}:{2}:{3}" -f $row.File, $row.Class, $row.VersionHandling, $row.FieldAlignment) -Strength 'Hard' -Impact 'Save' -Notes 'Serialization register marked this save surface for manual review.' -EvidenceSource 'SerializationRegister'
    }
}

$edgeRows = @($edges.ToArray() | Sort-Object SourceSystem, TargetSystem, EdgeType, Evidence)
$hardRows = @($edgeRows | Where-Object { $_.Strength -eq 'Hard' })
$softRows = @($edgeRows | Where-Object { $_.Strength -ne 'Hard' })
$conflictRows = @($edgeRows | Where-Object { $_.EdgeType -match 'Conflict|Duplicate|Override' -or $_.Notes -match 'conflict|mismatch|manual review|independent behavior' })

$standaloneRows = New-Object System.Collections.Generic.List[object]

foreach ($system in $systemCards)
{
    $name = $system.System
    $outgoing = @($edgeRows | Where-Object { $_.SourceSystem -eq $name }).Count
    $incoming = @($edgeRows | Where-Object { $_.TargetSystem -eq $name }).Count
    $runtimeRows = @($runtimeHookMap | Where-Object { $_.System -eq $name })
    $globalHookCount = @($runtimeRows | Where-Object { $_.HookType -in @('Event', 'Packet', 'Login', 'Logout', 'WorldSave', 'WorldLoad', 'Speech', 'Movement', 'Region') }).Count
    $packetCount = @($runtimeRows | Where-Object { $_.HookType -eq 'Packet' }).Count
    $serializedCount = @($serializationRegister | Where-Object { $_.CardSystems -match [regex]::Escape($name) -or $_.System -eq $name }).Count
    $configCount = @($configReferences | Where-Object {
        $systems = @(Get-OwnerSystemsForFile -File $_.File -OwnerByFile $ownerByFile -RuntimeByFile $runtimeByFile)
        $systems -contains $name
    }).Count
    $directReferenceEdges = @($edgeRows | Where-Object { ($_.SourceSystem -eq $name -or $_.TargetSystem -eq $name) -and $_.EdgeType -eq 'DirectReference' }).Count
    $docsOnlyEdges = @($edgeRows | Where-Object { ($_.SourceSystem -eq $name -or $_.TargetSystem -eq $name) -and $_.EdgeType -match '^Docs' }).Count
    $standaloneStatus = if ($outgoing -eq 0 -and $incoming -eq 0 -and $globalHookCount -eq 0 -and $serializedCount -eq 0 -and $configCount -eq 0) { 'StandaloneCandidateNeedsManualProof' } else { 'NotStandalone' }

    $standaloneRows.Add([pscustomobject]@{
        System = $name
        Classification = $system.Classification
        StandaloneStatus = $standaloneStatus
        OutgoingEdgeCount = $outgoing
        IncomingEdgeCount = $incoming
        DirectReferenceEdgeCount = $directReferenceEdges
        RuntimeHookCount = $runtimeRows.Count
        GlobalHookCount = $globalHookCount
        PacketHandlerCount = $packetCount
        SerializedRowCount = $serializedCount
        ConfigReferenceCount = $configCount
        DocsOnlyEdgeCount = $docsOnlyEdges
        NegativeEvidence = if ($standaloneStatus -eq 'StandaloneCandidateNeedsManualProof') { 'No generated edges, hooks, serialized rows, or config refs found; requires manual source review before standalone claim.' } else { 'Generated evidence found; standalone claim not supported.' }
    }) | Out-Null
}

$standaloneRowsArray = @($standaloneRows.ToArray() | Sort-Object System)

$graphPath = Export-AuditCsv -Rows $edgeRows -FileName 'dependency-graph.csv'
$phaseGraphPath = Export-AuditCsv -Rows $edgeRows -FileName 'phase-08-dependency-graph.csv'
$hardPath = Export-AuditCsv -Rows $hardRows -FileName 'phase-08-hard-dependency-list.csv'
$softPath = Export-AuditCsv -Rows $softRows -FileName 'phase-08-soft-dependency-list.csv'
$conflictPath = Export-AuditCsv -Rows $conflictRows -FileName 'phase-08-conflict-edge-list.csv'
$standalonePath = Export-AuditCsv -Rows $standaloneRowsArray -FileName 'phase-08-standalone-proof-list.csv'

$edgeTypeCounts = @($edgeRows | Group-Object EdgeType | Sort-Object Name)
$strengthCounts = @($edgeRows | Group-Object Strength | Sort-Object Name)
$systemsWithoutEdges = @($standaloneRowsArray | Where-Object { $_.OutgoingEdgeCount -eq 0 -and $_.IncomingEdgeCount -eq 0 })
$systemsWithoutEdgeNames = @($systemsWithoutEdges | ForEach-Object { $_.System })
$systemsWithoutEdgeText = if ($systemsWithoutEdgeNames.Count -eq 0) { 'None' } else { $systemsWithoutEdgeNames -join ';' }

$summaryPath = Join-Path $OutputDir 'phase-08-summary.md'
$summaryLines = New-Object System.Collections.Generic.List[string]
$summaryLines.Add('# Phase 8 Dependency Graph Summary') | Out-Null
$summaryLines.Add('') | Out-Null
$summaryLines.Add("Generated: $(Get-Date -Format o)") | Out-Null
$summaryLines.Add('') | Out-Null
$summaryLines.Add('## Required Inputs') | Out-Null
$summaryLines.Add('') | Out-Null
$summaryLines.Add('| Input | Status |') | Out-Null
$summaryLines.Add('| --- | --- |') | Out-Null
$summaryLines.Add("| System Cards | Present: ``phase-04-system-card-index.csv`` with $($systemCards.Count) rows |") | Out-Null
$summaryLines.Add("| CrossTreeRuntimeInventory | Present: ``cross-tree-runtime-inventory.csv`` with $($runtimeInventory.Count) rows |") | Out-Null
$summaryLines.Add("| Runtime Hook Map | Present: ``runtime-hook-map.csv`` with $($runtimeHookMap.Count) rows |") | Out-Null
$summaryLines.Add("| Serialization Register | Present: ``serialization-register.csv`` with $($serializationRegister.Count) rows |") | Out-Null
$summaryLines.Add("| Documentation Truth Table | Present: ``documentation-truth-table.csv`` with $($documentationTruth.Count) rows |") | Out-Null
$summaryLines.Add("| Project Truth Register | Present: ``project-truth-register.csv`` with $($projectTruth.Count) rows |") | Out-Null
$summaryLines.Add('') | Out-Null
$summaryLines.Add('## Generated Outputs') | Out-Null
$summaryLines.Add('') | Out-Null
$summaryLines.Add('| Output | Rows | Purpose |') | Out-Null
$summaryLines.Add('| --- | ---: | --- |') | Out-Null
$summaryLines.Add("| ``dependency-graph.csv`` | $($edgeRows.Count) | Canonical dependency graph. |") | Out-Null
$summaryLines.Add("| ``phase-08-dependency-graph.csv`` | $($edgeRows.Count) | Phase-scoped dependency graph. |") | Out-Null
$summaryLines.Add("| ``phase-08-hard-dependency-list.csv`` | $($hardRows.Count) | Hard source, runtime, serialization, project, and config dependencies. |") | Out-Null
$summaryLines.Add("| ``phase-08-soft-dependency-list.csv`` | $($softRows.Count) | Soft or speculative documentation/config relationships. |") | Out-Null
$summaryLines.Add("| ``phase-08-conflict-edge-list.csv`` | $($conflictRows.Count) | Conflict, mismatch, duplicate, or manual-review edges. |") | Out-Null
$summaryLines.Add("| ``phase-08-standalone-proof-list.csv`` | $($standaloneRowsArray.Count) | Negative-evidence standalone proof table for system-card systems. |") | Out-Null
$summaryLines.Add('') | Out-Null
$summaryLines.Add('## Edge Type Counts') | Out-Null
$summaryLines.Add('') | Out-Null
$summaryLines.Add('| Edge Type | Count |') | Out-Null
$summaryLines.Add('| --- | ---: |') | Out-Null
foreach ($group in $edgeTypeCounts)
{
    $summaryLines.Add("| $(Escape-MarkdownCell $group.Name) | $($group.Count) |") | Out-Null
}
$summaryLines.Add('') | Out-Null
$summaryLines.Add('## Strength Counts') | Out-Null
$summaryLines.Add('') | Out-Null
$summaryLines.Add('| Strength | Count |') | Out-Null
$summaryLines.Add('| --- | ---: |') | Out-Null
foreach ($group in $strengthCounts)
{
    $summaryLines.Add("| $(Escape-MarkdownCell $group.Name) | $($group.Count) |") | Out-Null
}
$summaryLines.Add('') | Out-Null
$summaryLines.Add('## Exit Criteria') | Out-Null
$summaryLines.Add('') | Out-Null
$summaryLines.Add("- Generated dependency edges cover direct source references, runtime hooks, serialization, project includes, XML/config references, and docs-only relationships.") | Out-Null
$summaryLines.Add("- Standalone proof rows record negative evidence counts; generated evidence marks systems as ``NotStandalone`` rather than assuming independence.") | Out-Null
$summaryLines.Add("- Systems without generated incoming or outgoing edges: $systemsWithoutEdgeText.") | Out-Null
$summaryLines.Add("- Docs-only and speculative edges are separated from hard source/runtime/save/project edges.") | Out-Null

Set-Content -LiteralPath $summaryPath -Value $summaryLines -Encoding UTF8

[pscustomobject]@{
    EdgeRows = $edgeRows.Count
    HardRows = $hardRows.Count
    SoftRows = $softRows.Count
    ConflictRows = $conflictRows.Count
    StandaloneRows = $standaloneRowsArray.Count
    SystemsWithoutEdges = @($systemsWithoutEdges).Count
    DependencyGraph = Convert-ToRepoPath $graphPath
    PhaseGraph = Convert-ToRepoPath $phaseGraphPath
    HardList = Convert-ToRepoPath $hardPath
    SoftList = Convert-ToRepoPath $softPath
    ConflictList = Convert-ToRepoPath $conflictPath
    StandaloneProof = Convert-ToRepoPath $standalonePath
    Summary = Convert-ToRepoPath $summaryPath
}
