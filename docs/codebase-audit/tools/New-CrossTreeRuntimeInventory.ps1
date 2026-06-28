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

function Get-RootBucket
{
    param([string]$Path)

    switch -Regex ($Path)
    {
        '^Data/Scripts/Custom/' { return 'Custom' }
        '^Data/Scripts/Items/' { return 'Items' }
        '^Data/Scripts/Magic/' { return 'Magic' }
        '^Data/Scripts/Mobiles/' { return 'Mobiles' }
        '^Data/Scripts/Quests/' { return 'Quests' }
        '^Data/Scripts/System/' { return 'System' }
        '^Data/Scripts/Trades/' { return 'Trades' }
        '^Data/System/Source/' { return 'ServerCore' }
        default { return 'Unknown' }
    }
}

function Get-SystemOwner
{
    param(
        [string]$Path,
        [string]$LikelySystem,
        [string]$Namespace,
        [string]$Types
    )

    if (-not [string]::IsNullOrWhiteSpace($LikelySystem) -and $LikelySystem -ne 'Unknown')
    {
        return $LikelySystem
    }

    if ($Namespace -match '^Server\.Items')
    {
        return 'Items:Namespace'
    }

    if ($Namespace -match '^Server\.Mobiles')
    {
        return 'Mobiles:Namespace'
    }

    if ($Namespace -match '^Server\.Gumps')
    {
        return 'System:Gumps'
    }

    if ($Types -match 'PlayerMobile')
    {
        return 'PlayerMobileCore'
    }

    if ($Path -match '^Data/System/Source/')
    {
        return 'ServerCore'
    }

    return 'Unknown'
}

function Add-Role
{
    param(
        [System.Collections.Generic.List[string]]$Roles,
        [string]$Role
    )

    if (-not $Roles.Contains($Role))
    {
        $Roles.Add($Role) | Out-Null
    }
}

