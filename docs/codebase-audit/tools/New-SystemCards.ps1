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
$SystemCardsDir = Join-Path $OutputDir 'system-cards'

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

function Normalize-Slug
{
    param([string]$Name)

    return (($Name.ToLowerInvariant() -replace '[^a-z0-9]+', '-') -replace '(^-|-$)', '')
}

function Join-Unique
{
    param([object[]]$Values)

    return (($Values | Where-Object { -not [string]::IsNullOrWhiteSpace([string]$_) } | Sort-Object -Unique) -join ';')
}

New-Item -ItemType Directory -Force -Path $SystemCardsDir | Out-Null

$runtimeInventory = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'cross-tree-runtime-inventory.csv'))
$projectTruth = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'project-truth-register.csv'))
$commands = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'phase-01-command-registration-inventory.csv'))
$docs = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'phase-01-documentation-inventory.csv'))
$configRefs = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'phase-01-config-reference-inventory.csv'))

$systemDefinitions = @(
    [pscustomobject]@{ Name = 'Character Level'; Classification = 'GameplayLayer'; Patterns = @('Data/Scripts/Custom/Progression/CharacterLevel/', 'CharacterLevel'); Docs = @('Character_Level'); Summary = 'Player progression layer and level-aware behavior. Marker evidence is provisional until dependency and serialization phases deepen the review.'; Dependencies = 'PlayerMobile; Random Encounters; combat display; documentation source traces' },
    [pscustomobject]@{ Name = 'Random Encounters'; Classification = 'GameplayLayer'; Patterns = @('Data/Scripts/Custom/RandomEncounters/', 'RandomEncounter'); Docs = @('Random_Encounter'); Summary = 'Automated PvE encounter layer driven by source and data markers.'; Dependencies = 'Character Level; XML/config encounter tables; timers; cleanup attachments' },
    [pscustomobject]@{ Name = 'PvP Consent'; Classification = 'GameplayLayer'; Patterns = @('Data/Scripts/Custom/PvPConsent/', 'PvPConsent', 'PKNONPK', 'PKGump', 'NONPK'); Docs = @('PvP_Consent', 'pvp-consent'); Summary = 'Combat consent and event-gate policy surface with player and staff-facing implications.'; Dependencies = 'PlayerMobile; Notoriety; Regions; event gates; harmful and beneficial action checks' },
    [pscustomobject]@{ Name = 'Government'; Classification = 'GameplayLayer'; Patterns = @('Data/Scripts/Custom/Government System/', 'Government', 'PlayerCity', 'City'); Docs = @('Government_System', 'Player_Government'); Summary = 'Persistent civic gameplay system for cities, elections, taxes, treasuries, bans, wars, vendors, and civic structures.'; Dependencies = 'PlayerMobile; Regions; Housing; Vendors; Banking; city gumps' },
    [pscustomobject]@{ Name = 'Homestead'; Classification = 'ImportedPackage'; Patterns = @("Data/Scripts/Custom/Vhaerun's CRL Homestead System", 'Homestead', 'Vhaerun', 'Winecraft', 'Cooking', 'Crops', 'Hunger'); Docs = @('Homestead'); Summary = 'Large imported homestead package with farming, cooking, crops, hunger, and production subpackages.'; Dependencies = 'Trades; Crafting; resource loops; package-specific gumps and serializers' },
    [pscustomobject]@{ Name = 'XMLSpawner'; Classification = 'SharedService'; Patterns = @('Data/Scripts/Custom/XMLSpawner/', 'XmlSpawner', 'XmlAttach', 'XmlQuest', 'XmlPoints'); Docs = @('XMLSpawner'); Summary = 'Shared staff/event infrastructure for spawners, attachments, quests, points, packet hooks, speech, movement, and persistence.'; Dependencies = 'Attachments; quests; packet handlers; speech/movement hooks; staff commands' },
    [pscustomobject]@{ Name = 'Invasion'; Classification = 'StaffEventTool'; Patterns = @('Data/Scripts/Custom/Invasion System/', 'Invasion'); Docs = @('Invasion_System'); Summary = 'Staff/event PvE pressure system with gumps, spawns, rewards, and event lifecycle risks.'; Dependencies = 'Mobiles; items; staff commands; world timers' },
    [pscustomobject]@{ Name = 'Champions'; Classification = 'GameplayLayer'; Patterns = @('Data/Scripts/Custom/Champions/', 'ChampionSpawn', 'CannedEvil'); Docs = @('Champion'); Summary = 'Champion spawn PvE escalation and reward system.'; Dependencies = 'Mobiles; rewards; regions; spawn timers' },
    [pscustomobject]@{ Name = 'Monster Nests'; Classification = 'GameplayLayer'; Patterns = @('Data/Scripts/Custom/MonsterNest/', 'MonsterNest'); Docs = @('Monster_Nest'); Summary = 'Monster nest PvE system with world-state and reward pressure.'; Dependencies = 'Mobiles; items; regions; player progression' },
    [pscustomobject]@{ Name = 'Clone Offline Player Characters'; Classification = 'GameplayLayer'; Patterns = @('Data/Scripts/Custom/CloneOfflinePlayerCharacters/', 'OfflinePlayer', 'Clone'); Docs = @('Clone_Offline'); Summary = 'Offline character clone system with mobile/item clone behavior and command surfaces.'; Dependencies = 'PlayerMobile; items; mobiles; timers' },
    [pscustomobject]@{ Name = 'Offline Skill Training'; Classification = 'GameplayLayer'; Patterns = @('Data/Scripts/Custom/Offline Skill Training/', 'OfflineSkill'); Docs = @('offline-skill-training'); Summary = 'Offline progression system for skill training.'; Dependencies = 'PlayerMobile; skill system; persistence' },
    [pscustomobject]@{ Name = 'OmniAI'; Classification = 'ImportedPackage'; Patterns = @('Data/Scripts/Custom/ThirdParty/OmniAI/', 'OmniAI'); Docs = @('OmniAI'); Summary = 'Imported AI package with multiple ability modules.'; Dependencies = 'Mobiles; combat; magic abilities' },
    [pscustomobject]@{ Name = 'AI Overhaul'; Classification = 'GameplayLayer'; Patterns = @('Data/Scripts/Custom/Combat/AIOverhaul/', 'AIOverhaul', 'AITactical'); Docs = @('AI_OVERHAUL', 'Creature_AI'); Summary = 'Custom AI behavior and targeting overhaul.'; Dependencies = 'Mobiles; combat; creature AI core' },
    [pscustomobject]@{ Name = 'Staff Toolbar'; Classification = 'StaffEventTool'; Patterns = @('Data/Scripts/Custom/Staff Toolbar', 'StaffToolbar'); Docs = @('Staff_Toolbar'); Summary = 'Staff-facing toolbar and administrative convenience surface.'; Dependencies = 'Access levels; staff commands; gumps' },
    [pscustomobject]@{ Name = 'Static Gump Tool'; Classification = 'StaffEventTool'; Patterns = @('Data/Scripts/Custom/StaffTools/StaticGumpTool/', 'StaticGump'); Docs = @('Static_Gump'); Summary = 'Staff-facing static gump creation and management tool.'; Dependencies = 'Gumps; staff commands; persistence where configured' },
    [pscustomobject]@{ Name = 'Spell Framework'; Classification = 'SharedService'; Patterns = @('Data/Scripts/Magic/', 'SpellRegistry', 'BaseSpell', 'Spellbook'); Docs = @('Base_Spell', 'Spell_System', 'Spell'); Summary = 'Shared spell and magic framework, including registry, spellbooks, and spell-family infrastructure.'; Dependencies = 'Magic schools; spellbooks; spellbars; combat; items' },
    [pscustomobject]@{ Name = 'Magic Schools'; Classification = 'GameplayLayer'; Patterns = @('Data/Scripts/Magic/Bard/', 'Data/Scripts/Magic/Death Knight/', 'Data/Scripts/Magic/Elementalism/', 'Data/Scripts/Magic/Holy Man/', 'Data/Scripts/Magic/Jedi/', 'Data/Scripts/Magic/Mystic/', 'Data/Scripts/Magic/Research/', 'Data/Scripts/Magic/Syth/'); Docs = @('Magery', 'Witchcraft', 'Misc_Magic'); Summary = 'Player-facing magic school content layered on the spell framework.'; Dependencies = 'Spell Framework; spellbooks; combat policy; gumps' },
    [pscustomobject]@{ Name = 'Crafting Core'; Classification = 'SharedService'; Patterns = @('Data/Scripts/Trades/Core/', 'CraftSystem', 'CraftItem', 'CraftGump'); Docs = @('Crafting_Core', 'Standard_Crafting'); Summary = 'Shared crafting framework and gump surface.'; Dependencies = 'Trades; resources; items; bulk orders' },
    [pscustomobject]@{ Name = 'Harvest System'; Classification = 'SharedService'; Patterns = @('Data/Scripts/Trades/Harvest/', 'HarvestSystem', 'Harvest'); Docs = @('Harvest_Resource'); Summary = 'Resource harvesting framework and economy input surface.'; Dependencies = 'Crafting; resources; map/region rules' },
    [pscustomobject]@{ Name = 'Bulk Orders'; Classification = 'GameplayLayer'; Patterns = @('Data/Scripts/Trades/Bulk Orders/', 'BulkOrder', 'BOD'); Docs = @('Bulk'); Summary = 'Bulk order crafting objective and reward system.'; Dependencies = 'Crafting Core; vendors; rewards; gumps' },
    [pscustomobject]@{ Name = 'Gardening'; Classification = 'GameplayLayer'; Patterns = @('Data/Scripts/Trades/Gardening/', 'Gardening', 'Plant'); Docs = @('Gardening'); Summary = 'Gardening production and plant lifecycle system.'; Dependencies = 'Items; crafting/economy loops; gumps; persistence' },
    [pscustomobject]@{ Name = 'Housing'; Classification = 'SharedService'; Patterns = @('Data/Scripts/Items/Houses/', 'BaseHouse', 'House'); Docs = @('Housing_System'); Summary = 'Housing framework, placement, ownership, remodeling, and house-related item surfaces.'; Dependencies = 'Regions; items; vendors; player ownership; gumps' },
    [pscustomobject]@{ Name = 'Boats'; Classification = 'GameplayLayer'; Patterns = @('Data/Scripts/Items/Boats/', 'Boat', 'Tiller'); Docs = @('Shipwright', 'Sailing', 'Boat'); Summary = 'Boat item and travel system.'; Dependencies = 'Items; maps; movement; shipwright docs and vendors' },
    [pscustomobject]@{ Name = 'Regions'; Classification = 'SharedService'; Patterns = @('Data/Scripts/System/Regions/', 'Region', 'GuardedRegion'); Docs = @('Regions'); Summary = 'Region policy framework affecting combat, travel, housing, and gameplay rules.'; Dependencies = 'Maps; Notoriety; PvP Consent; Government; housing' },
    [pscustomobject]@{ Name = 'PlayerMobile Core'; Classification = 'SharedService'; Patterns = @('PlayerMobile'); Docs = @('PlayerMobile'); Summary = 'Core player state surface coupled to many gameplay systems.'; Dependencies = 'PvP Consent; Government; Character Level; housing; account state; persistence' },
    [pscustomobject]@{ Name = 'Vendor Core'; Classification = 'SharedService'; Patterns = @('BaseVendor', 'SBInfo', 'Vendor'); Docs = @('Vendor_Core', 'Shoppes_Vendor'); Summary = 'Vendor framework, shop buy/sell info, and economy surface.'; Dependencies = 'Economy; crafting; Government; NPC mobiles; banking' },
    [pscustomobject]@{ Name = 'Obsolete Scripts'; Classification = 'LegacyCompatibility'; Patterns = @('Data/Scripts/System/Obsolete/', 'Obsolete'); Docs = @('Obsolete_Script'); Summary = 'Legacy or obsolete scripts that may still compile, register commands, or preserve compatibility.'; Dependencies = 'Save compatibility; old commands; legacy runtime hooks' }
)

