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

function Get-RequiredInput
{
    param([string]$FileName)

    $path = Join-Path $OutputDir $FileName
    if (-not (Test-Path -LiteralPath $path))
    {
        throw "Required Phase 12 input is missing: $FileName"
    }

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

$systemCardsPath = Get-RequiredInput 'phase-04-system-card-index.csv'
$dependencyPath = Get-RequiredInput 'dependency-graph.csv'
$serializationPath = Get-RequiredInput 'serialization-register.csv'
$projectTruthPath = Get-RequiredInput 'project-truth-register.csv'
$documentationPath = Get-RequiredInput 'documentation-truth-table.csv'
$findingPath = Get-RequiredInput 'risk-track-findings.csv'
$ownerMapPath = Get-RequiredInput 'system-owner-map.csv'

$systemCards = @(Import-Csv -LiteralPath $systemCardsPath)
$dependencies = @(Import-Csv -LiteralPath $dependencyPath)
$serializers = @(Import-Csv -LiteralPath $serializationPath)
$projectTruth = @(Import-Csv -LiteralPath $projectTruthPath)
$documentation = @(Import-Csv -LiteralPath $documentationPath)
$findings = @(Import-Csv -LiteralPath $findingPath)
$ownerMap = @(Import-Csv -LiteralPath $ownerMapPath)

function Count-PrefixRows
{
    param(
        [object[]]$Rows,
        [string]$PathProperty,
        [string]$Prefix
    )

    $normalizedPrefix = ($Prefix -replace '\\', '/').TrimEnd('/') + '/'
    return @($Rows | Where-Object {
        $value = ($_.$PathProperty -replace '\\', '/')
        $value -eq $Prefix -or $value.StartsWith($normalizedPrefix, [System.StringComparison]::OrdinalIgnoreCase)
    }).Count
}

function Get-SystemDocPaths
{
    param([string]$System)

    $normalized = (($System.ToLowerInvariant()) -replace '[^a-z0-9]', '')
    $paths = @()
    foreach ($doc in $documentation)
    {
        $text = (($doc.DocPath + ' ' + $doc.Title + ' ' + $doc.VerifiedSourceFiles + ' ' + $doc.IndexText).ToLowerInvariant()) -replace '[^a-z0-9]', ''
        if ($text.Contains($normalized))
        {
            $paths += $doc.DocPath
        }
    }

    return (($paths | Sort-Object -Unique) -join ';')
}

function New-MoveProposal
{
    param(
        [string]$System,
        [string]$CurrentPath,
        [string]$TargetPath,
        [string]$Reason,
        [string]$NamespaceChange,
        [string]$ApprovalGate
    )

    $serializerCount = Count-PrefixRows $serializers 'File' $CurrentPath
    $projectIncludeCount = Count-PrefixRows $projectTruth 'Path' $CurrentPath
    $findingCount = @($findings | Where-Object { ($_.System -eq $System) -or ($_.File -replace '\\', '/') -like (($CurrentPath -replace '\\', '/') + '*') }).Count
    $saveRisk = 'SafeNoSerializedTypesDetected'
    if ($serializerCount -gt 0)
    {
        $saveRisk = 'CautionFileMoveAllowedOnlyWithoutNamespaceOrTypeRename'
    }
    if ($System -eq 'PlayerMobile Core')
    {
        $saveRisk = 'DoNotMove'
    }

    return [pscustomobject]@{
        CurrentPath = $CurrentPath
        TargetPath = $TargetPath
        System = $System
        Reason = $Reason
        NamespaceChange = $NamespaceChange
        SaveRisk = $saveRisk
        SerializedTypeCount = $serializerCount
        RiskFindingCount = $findingCount
        ProjectUpdate = ("Update {0} `Scripts.csproj` include rows after the move; rerun project truth." -f $projectIncludeCount)
        DocsUpdate = Get-SystemDocPaths $System
        Verification = 'Re-run New-ProjectTruthRegister.ps1; re-run dependency, serialization, and docs trace outputs for moved paths; run Scripts.csproj build after Phase 2 project drift is repaired.'
        Rollback = ("Move `{0}` back to `{1}`, restore previous `Scripts.csproj` includes, rerun project truth, and revert docs source traces." -f $TargetPath, $CurrentPath)
        ApprovalGate = $ApprovalGate
        Phase12Status = 'DesignOnlyNotExecuted'
    }
}

$layoutRows = @(
    [pscustomobject]@{ TargetFolder = 'Data/Scripts/Custom/Core'; OwnershipRule = 'Small Confictura-native shared services that are not RunUO framework roots and do not own broad persistence surfaces.'; ExampleSystems = 'CharacterLevelService only after progression boundary review'; MovePolicy = 'Design candidate; avoid PlayerMobile and framework roots.' },
    [pscustomobject]@{ TargetFolder = 'Data/Scripts/Custom/Progression'; OwnershipRule = 'Confictura-native character progression systems and active-play progression helpers.'; ExampleSystems = 'Character Level; Offline Skill Training'; MovePolicy = 'Allowed only as small batches with project truth updates.' },
    [pscustomobject]@{ TargetFolder = 'Data/Scripts/Custom/PvE'; OwnershipRule = 'Confictura-native automated PvE objective systems.'; ExampleSystems = 'Random Encounters; Monster Nests'; MovePolicy = 'Keep reward/balance review tied to Phase 9/10 findings.' },
    [pscustomobject]@{ TargetFolder = 'Data/Scripts/Custom/PvP'; OwnershipRule = 'Confictura-native PvP consent, event gates, and combat-policy extensions.'; ExampleSystems = 'PvP Consent'; MovePolicy = 'Requires consent/region/government policy review.' },
    [pscustomobject]@{ TargetFolder = 'Data/Scripts/Custom/Combat'; OwnershipRule = 'Confictura-native combat behavior extensions that are not magic schools or core weapons.'; ExampleSystems = 'AI Overhaul; OmniAI if isolated'; MovePolicy = 'Imported AI packages may be contained without restyling.' },
    [pscustomobject]@{ TargetFolder = 'Data/Scripts/Custom/Magic'; OwnershipRule = 'Confictura-native magic extensions outside established `Data/Scripts/Magic` school roots.'; ExampleSystems = 'Future custom magic utilities'; MovePolicy = 'Do not move established Magic root schools without separate save/build review.' },
    [pscustomobject]@{ TargetFolder = 'Data/Scripts/Custom/Economy'; OwnershipRule = 'Confictura-native economy systems that are not RunUO vendors, banking, or Trades frameworks.'; ExampleSystems = 'Government economy extensions'; MovePolicy = 'Requires economy-loop review.' },
    [pscustomobject]@{ TargetFolder = 'Data/Scripts/Custom/Crafting'; OwnershipRule = 'Confictura-native custom crafting packages that are not existing Trades frameworks.'; ExampleSystems = 'Homestead only if contained as an imported package'; MovePolicy = 'Preserve imported package structure.' },
    [pscustomobject]@{ TargetFolder = 'Data/Scripts/Custom/Housing'; OwnershipRule = 'Custom housing extensions that do not belong in RunUO `Items/Houses` framework.'; ExampleSystems = 'Future custom housing tools'; MovePolicy = 'Do not move core house items or save-heavy house signs.' },
    [pscustomobject]@{ TargetFolder = 'Data/Scripts/Custom/Travel'; OwnershipRule = 'Custom travel tools outside RunUO boat and door framework code.'; ExampleSystems = 'BoatNavigationTotem'; MovePolicy = 'Keep core boats in `Items/Boats`.' },
    [pscustomobject]@{ TargetFolder = 'Data/Scripts/Custom/Government'; OwnershipRule = 'Confictura government package after persistence and project truth cleanup.'; ExampleSystems = 'Government System'; MovePolicy = 'Folder move only; namespace/type rename forbidden without migration.' },
    [pscustomobject]@{ TargetFolder = 'Data/Scripts/Custom/Events'; OwnershipRule = 'Staff or automated event packages that create temporary shard-wide gameplay.'; ExampleSystems = 'Invasion System; Champions custom extensions'; MovePolicy = 'Review staff/event balance and save risk first.' },
    [pscustomobject]@{ TargetFolder = 'Data/Scripts/Custom/StaffTools'; OwnershipRule = 'Staff-only commands, gumps, and world-editing tools.'; ExampleSystems = 'Static Gump Tool; Staff Toolbar'; MovePolicy = 'Access review required before moves.' },
    [pscustomobject]@{ TargetFolder = 'Data/Scripts/Custom/Integrations'; OwnershipRule = 'Bridge code between Confictura systems and imported services.'; ExampleSystems = 'Random Encounter XMLSpawner bridges'; MovePolicy = 'Keep dependency comments and docs source traces aligned.' },
    [pscustomobject]@{ TargetFolder = 'Data/Scripts/Custom/Legacy'; OwnershipRule = 'Compatibility shims retained for saves, commands, or old behavior.'; ExampleSystems = 'Only after active hooks are classified.'; MovePolicy = 'Do not delete or disable from this phase.' },
    [pscustomobject]@{ TargetFolder = 'Data/Scripts/Custom/ThirdParty'; OwnershipRule = 'Imported packages whose upstream shape matters to maintenance.'; ExampleSystems = 'XMLSpawner; Homestead; Staff Toolbar; OmniAI'; MovePolicy = 'Contain or document; avoid style-only rewrites.' }
)

$moveRows = @(
    (New-MoveProposal 'Character Level' 'Data/Scripts/Custom/CharacterLevel' 'Data/Scripts/Custom/Progression/CharacterLevel' 'Progression service is Confictura-native and already isolated.' 'NoneAllowedForFirstMove' 'After Phase 2 project drift repair and focused Scripts.csproj update.'),
    (New-MoveProposal 'Offline Skill Training' 'Data/Scripts/Custom/Offline Skill Training' 'Data/Scripts/Custom/Progression/OfflineSkillTraining' 'Progression convenience system belongs with progression review, not loose Custom root.' 'NoneAllowedForFirstMove' 'After serializer review and project truth repair.'),
    (New-MoveProposal 'Random Encounters' 'Data/Scripts/Custom/RandomEncounters' 'Data/Scripts/Custom/PvE/RandomEncounters' 'Automated PvE objective system with Character Level and XMLSpawner dependencies.' 'NoneAllowedForFirstMove' 'After dependency docs update and project truth repair.'),
    (New-MoveProposal 'Monster Nests' 'Data/Scripts/Custom/MonsterNest' 'Data/Scripts/Custom/PvE/MonsterNests' 'Automated PvE objective system should be reviewed with Random Encounters and Champions.' 'NoneAllowedForFirstMove' 'Only after serializer paths are source-reviewed.'),
    (New-MoveProposal 'PvP Consent' 'Data/Scripts/Custom/PvPConsent' 'Data/Scripts/Custom/PvP/PvPConsent' 'Consent/event gate code should be grouped with PvP policy extensions.' 'NoneAllowedForFirstMove' 'After PvP Consent, Government, Regions, XMLPoints, and PlayerMobile policy review.'),
    (New-MoveProposal 'Government' 'Data/Scripts/Custom/Government System' 'Data/Scripts/Custom/Government/GovernmentSystem' 'Large Confictura-native civic package would gain clearer ownership.' 'ForbiddenWithoutMigration' 'Requires explicit approval because 148 serialized files and many gumps/hooks are involved.'),
    (New-MoveProposal 'Invasion' 'Data/Scripts/Custom/Invasion System' 'Data/Scripts/Custom/Events/InvasionSystem' 'Staff event package belongs under Events if package shape remains intact.' 'NoneAllowedForFirstMove' 'Requires staff workflow docs and serializer review.'),
    (New-MoveProposal 'AI Overhaul' 'Data/Scripts/Custom/AIOverhaul' 'Data/Scripts/Custom/Combat/AIOverhaul' 'Small Confictura-native combat behavior extension already isolated.' 'NoneAllowedForFirstMove' 'Respect nested AGENTS.md under current folder.'),
    (New-MoveProposal 'OmniAI' 'Data/Scripts/Custom/OmniAI' 'Data/Scripts/Custom/ThirdParty/OmniAI' 'Imported AI package should be contained rather than restyled.' 'NoneAllowedForFirstMove' 'Review AI Overhaul overlap first.'),
    (New-MoveProposal 'Static Gump Tool' 'Data/Scripts/Custom/OzThothStaticGump' 'Data/Scripts/Custom/StaffTools/StaticGumpTool' 'Staff-only static gump tool belongs with staff tooling.' 'NoneAllowedForFirstMove' 'Respect nested AGENTS.md under current folder.'),
    (New-MoveProposal 'Staff Toolbar' 'Data/Scripts/Custom/Staff Toolbar [2.0]' 'Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]' 'Imported staff toolbar package should be contained without restyling.' 'NoneAllowedForFirstMove' 'Access and staff workflow review required.'),
    (New-MoveProposal 'XMLSpawner' 'Data/Scripts/Custom/XMLSpawner' 'Data/Scripts/Custom/ThirdParty/XMLSpawner' 'XMLSpawner is imported/shared infrastructure and should be contained only if it reduces confusion.' 'ForbiddenWithoutMigration' 'Requires explicit approval; packet, attachment, world save/load, and project truth risks are high.'),
    (New-MoveProposal 'Homestead' "Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]" "Data/Scripts/Custom/ThirdParty/Vhaerun's CRL Homestead System [2.0]" 'Large imported Homestead package should remain understandable and un-restyled.' 'ForbiddenWithoutMigration' 'Requires explicit approval; high serializer volume and package substructure.'),
    (New-MoveProposal 'Clone Offline Player Characters' 'Data/Scripts/Custom/CloneOfflinePlayerCharacters' 'Data/Scripts/Custom/PvE/CloneOfflinePlayerCharacters' 'Ambient PvE/social-world system belongs with PvE review if moved.' 'NoneAllowedForFirstMove' 'Review AI and serialization risks first.')
)

$keepRows = @(
    [pscustomobject]@{ PathOrSystem = 'Data/Scripts/Items'; Decision = 'KeepInPlace'; Reason = 'RunUO item framework and thousands of save surfaces should not be forced into Custom.'; SaveRisk = 'High'; FollowUp = 'Classify system ownership and repair project truth before any local item extension move.' },
    [pscustomobject]@{ PathOrSystem = 'Data/Scripts/Mobiles'; Decision = 'KeepInPlace'; Reason = 'Mobile framework includes PlayerMobile and save-heavy NPC content.'; SaveRisk = 'High'; FollowUp = 'Do not move PlayerMobile; review individual custom extensions only.' },
    [pscustomobject]@{ PathOrSystem = 'Data/Scripts/Magic'; Decision = 'KeepInPlace'; Reason = 'Established magic school and spell framework roots already carry namespace/build expectations.'; SaveRisk = 'High'; FollowUp = 'Leave magic schools in place unless a later design isolates new custom-only extensions.' },
    [pscustomobject]@{ PathOrSystem = 'Data/Scripts/System'; Decision = 'KeepInPlace'; Reason = 'Startup, command, packet, help, skill, and framework wiring belongs in System.'; SaveRisk = 'High'; FollowUp = 'Audit active obsolete/system hooks before cleanup.' },
    [pscustomobject]@{ PathOrSystem = 'Data/Scripts/Trades'; Decision = 'KeepInPlace'; Reason = 'Crafting, harvest, bulk orders, and gardening have conventional RunUO/Trades organization.'; SaveRisk = 'High'; FollowUp = 'Repair moved gump includes before any Trades cleanup.' },
    [pscustomobject]@{ PathOrSystem = 'Data/Scripts/Quests'; Decision = 'KeepInPlace'; Reason = 'Quest content is a stable non-Custom root with many gump and data assumptions.'; SaveRisk = 'Caution'; FollowUp = 'Only move Confictura-native custom quest extensions after docs and build review.' },
    [pscustomobject]@{ PathOrSystem = 'PlayerMobile Core'; Decision = 'DoNotMove'; Reason = 'Phase 6 marks PlayerMobile as a central save and policy hub.'; SaveRisk = 'DoNotMove'; FollowUp = 'No move or namespace change without migration approval and save-load verification.' },
    [pscustomobject]@{ PathOrSystem = 'Vendor Core'; Decision = 'KeepInPlace'; Reason = 'Vendor framework is economy infrastructure with many serialized mobiles.'; SaveRisk = 'High'; FollowUp = 'Document economy dependencies; avoid folder churn.' },
    [pscustomobject]@{ PathOrSystem = 'Obsolete Scripts'; Decision = 'KeepInPlaceForNow'; Reason = 'Legacy code can still register commands/hooks or preserve old saves.'; SaveRisk = 'High'; FollowUp = 'Use Phase 13 backlog before disabling or moving.' }
)

$thirdPartyRows = @(
    [pscustomobject]@{ Package = 'XMLSpawner'; CurrentPath = 'Data/Scripts/Custom/XMLSpawner'; ProposedContainment = 'Data/Scripts/Custom/ThirdParty/XMLSpawner'; Policy = 'Contain only as a package; no style rewrites; no namespace/type rename.'; Evidence = 'Shared service with packet handlers, world hooks, attachments, and 84 serialized files.' },
    [pscustomobject]@{ Package = 'Homestead'; CurrentPath = "Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]"; ProposedContainment = "Data/Scripts/Custom/ThirdParty/Vhaerun's CRL Homestead System [2.0]"; Policy = 'Preserve imported subpackage names and local style; move only as whole package with explicit approval.'; Evidence = 'Imported package with 402 files and 372 serialized files.' },
    [pscustomobject]@{ Package = 'Staff Toolbar'; CurrentPath = 'Data/Scripts/Custom/Staff Toolbar [2.0]'; ProposedContainment = 'Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]'; Policy = 'Preserve imported package; verify staff access before move.'; Evidence = 'StaffEventTool and imported package naming.' },
    [pscustomobject]@{ Package = 'OmniAI'; CurrentPath = 'Data/Scripts/Custom/OmniAI'; ProposedContainment = 'Data/Scripts/Custom/ThirdParty/OmniAI'; Policy = 'Contain or keep in place; do not merge with AI Overhaul until behavior ownership is reviewed.'; Evidence = 'ImportedPackage classification and Phase 9 AI overlap risk.' }
)

$saveRows = New-Object System.Collections.ArrayList
foreach ($proposal in $moveRows)
{
    [void]$saveRows.Add([pscustomobject]@{
        System = $proposal.System
        CurrentPath = $proposal.CurrentPath
        SerializedTypeCount = $proposal.SerializedTypeCount
        SaveRisk = $proposal.SaveRisk
        NamespaceRule = $proposal.NamespaceChange
        RequiredApproval = $proposal.ApprovalGate
        SaveVerification = 'Compare serialization register before/after; no namespace/type rename; run save-load test where available; build after project truth repair.'
    })
}

$projectRows = New-Object System.Collections.ArrayList
foreach ($proposal in $moveRows)
{
    [void]$projectRows.Add([pscustomobject]@{
        System = $proposal.System
        CurrentPath = $proposal.CurrentPath
        TargetPath = $proposal.TargetPath
        ProjectIncludeAction = $proposal.ProjectUpdate
        PreMoveBlocker = 'Phase 2 project truth currently has missing compile targets; repair or isolate affected include group before executing moves.'
        Verification = $proposal.Verification
    })
}

$namespaceRows = New-Object System.Collections.ArrayList
foreach ($proposal in $moveRows)
{
    $namespaceDecision = 'NoNamespaceChange'
    if ($proposal.SerializedTypeCount -gt 0 -or $proposal.NamespaceChange -match 'Forbidden')
    {
        $namespaceDecision = 'NamespaceRenameForbiddenWithoutMigration'
    }

    [void]$namespaceRows.Add([pscustomobject]@{
        System = $proposal.System
        CurrentPath = $proposal.CurrentPath
        TargetPath = $proposal.TargetPath
        NamespaceDecision = $namespaceDecision
        Reason = 'World saves, source references, project includes, and docs source traces should be updated independently from any namespace migration.'
        MigrationRequirement = if ($namespaceDecision -eq 'NoNamespaceChange') { 'None for folder-only move.' } else { 'Explicit migration plan and save-load verification required.' }
    })
}

$docRows = New-Object System.Collections.ArrayList
foreach ($proposal in $moveRows)
{
    [void]$docRows.Add([pscustomobject]@{
        System = $proposal.System
        CurrentPath = $proposal.CurrentPath
        TargetPath = $proposal.TargetPath
        DocsToUpdate = $proposal.DocsUpdate
        RequiredDocActions = 'Update source traces, wiki links, system cards, dependency evidence, and project truth references after the move.'
        Verification = 'Re-run New-DocumentationTruthAudit.ps1 and New-DependencyGraph.ps1 after docs/source trace edits.'
    })
}

$designRows = @(
    [pscustomobject]@{ Section = 'OrganizationPrinciples'; Decision = 'Runtime role and save risk determine target folder.'; Evidence = 'Phase 12 plan and Phase 6/8/10 registers.' },
    [pscustomobject]@{ Section = 'NoImmediateMoves'; Decision = 'Phase 12 executes no source moves.'; Evidence = 'Design phase only; moves require Phase 13 backlog and focused implementation batches.' },
    [pscustomobject]@{ Section = 'NamespacePolicy'; Decision = 'Folder moves do not imply namespace/type rename.'; Evidence = 'RunUO save compatibility and Phase 6 move/rename risk.' },
    [pscustomobject]@{ Section = 'ProjectPolicy'; Decision = 'Scripts.csproj include edits are required for every executed move.'; Evidence = 'Phase 2 explicit project truth register.' },
    [pscustomobject]@{ Section = 'DocsPolicy'; Decision = 'Source traces and dependency evidence must move with code.'; Evidence = 'Phase 7 and Phase 8 outputs.' }
)

$canonicalPath = Export-AuditCsv @($designRows) 'reorganization-design.csv'
$layoutPath = Export-AuditCsv @($layoutRows) 'phase-12-target-layout-proposal.csv'
$movePath = Export-AuditCsv @($moveRows) 'phase-12-move-proposal-table.csv'
$keepPath = Export-AuditCsv @($keepRows) 'phase-12-keep-in-place-decisions.csv'
$thirdPartyPath = Export-AuditCsv @($thirdPartyRows) 'phase-12-third-party-containment-plan.csv'
$savePath = Export-AuditCsv @($saveRows) 'phase-12-save-compatibility-notes.csv'
$projectPath = Export-AuditCsv @($projectRows) 'phase-12-project-update-plan.csv'
$namespacePath = Export-AuditCsv @($namespaceRows) 'phase-12-namespace-plan.csv'
$docPath = Export-AuditCsv @($docRows) 'phase-12-documentation-move-plan.csv'

$designMdPath = Join-Path $OutputDir 'reorganization-design.md'
$designMd = New-Object System.Collections.ArrayList
[void]$designMd.Add('# Reorganization Design')
[void]$designMd.Add('')
[void]$designMd.Add(("Generated: {0}" -f (Get-Date -Format 'o')))
[void]$designMd.Add('')
[void]$designMd.Add('## Principles')
[void]$designMd.Add('')
[void]$designMd.Add('- Runtime role beats folder nostalgia.')
[void]$designMd.Add('- Folder moves do not authorize namespace or serialized type renames.')
[void]$designMd.Add('- Framework roots stay intact unless a later batch proves a custom extension can move safely.')
[void]$designMd.Add('- Imported packages may be contained, but not restyled.')
[void]$designMd.Add('- Every executed move needs `Scripts.csproj`, docs, dependency, serialization, verification, and rollback updates.')
[void]$designMd.Add('')
[void]$designMd.Add('## Proposed Custom Layout')
[void]$designMd.Add('')
[void]$designMd.Add('| Folder | Ownership Rule | Move Policy |')
[void]$designMd.Add('| --- | --- | --- |')
foreach ($row in $layoutRows)
{
    [void]$designMd.Add(("| `{0}` | {1} | {2} |" -f $row.TargetFolder, (Escape-MarkdownCell $row.OwnershipRule), (Escape-MarkdownCell $row.MovePolicy)))
}
[void]$designMd.Add('')
[void]$designMd.Add('## Move Design Status')
[void]$designMd.Add('')
[void]$designMd.Add(('{0} move proposals were generated. All are `DesignOnlyNotExecuted`; no files were moved in Phase 12.' -f $moveRows.Count))
[void]$designMd.Add('')
[void]$designMd.Add('## Hard Gates')
[void]$designMd.Add('')
[void]$designMd.Add('- Repair or explicitly plan around Phase 2 project truth drift before executing moves.')
[void]$designMd.Add('- Do not rename namespaces or serialized types without migration approval.')
[void]$designMd.Add('- Run project truth, serialization, dependency, documentation truth, and build verification for each executed batch.')

[System.IO.File]::WriteAllLines($designMdPath, [string[]]$designMd, (New-Object System.Text.UTF8Encoding($false)))

$summaryPath = Join-Path $OutputDir 'phase-12-summary.md'
$summary = New-Object System.Collections.ArrayList
[void]$summary.Add('# Phase 12 Reorganization Design Summary')
[void]$summary.Add('')
[void]$summary.Add(("Generated: {0}" -f (Get-Date -Format 'o')))
[void]$summary.Add('')
[void]$summary.Add('## Required Inputs')
[void]$summary.Add('')
[void]$summary.Add('| Input | Status |')
[void]$summary.Add('| --- | --- |')
[void]$summary.Add(('| System Cards | Present: `phase-04-system-card-index.csv` with {0} rows |' -f $systemCards.Count))
[void]$summary.Add(('| Dependency Graph | Present: `dependency-graph.csv` with {0} rows |' -f $dependencies.Count))
[void]$summary.Add(('| Serialization Register | Present: `serialization-register.csv` with {0} rows |' -f $serializers.Count))
[void]$summary.Add(('| Project Truth Register | Present: `project-truth-register.csv` with {0} rows |' -f $projectTruth.Count))
[void]$summary.Add(('| Documentation Truth Table | Present: `documentation-truth-table.csv` with {0} rows |' -f $documentation.Count))
[void]$summary.Add(('| Risk-Specific Findings | Present: `risk-track-findings.csv` with {0} rows |' -f $findings.Count))
[void]$summary.Add('')
[void]$summary.Add('## Generated Outputs')
[void]$summary.Add('')
[void]$summary.Add('| Output | Rows | Purpose |')
[void]$summary.Add('| --- | ---: | --- |')
[void]$summary.Add(('| `reorganization-design.csv` | {0} | Canonical design principles and hard gates. |' -f $designRows.Count))
[void]$summary.Add('| `reorganization-design.md` | 1 | Human-readable design narrative. |')
[void]$summary.Add(('| `phase-12-target-layout-proposal.csv` | {0} | Target folders and ownership rules. |' -f $layoutRows.Count))
[void]$summary.Add(('| `phase-12-move-proposal-table.csv` | {0} | Design-only move proposals with rollback and verification plans. |' -f $moveRows.Count))
[void]$summary.Add(('| `phase-12-keep-in-place-decisions.csv` | {0} | Existing roots and systems to preserve. |' -f $keepRows.Count))
[void]$summary.Add(('| `phase-12-third-party-containment-plan.csv` | {0} | Imported package containment rules. |' -f $thirdPartyRows.Count))
[void]$summary.Add(('| `phase-12-save-compatibility-notes.csv` | {0} | Save risk and migration gates for proposed moves. |' -f $saveRows.Count))
[void]$summary.Add(('| `phase-12-project-update-plan.csv` | {0} | `Scripts.csproj` update expectations for proposed moves. |' -f $projectRows.Count))
[void]$summary.Add(('| `phase-12-namespace-plan.csv` | {0} | Namespace and serialized type rename policy by move proposal. |' -f $namespaceRows.Count))
[void]$summary.Add(('| `phase-12-documentation-move-plan.csv` | {0} | Documentation update requirements for proposed moves. |' -f $docRows.Count))
[void]$summary.Add('')
[void]$summary.Add('## Exit Criteria')
[void]$summary.Add('')
[void]$summary.Add('- Target folders have ownership rules.')
[void]$summary.Add('- Existing framework roots have keep-in-place decisions.')
[void]$summary.Add('- Move proposals include save risk, project updates, documentation updates, verification, and rollback plans.')
[void]$summary.Add('- Imported systems have containment policy.')
[void]$summary.Add('- No source files or project files were moved in Phase 12.')

[System.IO.File]::WriteAllLines($summaryPath, [string[]]$summary, (New-Object System.Text.UTF8Encoding($false)))

[pscustomobject]@{
    ReorganizationDesign = Convert-ToRepoPath $canonicalPath
    ReorganizationDesignMarkdown = Convert-ToRepoPath $designMdPath
    TargetLayout = Convert-ToRepoPath $layoutPath
    MoveProposals = Convert-ToRepoPath $movePath
    KeepInPlace = Convert-ToRepoPath $keepPath
    ThirdPartyContainment = Convert-ToRepoPath $thirdPartyPath
    SaveCompatibilityNotes = Convert-ToRepoPath $savePath
    ProjectUpdatePlan = Convert-ToRepoPath $projectPath
    NamespacePlan = Convert-ToRepoPath $namespacePath
    DocumentationMovePlan = Convert-ToRepoPath $docPath
    Summary = Convert-ToRepoPath $summaryPath
    MoveRows = $moveRows.Count
    KeepRows = $keepRows.Count
    LayoutRows = $layoutRows.Count
}