function Get-RoleData
{
    param(
        [string]$Path,
        [string]$Root,
        [string[]]$Markers,
        [bool]$HasSerialization,
        [string]$SystemOwner,
        [string]$Types
    )

    $roles = [System.Collections.Generic.List[string]]::new()
    $evidence = [System.Collections.Generic.List[string]]::new()

    if ($Markers -contains 'PacketHandlers.Register')
    {
        Add-Role $roles 'PacketNetwork'
        $evidence.Add('PacketHandlers.Register marker') | Out-Null
    }

    if ($Markers -contains 'CommandSystem.Register')
    {
        Add-Role $roles 'CommandSurface'
        $evidence.Add('CommandSystem.Register marker') | Out-Null
    }

    if ($Markers -contains 'Initialize' -or $Markers -contains 'EventSink' -or $Markers -contains 'Timer.DelayCall' -or $Markers -contains 'CustomTimerSubclass' -or $Markers -contains 'WorldSave' -or $Markers -contains 'WorldLoad')
    {
        Add-Role $roles 'StartupWiring'
        $evidence.Add('startup/event/timer/world marker') | Out-Null
    }

    if ($Markers -contains 'RegionOverride')
    {
        Add-Role $roles 'RegionPolicy'
        $evidence.Add('region marker') | Out-Null
    }

    if ($Markers -contains 'OnResponse' -or $Markers -contains 'SendGump')
    {
        Add-Role $roles 'GumpUI'
        $evidence.Add('gump marker') | Out-Null
    }

    if ($HasSerialization)
    {
        Add-Role $roles 'Persistence'
        $evidence.Add('serialization marker') | Out-Null
    }

    if ($Path -match '(?i)CharacterLevel|LevelSystem|Experience|SkillTraining|Offline Skill')
    {
        Add-Role $roles 'PlayerProgression'
        $evidence.Add('progression path/type marker') | Out-Null
    }

    if ($Path -match '(?i)PvP|PK|Notoriety|Combat|Harmful|Beneficial|Spell|Magic')
    {
        Add-Role $roles 'CombatPolicy'
        $evidence.Add('combat/magic policy path marker') | Out-Null
    }

    if ($Root -eq 'Trades' -or $Path -match '(?i)Craft|Harvest|Gardening|Bulk Orders|Cooking|Brewing|Winecraft')
    {
        Add-Role $roles 'Crafting'
        $evidence.Add('crafting/trades path marker') | Out-Null
    }

    if ($Path -match '(?i)Vendor|Bank|Tax|Treasury|Economy|Government|Resource')
    {
        Add-Role $roles 'Economy'
        $evidence.Add('economy/government path marker') | Out-Null
    }

    if ($Path -match '(?i)XMLSpawner|Staff|Admin|GM|Seer|StaticGump|Invasion|Event')
    {
        Add-Role $roles 'StaffTooling'
        $evidence.Add('staff/event tooling path marker') | Out-Null
    }

    if ($Root -eq 'Mobiles')
    {
        Add-Role $roles 'MobileContent'
        $evidence.Add('Mobiles root') | Out-Null
    }

    if ($Root -eq 'Items')
    {
        Add-Role $roles 'ItemContent'
        $evidence.Add('Items root') | Out-Null
    }

    if ($Root -eq 'Quests' -or $Path -match '(?i)Quest|Region|World|Controller|Spawner|Encounter|Champion|Nest|Housing|Boat')
    {
        Add-Role $roles 'WorldState'
        $evidence.Add('world/quest/controller path marker') | Out-Null
    }

    if ($Path -match '(?i)Obsolete|Legacy')
    {
        Add-Role $roles 'LegacyCompatibility'
        $evidence.Add('legacy/obsolete path marker') | Out-Null
    }

    if ($Root -eq 'ServerCore')
    {
        Add-Role $roles 'StartupWiring'
        Add-Role $roles 'WorldState'
        $evidence.Add('server core root') | Out-Null
    }

    if ($roles.Count -eq 0)
    {
        Add-Role $roles 'Unknown'
        $evidence.Add('no Phase 1 marker or path classifier matched') | Out-Null
    }

    $priority = @(
        'PacketNetwork',
        'CommandSurface',
        'StartupWiring',
        'RegionPolicy',
        'GumpUI',
        'Persistence',
        'PlayerProgression',
        'CombatPolicy',
        'Crafting',
        'Economy',
        'StaffTooling',
        'MobileContent',
        'ItemContent',
        'WorldState',
        'LegacyCompatibility',
        'Unknown'
    )

    $primary = 'Unknown'

    foreach ($role in $priority)
    {
        if ($roles.Contains($role))
        {
            $primary = $role
            break
        }
    }

    $secondary = @($roles | Where-Object { $_ -ne $primary })

    return [pscustomobject]@{
        PrimaryRole = $primary
        SecondaryRoles = ($secondary -join ';')
        Evidence = (($evidence | Sort-Object -Unique) -join '; ')
    }
}

function Join-Values
{
    param([object[]]$Values)

    return (($Values | Where-Object { -not [string]::IsNullOrWhiteSpace([string]$_) } | Sort-Object -Unique) -join ';')
}

New-Item -ItemType Directory -Force -Path $OutputDir | Out-Null

$sourceRows = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'phase-01-source-files.csv'))
$namespaceRows = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'phase-01-namespace-type-inventory.csv'))
$runtimeRows = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'phase-01-runtime-marker-inventory.csv'))
$commandRows = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'phase-01-command-registration-inventory.csv'))
$serializationRows = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'phase-01-serialization-marker-inventory.csv'))
$gumpRows = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'phase-01-gump-inventory.csv'))
$configReferenceRows = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'phase-01-config-reference-inventory.csv'))
$projectTruthRows = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'project-truth-register.csv') | Where-Object { $_.RecordType -eq 'SourceFile' })

$namespaceByFile = @{}
$typesByFile = @{}
$markersByFile = @{}
$commandsByFile = @{}
$serializationByFile = @{}
$gumpsByFile = @{}
$configByFile = @{}
$projectByFile = @{}

foreach ($row in $namespaceRows)
{
    $namespaceByFile[$row.Path] = $row.Namespace
    $typesByFile[$row.Path] = $row.Types
}