$ownerRows = New-Object System.Collections.Generic.List[object]
$backlogRows = New-Object System.Collections.Generic.List[object]
$priorityRows = New-Object System.Collections.Generic.List[object]
$cards = New-Object System.Collections.Generic.List[object]
$cardIndex = 1

foreach ($definition in $systemDefinitions)
{
    $matchedFiles = @($runtimeInventory | Where-Object {
        $path = $_.Path
        $types = $_.Types
        $system = $_.System
        $matched = $false

        foreach ($pattern in $definition.Patterns)
        {
            if ($path -match [regex]::Escape($pattern) -or $types -match [regex]::Escape($pattern) -or $system -match [regex]::Escape($pattern))
            {
                $matched = $true
                break
            }
        }

        $matched
    })

    $matchedDocs = @($docs | Where-Object {
        $docPath = $_.DocPath
        $matched = $false

        foreach ($pattern in $definition.Docs)
        {
            if ($docPath -match [regex]::Escape($pattern))
            {
                $matched = $true
                break
            }
        }

        $matched
    })

    $matchedCommands = @($commands | Where-Object {
        $file = $_.File
        ($matchedFiles | Where-Object { $_.Path -eq $file } | Measure-Object).Count -gt 0
    })

    $matchedConfig = @($configRefs | Where-Object {
        $file = $_.File
        ($matchedFiles | Where-Object { $_.Path -eq $file } | Measure-Object).Count -gt 0
    })

    foreach ($file in $matchedFiles)
    {
        $ownerRows.Add([pscustomobject]@{
            System = $definition.Name
            Classification = $definition.Classification
            File = $file.Path
            PrimaryRole = $file.PrimaryRole
            SecondaryRoles = $file.SecondaryRoles
            RuntimeHooks = $file.Hooks
            SerializedState = $file.SerializedTypes
            Gumps = $file.Gumps
            ConfigData = $file.ConfigData
            VerificationStatus = 'PartiallyVerified'
        }) | Out-Null
    }

    $serializedFiles = @($matchedFiles | Where-Object { -not [string]::IsNullOrWhiteSpace($_.SerializedTypes) })
    $hookFiles = @($matchedFiles | Where-Object { -not [string]::IsNullOrWhiteSpace($_.Hooks) })
    $gumpFiles = @($matchedFiles | Where-Object { -not [string]::IsNullOrWhiteSpace($_.Gumps) })
    $verificationStatus = if ($matchedFiles.Count -eq 0) { 'Blocked' } elseif ($serializedFiles.Count -gt 0) { 'NeedsSerializationReview' } elseif ($hookFiles.Count -gt 0 -or $gumpFiles.Count -gt 0) { 'NeedsRuntimeReview' } else { 'PartiallyVerified' }
    $cardFileName = (Normalize-Slug $definition.Name) + '.md'
    $cardPath = Join-Path $SystemCardsDir $cardFileName

    $sourceLines = @('| File | Primary Role | Runtime Hooks | Serialized | Gumps |', '| --- | --- | --- | --- | --- |')
    $sourceLimit = 250
    $sourceShown = @($matchedFiles | Select-Object -First $sourceLimit)

    foreach ($file in $sourceShown)
    {
        $sourceLines += "| $(Escape-MarkdownCell $file.Path) | $(Escape-MarkdownCell $file.PrimaryRole) | $(Escape-MarkdownCell $file.Hooks) | $(if ([string]::IsNullOrWhiteSpace($file.SerializedTypes)) { 'No' } else { 'Yes' }) | $(Escape-MarkdownCell $file.Gumps) |"
    }

    if ($matchedFiles.Count -gt $sourceLimit)
    {
        $sourceLines += "| ... | ... | ... | ... | Full file list is in `phase-04-system-owner-map.csv`. |"
    }

    $playerCommands = @($matchedCommands | Where-Object { $_.AccessLevel -eq 'Player' -or [string]::IsNullOrWhiteSpace($_.AccessLevel) })
    $staffCommands = @($matchedCommands | Where-Object { $_.AccessLevel -ne 'Player' -and -not [string]::IsNullOrWhiteSpace($_.AccessLevel) })

    $playerLines = @('| Entry | Evidence |', '| --- | --- |')
    foreach ($command in $playerCommands)
    {
        $playerLines += "| Command `$(Escape-MarkdownCell $command.Command)` | $(Escape-MarkdownCell $command.File):$($command.Line) |"
    }
    if ($playerCommands.Count -eq 0)
    {
        $playerLines += '| None found in Phase 1 command markers | Review items, NPCs, speech, regions, and gumps in later phases. |'
    }

    $staffLines = @('| Entry | Evidence |', '| --- | --- |')
    foreach ($command in $staffCommands)
    {
        $staffLines += "| Command `$(Escape-MarkdownCell $command.Command)` (`$(Escape-MarkdownCell $command.AccessLevel)`) | $(Escape-MarkdownCell $command.File):$($command.Line) |"
    }
    if ($staffCommands.Count -eq 0)
    {
        $staffLines += '| None found in Phase 1 command markers | Review staff gumps and props in later phases. |'
    }

    $hookSummary = Join-Unique (@($matchedFiles | ForEach-Object { $_.Hooks }))
    $serializedSummary = Join-Unique (@($serializedFiles | ForEach-Object { $_.Path }))
    $configSummary = Join-Unique (@($matchedConfig | ForEach-Object { $_.Reference }))
    $docSummary = Join-Unique (@($matchedDocs | ForEach-Object { $_.DocPath }))
    $configText = if ([string]::IsNullOrWhiteSpace($configSummary)) { 'No XML/config/text/json references were found in Phase 1 string-reference markers.' } else { $configSummary }
    $hookText = if ([string]::IsNullOrWhiteSpace($hookSummary)) { 'No runtime hook markers were found in matched files.' } else { $hookSummary }
    $serializedText = if ([string]::IsNullOrWhiteSpace($serializedSummary)) { 'No serialization marker rows were found in matched files.' } else { "Serialized marker files: $($serializedFiles.Count). See `phase-01-serialization-marker-inventory.csv` and Phase 6 for write/read maps." }
    $docText = if ([string]::IsNullOrWhiteSpace($docSummary)) { 'No matching wiki page was found by Phase 4 doc-name pattern matching.' } else { $docSummary }

    $cardLines = @(
        "# System: $($definition.Name)",
        '',
        '## Classification',
        '',
        $definition.Classification,
        '',
        '## Summary',
        '',
        $definition.Summary,
        '',
        '## Source Files',
        '',
        "Matched source files: $($matchedFiles.Count).",
        ''
    ) + $sourceLines + @(
        '',
        '## Data Files',
        '',
        $configText,
        '',
        '## Player Entry Points',
        ''
    ) + $playerLines + @(
        '',
        '## Staff Entry Points',
        ''
    ) + $staffLines + @(
        '',
        '## Runtime Hooks',
        '',
        $hookText,
        '',
        '## Serialized State',
        '',
        $serializedText,
        '',
        '## Dependencies',
        '',
        $definition.Dependencies,
        '',
        '## Dependents',
        '',
        'Not fully verified in Phase 4. Dependency and dependent edges are deferred to Phase 8.',
        '',
        '## Synergies',
        '',
        'Deferred to Phase 9 synergy and conflict matrix.',
        '',
        '## Conflicts And Risks',
        '',
        "- Verification status is `$verificationStatus`; this card is generated from marker inventories and requires deeper Phase 5/6 review.",
        "- Project truth issues for matched files are tracked in `project-truth-register.csv` and `project-cleanup-backlog.csv`.",
        '',
        '## Documentation',
        '',
        $docText,
        '',
        '## Verification Status',
        '',
        $verificationStatus,
        '',
        '## Follow-Up Work',
        '',
        '- Review runtime hooks in Phase 5.',
        '- Review serialized state in Phase 6 when serialization markers are present.',
        '- Verify documentation source traces in Phase 7.',
        '- Convert findings into Phase 13 repair backlog items where needed.'
    )

    Set-Content -LiteralPath $cardPath -Value $cardLines -Encoding UTF8

    $cards.Add([pscustomobject]@{
        System = $definition.Name
        Classification = $definition.Classification
        CardPath = "docs/codebase-audit/outputs/system-cards/$cardFileName"
        SourceFileCount = $matchedFiles.Count
        CommandCount = $matchedCommands.Count
        HookFileCount = $hookFiles.Count
        SerializedFileCount = $serializedFiles.Count
        GumpFileCount = $gumpFiles.Count
        DocCount = $matchedDocs.Count
        VerificationStatus = $verificationStatus
    }) | Out-Null

    $backlogRows.Add([pscustomobject]@{
        Id = ('SC-{0:000}' -f $cardIndex)
        System = $definition.Name
        Status = if ($verificationStatus -eq 'Blocked') { 'Blocked' } else { 'Open' }
        VerificationStatus = $verificationStatus
        Reason = if ($matchedFiles.Count -eq 0) { 'No source files matched Phase 4 seed patterns.' } else { 'Generated card requires deeper runtime, serialization, dependency, and documentation review.' }
        NextAction = 'Use Phases 5 through 8 to replace marker-based partial verification with source-traced review.'
        CardPath = "docs/codebase-audit/outputs/system-cards/$cardFileName"
    }) | Out-Null

    $priorityRows.Add([pscustomobject]@{
        Priority = $cardIndex
        System = $definition.Name
        Classification = $definition.Classification
        SourceFileCount = $matchedFiles.Count
        RiskBasis = if ($definition.Classification -eq 'SharedService') { 'Shared service or framework dependency' } elseif ($definition.Classification -eq 'LegacyCompatibility') { 'Legacy compatibility and possible active hooks' } elseif ($serializedFiles.Count -gt 0) { 'Serialization markers present' } elseif ($hookFiles.Count -gt 0) { 'Runtime hook markers present' } else { 'Seed high-risk system' }
        FirstFollowUp = 'Review system card and feed Phase 5/6/8 outputs.'
    }) | Out-Null

    $cardIndex++
}