foreach ($row in $runtimeRows)
{
    if (-not $markersByFile.ContainsKey($row.File))
    {
        $markersByFile[$row.File] = New-Object System.Collections.Generic.List[string]
    }

    $markersByFile[$row.File].Add($row.Marker) | Out-Null
}

foreach ($row in $commandRows)
{
    if (-not $commandsByFile.ContainsKey($row.File))
    {
        $commandsByFile[$row.File] = New-Object System.Collections.Generic.List[string]
    }

    $commandText = if ([string]::IsNullOrWhiteSpace($row.Command)) { $row.RegistrationLine } else { "$($row.Command):$($row.AccessLevel)" }
    $commandsByFile[$row.File].Add($commandText) | Out-Null
}

foreach ($row in $serializationRows)
{
    $serializationByFile[$row.File] = $row
}

foreach ($row in $gumpRows)
{
    if (-not $gumpsByFile.ContainsKey($row.File))
    {
        $gumpsByFile[$row.File] = New-Object System.Collections.Generic.List[string]
    }

    $gumpsByFile[$row.File].Add($row.GumpMarker) | Out-Null
}

foreach ($row in $configReferenceRows)
{
    if (-not $configByFile.ContainsKey($row.File))
    {
        $configByFile[$row.File] = New-Object System.Collections.Generic.List[string]
    }

    $configByFile[$row.File].Add($row.Reference) | Out-Null
}

foreach ($row in $projectTruthRows)
{
    $projectByFile[$row.Path] = $row
}

$inventoryRows = New-Object System.Collections.Generic.List[object]

foreach ($source in $sourceRows)
{
    $path = $source.Path
    $root = Get-RootBucket $path
    $markers = if ($markersByFile.ContainsKey($path)) { @($markersByFile[$path] | Sort-Object -Unique) } else { @() }
    $namespace = if ($namespaceByFile.ContainsKey($path)) { $namespaceByFile[$path] } else { '' }
    $types = if ($typesByFile.ContainsKey($path)) { $typesByFile[$path] } else { '' }
    $systemOwner = Get-SystemOwner -Path $path -LikelySystem $source.LikelySystem -Namespace $namespace -Types $types
    $hasSerialization = $serializationByFile.ContainsKey($path)
    $roleData = Get-RoleData -Path $path -Root $root -Markers $markers -HasSerialization $hasSerialization -SystemOwner $systemOwner -Types $types
    $projectInfo = if ($projectByFile.ContainsKey($path)) { $projectByFile[$path] } else { $null }

    $inventoryRows.Add([pscustomobject]@{
        Path = $path
        Root = $root
        PrimaryRole = $roleData.PrimaryRole
        SecondaryRoles = $roleData.SecondaryRoles
        System = $systemOwner
        Namespace = $namespace
        Types = $types
        IncludedInScriptsProject = if ($projectInfo -ne $null) { $projectInfo.IncludedInScriptsProject } else { '' }
        ProjectTruthAction = if ($projectInfo -ne $null) { $projectInfo.Action } else { '' }
        Commands = if ($commandsByFile.ContainsKey($path)) { Join-Values $commandsByFile[$path] } else { '' }
        Hooks = Join-Values (@($markers | Where-Object { $_ -notin @('CommandSystem.Register', 'OnResponse', 'SendGump') }))
        SerializedTypes = if ($hasSerialization) { $types } else { '' }
        Gumps = if ($gumpsByFile.ContainsKey($path)) { Join-Values $gumpsByFile[$path] } else { '' }
        ConfigData = if ($configByFile.ContainsKey($path)) { Join-Values $configByFile[$path] } else { '' }
        Evidence = $roleData.Evidence
    }) | Out-Null
}

$rootSummaryRows = foreach ($group in ($inventoryRows | Group-Object -Property Root | Sort-Object Name))
{
    $files = @($group.Group)

    [pscustomobject]@{
        Root = $group.Name
        FileCount = $files.Count
        InitializationFileCount = @($files | Where-Object { $_.Hooks -match 'Initialize' }).Count
        CommandFileCount = @($files | Where-Object { -not [string]::IsNullOrWhiteSpace($_.Commands) }).Count
        EventHookFileCount = @($files | Where-Object { $_.Hooks -match 'EventSink' }).Count
        PacketHookFileCount = @($files | Where-Object { $_.Hooks -match 'PacketHandlers\.Register' }).Count
        SerializationFileCount = @($files | Where-Object { -not [string]::IsNullOrWhiteSpace($_.SerializedTypes) }).Count
        GumpReferenceFileCount = @($files | Where-Object { -not [string]::IsNullOrWhiteSpace($_.Gumps) }).Count
        UnknownRoleCount = @($files | Where-Object { $_.PrimaryRole -eq 'Unknown' }).Count
        UnknownOwnerCount = @($files | Where-Object { $_.System -eq 'Unknown' }).Count
    }
}

$unknownRows = @($inventoryRows | Where-Object { $_.PrimaryRole -eq 'Unknown' -or $_.System -eq 'Unknown' } | ForEach-Object {
    [pscustomobject]@{
        Path = $_.Path
        Root = $_.Root
        PrimaryRole = $_.PrimaryRole
        System = $_.System
        Evidence = $_.Evidence
        FollowUp = 'Review folder, namespace, type names, docs, and references to assign a durable owner and role.'
    }
})

$highRiskRows = @(
    [pscustomobject]@{ Root = 'Custom'; MainRole = 'Mixed custom gameplay, imported packages, and staff tools'; MainRisk = 'Package boundaries, global hooks, project include drift, and persistence-heavy custom systems'; FirstFollowUp = 'Prioritize XMLSpawner, Government, PvP Consent, Homestead, AI, and Random Encounters system cards.' },
    [pscustomobject]@{ Root = 'Items'; MainRole = 'Item content and persistent item save surfaces'; MainRisk = 'Large serialized save surface and moved gump project drift'; FirstFollowUp = 'Use serialization register before item moves; review missing gump compile targets.' },
    [pscustomobject]@{ Root = 'Magic'; MainRole = 'Spell framework and magic school content'; MainRisk = 'Spell registry coupling and high-ID spell family registration'; FirstFollowUp = 'Map spell framework dependencies and registry limits before edits.' },
    [pscustomobject]@{ Root = 'Mobiles'; MainRole = 'Mobile content and AI behavior'; MainRisk = 'Serialized mobiles, AI assumptions, and save compatibility'; FirstFollowUp = 'Prioritize high-risk serialized mobiles in Phase 6.' },
    [pscustomobject]@{ Root = 'Quests'; MainRole = 'Quest systems, gumps, and rewards'; MainRisk = 'Quest state, gump responses, and reward side effects'; FirstFollowUp = 'Trace quest entry points and serialized quest state.' },
    [pscustomobject]@{ Root = 'System'; MainRole = 'Runtime wiring, commands, framework, help, skills, and policy'; MainRisk = 'Global hooks, command access, packet handlers, and project drift'; FirstFollowUp = 'Use runtime hook map to review command/event/packet surfaces.' },
    [pscustomobject]@{ Root = 'Trades'; MainRole = 'Crafting, harvest, economy, bulk orders, and gardening'; MainRisk = 'Economy loops, pooled enumerable usage, and moved gump project drift'; FirstFollowUp = 'Review crafting roots and bulk order/gardening gump drift.' },
    [pscustomobject]@{ Root = 'ServerCore'; MainRole = 'Core server framework'; MainRisk = 'Shared runtime framework and build dependency for scripts'; FirstFollowUp = 'Keep server framework separate from script reorganization unless specifically required.' }
)