Export-AuditCsv -Rows $cards -FileName 'phase-04-system-card-index.csv' | Out-Null
Export-AuditCsv -Rows $ownerRows -FileName 'phase-04-system-owner-map.csv' | Out-Null
Export-AuditCsv -Rows $ownerRows -FileName 'system-owner-map.csv' | Out-Null
Export-AuditCsv -Rows $backlogRows -FileName 'phase-04-system-card-backlog.csv' | Out-Null
Export-AuditCsv -Rows $priorityRows -FileName 'phase-04-high-risk-system-priority-list.csv' | Out-Null

$summaryPath = Join-Path $OutputDir 'phase-04-summary.md'
$summaryLines = @(
    '# Phase 4 System Cards Summary',
    '',
    "Generated: $(Get-Date -Format o)",
    '',
    '## Required Inputs',
    '',
    '| Input | Status |',
    '| --- | --- |',
    '| CrossTreeRuntimeInventory | Present: `cross-tree-runtime-inventory.csv` |',
    '| Project Truth Register | Present: `project-truth-register.csv` |',
    '| Runtime Hook Map draft | Marker-derived draft from `phase-01-runtime-marker-inventory.csv` |',
    '| Serialization Register draft | Marker-derived draft from `phase-01-serialization-marker-inventory.csv` |',
    '| Existing wiki pages | Present: `phase-01-documentation-inventory.csv` |',
    '| `SystemAudit.md` | Present under `docs/wiki/SystemAudit.md` |',
    '',
    '## Generated Outputs',
    '',
    '| Output | Rows | Purpose |',
    '| --- | ---: | --- |',
    "| `system-cards/` | $($cards.Count) | One generated card per seeded high-risk system. |",
    "| `phase-04-system-card-index.csv` | $($cards.Count) | Card metadata and verification status. |",
    "| `phase-04-system-owner-map.csv` | $($ownerRows.Count) | Matched file-to-system ownership evidence. |",
    "| `system-owner-map.csv` | $($ownerRows.Count) | Canonical owner map copy. |",
    "| `phase-04-system-card-backlog.csv` | $($backlogRows.Count) | Follow-up work for partial or blocked cards. |",
    "| `phase-04-high-risk-system-priority-list.csv` | $($priorityRows.Count) | Review order for seeded high-risk systems. |",
    '',
    '## Verification Status Counts',
    '',
    '| Status | Count |',
    '| --- | ---: |'
)

foreach ($group in ($cards | Group-Object VerificationStatus | Sort-Object Name))
{
    $summaryLines += "| $($group.Name) | $($group.Count) |"
}

$summaryLines += @(
    '',
    '## Exit Criteria',
    '',
    '- Each seeded high-risk system has a generated system card.',
    '- Cards list matched source files, docs, commands, runtime hooks, serialized marker files, config references, dependencies, and follow-up work.',
    '- Generated cards are marked `PartiallyVerified`, `NeedsRuntimeReview`, `NeedsSerializationReview`, or `Blocked`; no card claims full verification from marker data alone.',
    '- Full dependency and synergy claims are deferred to Phases 8 and 9.'
)

Set-Content -LiteralPath $summaryPath -Value $summaryLines -Encoding UTF8

[pscustomobject]@{
    OutputDir = $OutputDir
    CardCount = $cards.Count
    OwnerMapRows = $ownerRows.Count
    BacklogRows = $backlogRows.Count
    PriorityRows = $priorityRows.Count
}