Export-AuditCsv -Rows $inventoryRows -FileName 'phase-03-cross-tree-runtime-inventory.csv' | Out-Null
Export-AuditCsv -Rows $inventoryRows -FileName 'cross-tree-runtime-inventory.csv' | Out-Null
Export-AuditCsv -Rows $rootSummaryRows -FileName 'phase-03-root-role-summary.csv' | Out-Null
Export-AuditCsv -Rows $unknownRows -FileName 'phase-03-unknown-owner-list.csv' | Out-Null
Export-AuditCsv -Rows $highRiskRows -FileName 'phase-03-high-risk-root-summary.csv' | Out-Null

$highRiskPath = Join-Path $OutputDir 'phase-03-high-risk-root-summary.md'
$highRiskLines = @(
    '# Phase 3 High-Risk Root Summary',
    '',
    "Generated: $(Get-Date -Format o)",
    '',
    '| Root | Main Role | Main Risk | First Follow-Up |',
    '| --- | --- | --- | --- |'
)

foreach ($row in $highRiskRows)
{
    $highRiskLines += "| $($row.Root) | $($row.MainRole) | $($row.MainRisk) | $($row.FirstFollowUp) |"
}

Set-Content -LiteralPath $highRiskPath -Value $highRiskLines -Encoding UTF8

$summaryPath = Join-Path $OutputDir 'phase-03-summary.md'
$summaryLines = @(
    '# Phase 3 Cross-Tree Runtime Inventory Summary',
    '',
    "Generated: $(Get-Date -Format o)",
    '',
    '## Required Inputs',
    '',
    '| Input | Status |',
    '| --- | --- |',
    '| Phase 1 file inventory | Present: `phase-01-source-files.csv` |',
    '| Phase 2 project truth register | Present: `project-truth-register.csv` |',
    '| Runtime marker scans | Present: `phase-01-runtime-marker-inventory.csv` |',
    '| Serialization marker scans | Present: `phase-01-serialization-marker-inventory.csv` |',
    '',
    '## Generated Outputs',
    '',
    '| Output | Rows | Purpose |',
    '| --- | ---: | --- |',
    "| `cross-tree-runtime-inventory.csv` | $($inventoryRows.Count) | Canonical runtime role and owner inventory. |",
    "| `phase-03-cross-tree-runtime-inventory.csv` | $($inventoryRows.Count) | Phase-scoped copy of the runtime inventory. |",
    "| `phase-03-root-role-summary.csv` | $($rootSummaryRows.Count) | Root-level marker and role counts. |",
    "| `phase-03-unknown-owner-list.csv` | $($unknownRows.Count) | Files with `Unknown` owner or role requiring follow-up. |",
    "| `phase-03-high-risk-root-summary.csv` | $($highRiskRows.Count) | Machine-readable high-risk root summary. |",
    "| `phase-03-high-risk-root-summary.md` | $($highRiskRows.Count) | Human-readable high-risk root summary. |",
    '',
    '## Root Summary',
    '',
    '| Root | Files | Init | Commands | Events | Packets | Serialization | Gumps | Unknown Roles | Unknown Owners |',
    '| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |'
)

foreach ($row in $rootSummaryRows)
{
    $summaryLines += "| $($row.Root) | $($row.FileCount) | $($row.InitializationFileCount) | $($row.CommandFileCount) | $($row.EventHookFileCount) | $($row.PacketHookFileCount) | $($row.SerializationFileCount) | $($row.GumpReferenceFileCount) | $($row.UnknownRoleCount) | $($row.UnknownOwnerCount) |"
}

$summaryLines += @(
    '',
    '## Exit Criteria',
    '',
    '- Every audited `.cs` file has a provisional primary role.',
    '- Every file has a provisional system owner or an explicit `Unknown` row with follow-up.',
    '- Each root has marker counts for initialization, commands, events, packets, serialization, and gumps.',
    '- High-risk root follow-up notes are recorded for later system cards and risk tracks.'
)

Set-Content -LiteralPath $summaryPath -Value $summaryLines -Encoding UTF8

[pscustomobject]@{
    OutputDir = $OutputDir
    RuntimeInventoryRows = $inventoryRows.Count
    RootSummaryRows = $rootSummaryRows.Count
    UnknownRows = $unknownRows.Count
    HighRiskRootRows = $highRiskRows.Count
}
