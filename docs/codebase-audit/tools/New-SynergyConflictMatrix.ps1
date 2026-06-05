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
        [int]$MaxLength = 260
    )

    if ($null -eq $Value)
    {
        return ''
    }

    $oneLine = ($Value -replace "`r?`n", ' ').Trim()
    if ($oneLine.Length -le $MaxLength)
    {
        return $oneLine
    }

    return ($oneLine.Substring(0, $MaxLength - 3) + '...')
}

function Normalize-Key
{
    param([string]$Value)

    if ($null -eq $Value)
    {
        return ''
    }

    return (($Value.ToLowerInvariant()) -replace '[^a-z0-9]', '')
}

function Get-PairKey
{
    param(
        [string]$A,
        [string]$B
    )

    if ([System.String]::Compare($A, $B, [System.StringComparison]::OrdinalIgnoreCase) -le 0)
    {
        return "$A||$B"
    }

    return "$B||$A"
}

function Add-ListValue
{
    param(
        [hashtable]$Map,
        [string]$Key,
        [object]$Value
    )

    if (-not $Map.ContainsKey($Key))
    {
        $Map[$Key] = New-Object System.Collections.ArrayList
    }

    [void]$Map[$Key].Add($Value)
}

function Get-RequiredInput
{
    param([string]$FileName)

    $path = Join-Path $OutputDir $FileName
    if (-not (Test-Path -LiteralPath $path))
    {
        throw "Required Phase 9 input is missing: $FileName"
    }

    return $path
}

$cardPath = Get-RequiredInput 'phase-04-system-card-index.csv'
$graphPath = Get-RequiredInput 'dependency-graph.csv'
$docTruthPath = Get-RequiredInput 'documentation-truth-table.csv'
$hookPath = Get-RequiredInput 'runtime-hook-map.csv'

$cards = @(Import-Csv -LiteralPath $cardPath)
$graph = @(Import-Csv -LiteralPath $graphPath)
$docTruth = @(Import-Csv -LiteralPath $docTruthPath)
$hooks = @(Import-Csv -LiteralPath $hookPath)

$systems = @($cards | Select-Object -ExpandProperty System)
$systemSet = @{}
foreach ($system in $systems)
{
    $systemSet[$system] = $true
}

$domainMap = @{
    'Character Level' = @('Progression')
    'Random Encounters' = @('PvE', 'Progression')
    'PvP Consent' = @('PvP', 'Combat')
    'Government' = @('Government', 'Economy', 'Documentation')
    'Homestead' = @('Economy', 'Crafting', 'Housing')
    'XMLSpawner' = @('Staff events', 'PvE', 'Documentation')
    'Invasion' = @('PvE', 'Staff events')
    'Champions' = @('PvE', 'Progression')
    'Monster Nests' = @('PvE', 'Exploration')
    'Clone Offline Player Characters' = @('AI', 'PvE')
    'Offline Skill Training' = @('Progression')
    'OmniAI' = @('AI', 'Combat')
    'AI Overhaul' = @('AI', 'Combat')
    'Staff Toolbar' = @('Staff events')
    'Static Gump Tool' = @('Staff events')
    'Spell Framework' = @('Magic')
    'Magic Schools' = @('Magic', 'Combat')
    'Crafting Core' = @('Crafting', 'Economy')
    'Harvest System' = @('Crafting', 'Economy')
    'Bulk Orders' = @('Crafting', 'Economy')
    'Gardening' = @('Crafting', 'Economy')
    'Housing' = @('Housing', 'Economy')
    'Boats' = @('Travel', 'Housing')
    'Regions' = @('World', 'PvP', 'Government')
    'PlayerMobile Core' = @('Core', 'Progression', 'Combat')
    'Vendor Core' = @('Economy')
    'Obsolete Scripts' = @('Legacy', 'Documentation')
}

$aliasMap = @{
    'Character Level' = @('character level', 'characterlevel', 'charlevel', 'level system')
    'Random Encounters' = @('random encounters', 'randomencounters')
    'PvP Consent' = @('pvp consent', 'pvpconsent', 'consent system')
    'Government' = @('government', 'government system', 'city bans', 'civic')
    'Homestead' = @('homestead', 'crl', 'crops', 'cooking', 'brewing', 'winecrafting', 'hunger')
    'XMLSpawner' = @('xmlspawner', 'xml spawner', 'xmlpoints', 'xmlattach', 'xmlquest')
    'Invasion' = @('invasion', 'invasion system')
    'Champions' = @('champion', 'champions', 'champion spawn')
    'Monster Nests' = @('monster nests', 'monsternests', 'monster nest')
    'Clone Offline Player Characters' = @('clone offline', 'offline player characters')
    'Offline Skill Training' = @('offline skill training', 'offline training')
    'OmniAI' = @('omniai', 'omni ai')
    'AI Overhaul' = @('ai overhaul', 'aioverhaul')
    'Staff Toolbar' = @('staff toolbar')
    'Static Gump Tool' = @('static gump', 'static gump tool', 'ozthothstaticgump')
    'Spell Framework' = @('spell framework', 'spell registry')
    'Magic Schools' = @('magic schools', 'bard', 'death knight', 'elementalism', 'holy man', 'jedi', 'mystic', 'research', 'syth')
    'Crafting Core' = @('crafting core', 'crafting')
    'Harvest System' = @('harvest system', 'harvest', 'resource')
    'Bulk Orders' = @('bulk orders', 'bulk order', 'bod')
    'Gardening' = @('gardening', 'plants')
    'Housing' = @('housing', 'house')
    'Boats' = @('boats', 'boat')
    'Regions' = @('regions', 'region')
    'PlayerMobile Core' = @('playermobile', 'player mobile')
    'Vendor Core' = @('vendor core', 'vendor')
    'Obsolete Scripts' = @('obsolete scripts', 'obsolete')
}

function Get-SystemMatches
{
    param([string]$Text)

    $normalizedText = Normalize-Key $Text
    $matches = New-Object System.Collections.ArrayList

    foreach ($system in $systems)
    {
        $tokens = @($aliasMap[$system])
        if ($tokens.Count -eq 0)
        {
            $tokens = @($system)
        }

        foreach ($token in $tokens)
        {
            $normalizedToken = Normalize-Key $token
            if ($normalizedToken.Length -gt 0 -and $normalizedText.Contains($normalizedToken))
            {
                [void]$matches.Add($system)
                break
            }
        }
    }

    return @($matches | Sort-Object -Unique)
}

$pairEdges = @{}
foreach ($edge in $graph)
{
    if ($systemSet.ContainsKey($edge.SourceSystem) -and $systemSet.ContainsKey($edge.TargetSystem) -and $edge.SourceSystem -ne $edge.TargetSystem)
    {
        $key = Get-PairKey $edge.SourceSystem $edge.TargetSystem
        Add-ListValue $pairEdges $key $edge
    }
}

$hookCountBySystem = @{}
foreach ($hook in $hooks)
{
    if (-not $hookCountBySystem.ContainsKey($hook.System))
    {
        $hookCountBySystem[$hook.System] = 0
    }

    $hookCountBySystem[$hook.System]++
}

$docRiskRows = New-Object System.Collections.ArrayList
$docRiskBySystem = @{}
foreach ($doc in $docTruth)
{
    $reasons = New-Object System.Collections.ArrayList

    if ($doc.Scope -eq 'Canonical' -and $doc.SourceTracePresent -ne 'Yes')
    {
        [void]$reasons.Add('Canonical page missing Source Trace')
    }
    if ($doc.LegacySlug -eq 'True' -and $doc.MissingClaims)
    {
        [void]$reasons.Add('Alias or legacy slug has independent claims')
    }
    if ($doc.StaleClaims)
    {
        [void]$reasons.Add('Stale or Needs Rework marker present')
    }
    if ($doc.MissingSourceFiles)
    {
        [void]$reasons.Add('Referenced source path missing')
    }
    if ($doc.MissingClaims)
    {
        [void]$reasons.Add('Missing source-backed claim or alias cleanup needed')
    }

    if ($reasons.Count -gt 0)
    {
        $text = @(
            $doc.DocPath,
            $doc.Title,
            $doc.IndexText,
            $doc.VerifiedSourceFiles,
            $doc.RuntimeHooksCovered,
            $doc.SerializationCovered
        ) -join ' '

        $matchedSystems = @(Get-SystemMatches $text)
        if ($matchedSystems.Count -eq 0)
        {
            $matchedSystems = @('Unknown')
        }

        foreach ($matchedSystem in $matchedSystems)
        {
            $risk = [pscustomobject]@{
                RiskId = ('DOC-RISK-{0:0000}' -f ($docRiskRows.Count + 1))
                System = $matchedSystem
                DocPath = $doc.DocPath
                Title = $doc.Title
                RiskReason = Join-Unique @($reasons)
                CanonicalPage = $doc.CanonicalPage
                SourceTracePresent = $doc.SourceTracePresent
                StaleClaims = $doc.StaleClaims
                MissingSourceFiles = $doc.MissingSourceFiles
                ExistingBacklogIds = $doc.ExistingBacklogIds
                GeneratedBacklogId = $doc.GeneratedBacklogId
                Recommendation = 'Resolve in documentation backlog with source traces before relying on player-facing claims.'
                Evidence = Trim-Text ("Documentation truth row: {0}" -f $doc.DocPath)
            }

            [void]$docRiskRows.Add($risk)

            if ($matchedSystem -ne 'Unknown')
            {
                Add-ListValue $docRiskBySystem $matchedSystem $risk
            }
        }
    }
}

$needsReworkRows = New-Object System.Collections.ArrayList
$needsReworkBySystem = @{}
$docsRoot = Join-Path $RepoRoot 'docs'
if (Test-Path -LiteralPath $docsRoot)
{
    $markdownFiles = @(Get-ChildItem -LiteralPath $docsRoot -Recurse -File -Filter '*.md' | Where-Object {
        $_.FullName -notlike (Join-Path $RepoRoot 'docs\codebase-audit\*')
    })

    foreach ($file in $markdownFiles)
    {
        $lineNumber = 0
        foreach ($line in [System.IO.File]::ReadLines($file.FullName))
        {
            $lineNumber++
            if ($line -match 'Needs Rework|TODO|stub')
            {
                $repoPath = Convert-ToRepoPath $file.FullName
                $matchedSystems = @(Get-SystemMatches ($repoPath + ' ' + $line))
                if ($matchedSystems.Count -eq 0)
                {
                    $matchedSystems = @('Unknown')
                }

                foreach ($matchedSystem in $matchedSystems)
                {
                    $row = [pscustomobject]@{
                        System = $matchedSystem
                        File = $repoPath
                        Line = $lineNumber
                        Marker = Trim-Text $line 180
                    }

                    [void]$needsReworkRows.Add($row)

                    if ($matchedSystem -ne 'Unknown')
                    {
                        Add-ListValue $needsReworkBySystem $matchedSystem $row
                    }
                }
            }
        }
    }
}

$manualRules = @{}
function Add-ManualRule
{
    param(
        [string]$A,
        [string]$B,
        [string[]]$Labels,
        [string]$Summary,
        [string]$RiskType,
        [string]$ObjectiveImpact,
        [bool]$StaffDependency,
        [string]$Recommendation,
        [string]$Evidence
    )

    if (-not $systemSet.ContainsKey($A) -or -not $systemSet.ContainsKey($B))
    {
        return
    }

    $key = Get-PairKey $A $B
    Add-ListValue $manualRules $key ([pscustomobject]@{
        Labels = ($Labels -join ';')
        Summary = $Summary
        RiskType = $RiskType
        ObjectiveImpact = $ObjectiveImpact
        StaffDependency = $StaffDependency
        Recommendation = $Recommendation
        Evidence = $Evidence
    })
}

Add-ManualRule 'Character Level' 'Random Encounters' @('Supports') 'Encounter objectives can use level state to scale goals and pacing.' 'GameplaySynergy' 'Progression and exploration goals become more objective-aware.' $false 'Preserve the level-to-encounter relationship during cleanup and document the contract.' 'Phase 9 synergy checklist: Character Level making Random Encounters objective-aware.'
Add-ManualRule 'Government' 'Housing' @('Supports') 'Civic systems can give housing ownership a social and territorial purpose.' 'GameplaySynergy' 'Medium and long-term civic goals.' $false 'Keep civic ownership and housing claims explicit in reorganization design.' 'Phase 9 synergy checklist: Government giving civic goals to housing/economy systems.'
Add-ManualRule 'Government' 'Vendor Core' @('Supports', 'BalanceRisk') 'City vendors, taxes, and treasuries can create civic economy goals, but can also distort gold flow.' 'EconomyBalance' 'Social and economy goals.' $false 'Review tax/vendor reward loops before tuning economy changes.' 'Phase 9 synergy checklist: Government economy relationships.'
Add-ManualRule 'Government' 'Homestead' @('Supports', 'BalanceRisk') 'Homestead production can deepen non-combat economy if civic taxes and vendor flows are bounded.' 'EconomyBalance' 'Crafting and civic economy goals.' $false 'Preserve civic economy hooks but backlog balance review for production and treasury loops.' 'Phase 9 synergy checklist: Homestead deepening non-combat economy.'
Add-ManualRule 'PvP Consent' 'Government' @('Overrides', 'Conflicts', 'BalanceRisk') 'Consent rules, city bans, wars, and civic enforcement can produce competing PvP policy decisions.' 'PolicyConflict' 'Social and PvP goals can conflict if enforcement bypasses consent expectations.' $false 'Create a repair/backlog item that reviews war, ban, region, and consent precedence together.' 'Phase 9 conflict checklist: PvP Consent versus Government war and city-ban rules.'
Add-ManualRule 'PvP Consent' 'Regions' @('Overrides', 'DependsOn', 'Conflicts') 'Region rules can become the policy boundary where PvP consent is allowed, blocked, or bypassed.' 'PolicyConflict' 'PvP safety and event goals depend on region policy clarity.' $false 'Document and review region precedence before any PvP or region cleanup.' 'Phase 9 conflict checklist: PvP Consent, regions, and event gates.'
Add-ManualRule 'PvP Consent' 'XMLSpawner' @('Bypasses', 'Conflicts', 'StaffDependency', 'BalanceRisk') 'XMLPoints and staff event overrides can bypass normal PvP consent expectations.' 'PolicyConflict' 'Staff-driven event goals may conflict with normal PvP expectations.' $true 'Require an explicit event override policy and staff documentation.' 'Phase 9 conflict checklist: XMLPoints event overrides versus normal PvP consent.'
Add-ManualRule 'PvP Consent' 'PlayerMobile Core' @('DependsOn', 'Overrides') 'PvP consent persistence and combat policy depend on PlayerMobile-facing state and checks.' 'CorePolicyDependency' 'PvP goals rely on player state staying save-compatible.' $false 'Avoid PlayerMobile changes without consent, notoriety, combat, and save review.' 'High-risk systems list: PvP Consent interactions with PlayerMobile and harmful/beneficial checks.'
Add-ManualRule 'Spell Framework' 'Magic Schools' @('Supports', 'BalanceRisk') 'Shared spell registration enables varied build identity, while high-ID families can stress registry capacity.' 'MagicBalance' 'Combat build identity and long-term character goals.' $false 'Preserve the framework/school split and backlog spell ID capacity review.' 'Phase 9 synergy and conflict checklist: magic schools and spell registry capacity.'
Add-ManualRule 'Homestead' 'Crafting Core' @('Supports', 'BalanceRisk') 'Homestead expands crafting into production loops that need pacing and resource review.' 'EconomyBalance' 'Crafting and economy goals.' $false 'Treat production loops as balance risk, not a serializer/build correctness issue.' 'Phase 9 conflict checklist: crafting/resource loops versus progression pacing.'
Add-ManualRule 'Homestead' 'Harvest System' @('Supports', 'BalanceRisk') 'Harvest resources feed homestead production and can accelerate non-combat progression.' 'EconomyBalance' 'Crafting and economy goals.' $false 'Review resource rates before economy tuning.' 'Phase 9 conflict checklist: crafting/resource loops versus progression pacing.'
Add-ManualRule 'Homestead' 'Gardening' @('Supports', 'BalanceRisk') 'Gardening and crops can support homestead production, food, and trade loops.' 'EconomyBalance' 'Crafting and economy goals.' $false 'Preserve non-combat depth but backlog resource-loop tuning.' 'Phase 9 synergy checklist: Homestead deepening non-combat economy.'
Add-ManualRule 'Homestead' 'Bulk Orders' @('BalanceRisk', 'Duplicates') 'Bulk order rewards and homestead production can duplicate crafting progression incentives.' 'EconomyBalance' 'Crafting progression goals may stack too quickly.' $false 'Review reward overlap before balance changes.' 'Phase 9 conflict checklist: crafting/resource loops versus progression pacing.'
Add-ManualRule 'Homestead' 'Vendor Core' @('BalanceRisk', 'DependsOn') 'Vendor outlets can turn homestead production into gold flow that needs economy review.' 'EconomyBalance' 'Crafting economy goals.' $false 'Backlog vendor price and output review for homestead products.' 'Phase 9 conflict checklist: crafting/resource loops versus progression pacing.'
Add-ManualRule 'Random Encounters' 'Champions' @('BalanceRisk', 'Duplicates') 'Automated encounter rewards and champion rewards can stack PvE progression.' 'PvEBalance' 'Exploration and combat goals can accelerate progression.' $false 'Review reward cadence across automated PvE systems.' 'Phase 9 conflict checklist: staff-spawned events versus automated PvE systems.'
Add-ManualRule 'Random Encounters' 'Invasion' @('BalanceRisk', 'StaffDependency') 'Automated encounters and invasions can overlap danger/reward pacing, especially when staff events are active.' 'PvEBalance' 'Exploration and staff event goals can collide.' $true 'Review event windows and reward stacking.' 'Phase 9 conflict checklist: staff-spawned events versus automated PvE systems.'
Add-ManualRule 'Random Encounters' 'Monster Nests' @('BalanceRisk', 'Duplicates') 'Encounter and nest systems can duplicate roaming PvE objectives.' 'PvEBalance' 'Exploration goals may become repetitive or over-rewarded.' $false 'Keep distinct objective roles for encounters and nests.' 'Phase 9 player objective review: exploration goals.'
Add-ManualRule 'Champions' 'Invasion' @('BalanceRisk', 'StaffDependency') 'Champion and invasion events can both serve group PvE spikes and reward bursts.' 'PvEBalance' 'Staff-driven and long-term PvE goals can overlap.' $true 'Review event reward tables and staff scheduling assumptions.' 'Phase 9 conflict checklist: staff-spawned events versus automated PvE systems.'
Add-ManualRule 'Champions' 'Monster Nests' @('BalanceRisk', 'Duplicates') 'Champion spawns and monster nests can duplicate escalation-style PvE goals.' 'PvEBalance' 'Group combat and exploration goals overlap.' $false 'Document distinct purpose or backlog consolidation review.' 'Phase 9 player objective review: PvE goals.'
Add-ManualRule 'Invasion' 'XMLSpawner' @('Supports', 'StaffDependency') 'XMLSpawner can provide event tooling and quest/event behaviors for invasion-style content.' 'StaffEventSupport' 'Social and staff-driven goals.' $true 'Preserve staff event tooling while documenting manual dependencies.' 'Phase 9 synergy checklist: XMLSpawner supporting staff events and quests.'
Add-ManualRule 'XMLSpawner' 'Staff Toolbar' @('Supports', 'StaffDependency') 'Staff toolbar workflows can expose or support XMLSpawner-heavy event operations.' 'StaffEventSupport' 'Staff-driven goals.' $true 'Keep staff-only access and documentation visible.' 'Phase 9 staff-dependency review.'
Add-ManualRule 'XMLSpawner' 'Static Gump Tool' @('Supports', 'StaffDependency') 'Static gump tooling can support XMLSpawner events and staff-authored content.' 'StaffEventSupport' 'Staff-driven goals.' $true 'Keep staff-only access and docs visible.' 'Phase 9 staff-dependency review.'
Add-ManualRule 'XMLSpawner' 'Obsolete Scripts' @('Conflicts', 'StaffDependency') 'Legacy or obsolete scripts can still register hooks or commands that obscure XMLSpawner behavior.' 'LegacyMaintenanceRisk' 'Staff workflows can be confusing when stale hooks remain active.' $true 'Backlog source review for obsolete hook registrations before cleanup.' 'High-risk systems list: obsolete or legacy files that still register commands or hooks.'
Add-ManualRule 'AI Overhaul' 'OmniAI' @('Duplicates', 'Conflicts', 'BalanceRisk') 'Multiple AI layers can duplicate combat behavior or produce unclear authority.' 'CombatBalance' 'Combat difficulty and PvE goals can become inconsistent.' $false 'Review AI ownership boundaries before changing combat behavior.' 'High-risk review area: OmniAI and AI Overhaul.'
Add-ManualRule 'Clone Offline Player Characters' 'OmniAI' @('Supports', 'BalanceRisk') 'Offline player clones rely on AI-like behavior and can affect PvE danger or reward expectations.' 'CombatBalance' 'PvE and social-world goals.' $false 'Review clone combat power and loot/reward interactions.' 'High-risk review area: AI and offline player systems.'
Add-ManualRule 'Clone Offline Player Characters' 'AI Overhaul' @('Supports', 'BalanceRisk') 'AI behavior changes can affect offline player clone difficulty and world behavior.' 'CombatBalance' 'PvE and social-world goals.' $false 'Review clone behavior after AI changes.' 'High-risk review area: AI and offline player systems.'
Add-ManualRule 'Offline Skill Training' 'Character Level' @('Supports', 'BalanceRisk', 'Bypasses') 'Offline training can support progression goals but can bypass active-play danger and objective pacing.' 'ProgressionBalance' 'Progression goals may advance without active objectives.' $false 'Separate accepted convenience from progression bypass risk in the backlog.' 'Phase 9 balance review: bypass intended danger or grants permanent power too quickly.'
Add-ManualRule 'Offline Skill Training' 'Random Encounters' @('BalanceRisk', 'Bypasses') 'Offline progression can bypass danger created by active random encounters.' 'ProgressionBalance' 'Exploration danger may not gate progression.' $false 'Review offline gains against active encounter reward pacing.' 'Phase 9 balance review: bypass intended danger.'
Add-ManualRule 'Crafting Core' 'Harvest System' @('Supports') 'Harvest resources are a normal upstream dependency for crafting.' 'GameplaySynergy' 'Crafting and economy goals.' $false 'Preserve source-to-crafting flow and document rates separately.' 'Phase 9 domain review: Crafting and Economy.'
Add-ManualRule 'Crafting Core' 'Bulk Orders' @('Supports', 'BalanceRisk') 'Bulk orders provide structured objectives for crafting progression.' 'CraftingBalance' 'Medium-term crafting goals.' $false 'Preserve objective loop; review reward acceleration separately.' 'Phase 9 player objective review: medium-term crafting goals.'
Add-ManualRule 'Crafting Core' 'Gardening' @('Supports', 'BalanceRisk') 'Gardening can feed crafting inputs and economy loops.' 'EconomyBalance' 'Crafting and economy goals.' $false 'Keep input relationship visible in dependency docs.' 'Phase 9 domain review: Crafting and Economy.'
Add-ManualRule 'Harvest System' 'Gardening' @('Supports', 'BalanceRisk') 'Harvest and gardening systems both provide resources and can stack supply rates.' 'EconomyBalance' 'Resource acquisition goals.' $false 'Review resource faucet overlap before tuning.' 'Phase 9 conflict checklist: crafting/resource loops versus progression pacing.'
Add-ManualRule 'Boats' 'Regions' @('DependsOn') 'Boat movement and placement are region-sensitive world interactions.' 'TravelPolicyDependency' 'Travel and exploration goals.' $false 'Review region assumptions before boat or travel cleanup.' 'Phase 9 domain review: Travel and Regions.'
Add-ManualRule 'Staff Toolbar' 'Static Gump Tool' @('Supports', 'Duplicates', 'StaffDependency') 'Staff UI tools support event operations but can duplicate administrative surfaces.' 'StaffWorkflowRisk' 'Staff-driven goals.' $true 'Document separate staff workflows or backlog consolidation review.' 'Phase 9 staff-dependency review.'
Add-ManualRule 'Invasion' 'Staff Toolbar' @('Supports', 'StaffDependency') 'Invasion operations can depend on staff tooling for setup, moderation, or intervention.' 'StaffEventSupport' 'Staff-driven goals.' $true 'Document required access and manual steps.' 'Phase 9 staff-dependency review.'
Add-ManualRule 'Invasion' 'Static Gump Tool' @('Supports', 'StaffDependency') 'Static gump tooling may support event presentation or staff-authored invasion content.' 'StaffEventSupport' 'Staff-driven goals.' $true 'Document required access and manual steps.' 'Phase 9 staff-dependency review.'
Add-ManualRule 'XMLSpawner' 'Random Encounters' @('Conflicts', 'BalanceRisk', 'StaffDependency') 'Staff-spawned XML events can overlap automated encounter objectives and rewards.' 'PvEBalance' 'Exploration and staff event goals can collide.' $true 'Review event reward and spawn precedence.' 'Phase 9 conflict checklist: staff-spawned events versus automated PvE systems.'
Add-ManualRule 'XMLSpawner' 'Champions' @('Supports', 'BalanceRisk', 'StaffDependency') 'XMLSpawner can support champion/event content, but staff rewards can stack with normal champion rewards.' 'PvEBalance' 'Group PvE and staff event goals.' $true 'Document staff event overrides and reward policy.' 'Phase 9 conflict checklist: staff-spawned events versus automated PvE systems.'
Add-ManualRule 'XMLSpawner' 'Monster Nests' @('Conflicts', 'BalanceRisk', 'StaffDependency') 'Staff-spawned XML content can obscure or duplicate automated nest objectives.' 'PvEBalance' 'Exploration and staff event goals.' $true 'Review spawn overlap and documentation.' 'Phase 9 conflict checklist: staff-spawned events versus automated PvE systems.'

$objectiveMap = @{
    'Character Level' = @{
        Immediate = 'Visible progression feedback.'
        Medium = 'Level milestones and unlock pacing.'
        Long = 'Long-term character growth.'
        SocialStaff = ''
        Exploration = ''
        CraftEconomy = ''
        Progression = 'Primary progression goal system.'
    }
    'Random Encounters' = @{
        Immediate = 'Short exploration combat objective.'
        Medium = 'Repeatable roaming challenge loop.'
        Long = ''
        SocialStaff = ''
        Exploration = 'Primary exploration objective source.'
        CraftEconomy = ''
        Progression = 'Can scale with character progress.'
    }
    'PvP Consent' = @{
        Immediate = 'Clear opt-in or opt-out combat expectation.'
        Medium = 'PvP safety and event eligibility.'
        Long = ''
        SocialStaff = 'Needs clear staff/event override policy.'
        Exploration = ''
        CraftEconomy = ''
        Progression = ''
    }
    'Government' = @{
        Immediate = 'Civic participation and city status.'
        Medium = 'Elections, taxes, vendors, city rules.'
        Long = 'Long-term civic control.'
        SocialStaff = 'Social and possibly staff-moderated goals.'
        Exploration = ''
        CraftEconomy = 'Civic economy and treasury goals.'
        Progression = ''
    }
    'Homestead' = @{
        Immediate = 'Production and gathering tasks.'
        Medium = 'Cooking, crops, brewing, and trade loops.'
        Long = 'Non-combat economy identity.'
        SocialStaff = ''
        Exploration = ''
        CraftEconomy = 'Primary non-combat economy expansion.'
        Progression = ''
    }
    'XMLSpawner' = @{
        Immediate = 'Staff-authored content and event triggers.'
        Medium = 'Quest/event scripting.'
        Long = 'Reusable event infrastructure.'
        SocialStaff = 'Primary staff event dependency.'
        Exploration = 'Can create authored exploration.'
        CraftEconomy = ''
        Progression = ''
    }
    'Invasion' = @{
        Immediate = 'Event combat pressure.'
        Medium = 'Group event response.'
        Long = ''
        SocialStaff = 'Staff event dependency likely.'
        Exploration = ''
        CraftEconomy = ''
        Progression = ''
    }
    'Champions' = @{
        Immediate = 'Group combat challenge.'
        Medium = 'Escalation and reward objective.'
        Long = 'Repeatable high-end PvE.'
        SocialStaff = ''
        Exploration = ''
        CraftEconomy = ''
        Progression = 'PvE reward progression.'
    }
    'Monster Nests' = @{
        Immediate = 'Localized exploration threat.'
        Medium = 'Clearable area objective.'
        Long = ''
        SocialStaff = ''
        Exploration = 'Primary nest exploration objective.'
        CraftEconomy = ''
        Progression = ''
    }
    'Clone Offline Player Characters' = @{
        Immediate = 'World persistence and ambient challenge.'
        Medium = 'Social-world continuity.'
        Long = ''
        SocialStaff = ''
        Exploration = 'Can affect world encounters.'
        CraftEconomy = ''
        Progression = ''
    }
    'Offline Skill Training' = @{
        Immediate = 'Convenience progression.'
        Medium = 'Skill advancement while away.'
        Long = 'Long-term character growth.'
        SocialStaff = ''
        Exploration = ''
        CraftEconomy = ''
        Progression = 'Progression without active play.'
    }
    'OmniAI' = @{
        Immediate = 'Combat behavior.'
        Medium = 'Encounter difficulty.'
        Long = ''
        SocialStaff = ''
        Exploration = ''
        CraftEconomy = ''
        Progression = ''
    }
    'AI Overhaul' = @{
        Immediate = 'Combat behavior.'
        Medium = 'Encounter difficulty.'
        Long = ''
        SocialStaff = ''
        Exploration = ''
        CraftEconomy = ''
        Progression = ''
    }
    'Staff Toolbar' = @{
        Immediate = 'Staff operational command surface.'
        Medium = 'Event support and moderation.'
        Long = ''
        SocialStaff = 'Staff-only workflow.'
        Exploration = ''
        CraftEconomy = ''
        Progression = ''
    }
    'Static Gump Tool' = @{
        Immediate = 'Staff-authored static presentation.'
        Medium = 'Event or content support.'
        Long = ''
        SocialStaff = 'Staff-only workflow.'
        Exploration = ''
        CraftEconomy = ''
        Progression = ''
    }
    'Spell Framework' = @{
        Immediate = 'Common magic registration.'
        Medium = 'Spell family expansion.'
        Long = 'Build identity infrastructure.'
        SocialStaff = ''
        Exploration = ''
        CraftEconomy = ''
        Progression = 'Magic progression support.'
    }
    'Magic Schools' = @{
        Immediate = 'Distinct spell choices.'
        Medium = 'Build identity.'
        Long = 'Long-term magic specialization.'
        SocialStaff = ''
        Exploration = ''
        CraftEconomy = ''
        Progression = 'Combat/magic progression.'
    }
    'Crafting Core' = @{
        Immediate = 'Craft item goals.'
        Medium = 'Recipe/material progression.'
        Long = 'Crafter identity.'
        SocialStaff = ''
        Exploration = ''
        CraftEconomy = 'Core crafting objective.'
        Progression = ''
    }
    'Harvest System' = @{
        Immediate = 'Gathering activity.'
        Medium = 'Resource planning.'
        Long = ''
        SocialStaff = ''
        Exploration = 'Resource location goals.'
        CraftEconomy = 'Crafting supply source.'
        Progression = ''
    }
    'Bulk Orders' = @{
        Immediate = 'Structured crafting task.'
        Medium = 'Reward and reputation pacing.'
        Long = ''
        SocialStaff = ''
        Exploration = ''
        CraftEconomy = 'Crafting objective/reward loop.'
        Progression = ''
    }
    'Gardening' = @{
        Immediate = 'Plant tending.'
        Medium = 'Crop/resource output.'
        Long = ''
        SocialStaff = ''
        Exploration = ''
        CraftEconomy = 'Resource and homestead support.'
        Progression = ''
    }
    'Housing' = @{
        Immediate = 'Place ownership.'
        Medium = 'Storage, display, civic anchor.'
        Long = 'Long-term social/home objective.'
        SocialStaff = ''
        Exploration = ''
        CraftEconomy = 'Supports economy and government goals.'
        Progression = ''
    }
    'Boats' = @{
        Immediate = 'Travel capability.'
        Medium = 'Route and exploration planning.'
        Long = ''
        SocialStaff = ''
        Exploration = 'Travel/exploration objective.'
        CraftEconomy = ''
        Progression = ''
    }
    'Regions' = @{
        Immediate = 'World rule boundary.'
        Medium = 'Policy and event gates.'
        Long = ''
        SocialStaff = 'Can encode staff/event zones.'
        Exploration = 'World area expectations.'
        CraftEconomy = ''
        Progression = ''
    }
    'PlayerMobile Core' = @{
        Immediate = 'Player state and lifecycle.'
        Medium = 'Cross-system persistence.'
        Long = 'Save-compatible player identity.'
        SocialStaff = ''
        Exploration = ''
        CraftEconomy = ''
        Progression = 'Core progression state.'
    }
    'Vendor Core' = @{
        Immediate = 'Buy/sell interaction.'
        Medium = 'Gold and goods flow.'
        Long = 'Economy stability.'
        SocialStaff = ''
        Exploration = ''
        CraftEconomy = 'Core economy outlet.'
        Progression = ''
    }
    'Obsolete Scripts' = @{
        Immediate = 'No intended player objective until reviewed.'
        Medium = ''
        Long = ''
        SocialStaff = 'Legacy hooks may affect staff workflows.'
        Exploration = ''
        CraftEconomy = ''
        Progression = ''
    }
}

$domainRows = New-Object System.Collections.ArrayList
foreach ($system in $systems)
{
    $domains = @($domainMap[$system])
    if ($domains.Count -eq 0)
    {
        $domains = @('Unknown')
    }

    [void]$domainRows.Add([pscustomobject]@{
        System = $system
        Domains = ($domains -join ';')
        Classification = ($cards | Where-Object { $_.System -eq $system } | Select-Object -First 1 -ExpandProperty Classification)
        ReviewGroup = if ($domains -contains 'Staff events') { 'Staff events' } elseif ($domains -contains 'PvP') { 'PvP' } elseif ($domains -contains 'PvE') { 'PvE' } elseif ($domains -contains 'Economy') { 'Economy' } else { $domains[0] }
        Evidence = 'Phase 9 domain bucket assigned from system card scope and root audit high-risk areas.'
    })
}

$staffSystems = @('XMLSpawner', 'Invasion', 'Staff Toolbar', 'Static Gump Tool')
$matrixRows = New-Object System.Collections.ArrayList

for ($i = 0; $i -lt $systems.Count; $i++)
{
    for ($j = $i + 1; $j -lt $systems.Count; $j++)
    {
        $a = $systems[$i]
        $b = $systems[$j]
        $key = Get-PairKey $a $b
        $edges = @()
        if ($pairEdges.ContainsKey($key))
        {
            $edges = @($pairEdges[$key])
        }

        $rules = @()
        if ($manualRules.ContainsKey($key))
        {
            $rules = @($manualRules[$key])
        }

        $labels = New-Object System.Collections.ArrayList
        $summaries = New-Object System.Collections.ArrayList
        $riskTypes = New-Object System.Collections.ArrayList
        $objectives = New-Object System.Collections.ArrayList
        $recommendations = New-Object System.Collections.ArrayList
        $evidenceItems = New-Object System.Collections.ArrayList

        $edgeTypes = @($edges | Select-Object -ExpandProperty EdgeType | Sort-Object -Unique)
        $hardCount = @($edges | Where-Object { $_.Strength -eq 'Hard' }).Count
        $softCount = @($edges | Where-Object { $_.Strength -eq 'Soft' }).Count
        $speculativeCount = @($edges | Where-Object { $_.Strength -eq 'Speculative' }).Count

        if ($hardCount -gt 0)
        {
            [void]$labels.Add('DependsOn')
            [void]$summaries.Add('Dependency graph has hard source/runtime/save/project/config edges between these systems.')
            [void]$riskTypes.Add('MaintenanceCoupling')
            [void]$objectives.Add('Shared runtime or persistence surface; player objective impact requires source review before behavior changes.')
            [void]$recommendations.Add('Review exact edges before moving files or changing behavior.')
        }

        if ($edgeTypes -contains 'DocumentationConflict')
        {
            [void]$labels.Add('DocRisk')
            [void]$labels.Add('Conflicts')
            [void]$riskTypes.Add('DocumentationRisk')
            [void]$recommendations.Add('Resolve source-trace and alias/canonical documentation conflicts.')
        }

        if ($edgeTypes -contains 'ProjectTruthConflict' -or $edgeTypes -contains 'SerializationConflict')
        {
            [void]$labels.Add('Conflicts')
            [void]$riskTypes.Add('CodeCorrectnessRisk')
            [void]$recommendations.Add('Resolve project truth or save-compatibility conflict before reorganization.')
        }

        if (($edgeTypes -contains 'DocsOnly' -or $edgeTypes -contains 'DocsSourceTrace') -and ($docRiskBySystem.ContainsKey($a) -or $docRiskBySystem.ContainsKey($b)))
        {
            [void]$labels.Add('DocRisk')
            [void]$riskTypes.Add('DocumentationRisk')
            [void]$recommendations.Add('Do not treat documentation-only links as source proof until source traces are repaired.')
        }

        foreach ($rule in $rules)
        {
            foreach ($label in ($rule.Labels -split ';'))
            {
                [void]$labels.Add($label)
            }
            [void]$summaries.Add($rule.Summary)
            [void]$riskTypes.Add($rule.RiskType)
            [void]$objectives.Add($rule.ObjectiveImpact)
            [void]$recommendations.Add($rule.Recommendation)
            [void]$evidenceItems.Add($rule.Evidence)
        }

        $docRiskCount = 0
        $docEvidence = New-Object System.Collections.ArrayList
        foreach ($system in @($a, $b))
        {
            if ($docRiskBySystem.ContainsKey($system))
            {
                $docRiskCount += @($docRiskBySystem[$system]).Count
                foreach ($risk in @($docRiskBySystem[$system] | Select-Object -First 2))
                {
                    [void]$docEvidence.Add(("{0}:{1}" -f $system, $risk.DocPath))
                }
            }
        }

        $needsReworkCount = 0
        $needsEvidence = New-Object System.Collections.ArrayList
        foreach ($system in @($a, $b))
        {
            if ($needsReworkBySystem.ContainsKey($system))
            {
                $needsReworkCount += @($needsReworkBySystem[$system]).Count
                foreach ($row in @($needsReworkBySystem[$system] | Select-Object -First 2))
                {
                    [void]$needsEvidence.Add(("{0}:{1}:L{2}" -f $system, $row.File, $row.Line))
                }
            }
        }

        if ($docRiskCount -gt 0 -and $rules.Count -gt 0)
        {
            [void]$labels.Add('DocRisk')
            [void]$riskTypes.Add('DocumentationRisk')
        }

        if ($edges.Count -gt 0)
        {
            foreach ($edge in @($edges | Select-Object -First 4))
            {
                [void]$evidenceItems.Add(("{0}->{1}:{2}:{3}" -f $edge.SourceSystem, $edge.TargetSystem, $edge.EdgeType, $edge.Evidence))
            }
        }

        foreach ($item in @($docEvidence | Select-Object -First 4))
        {
            [void]$evidenceItems.Add($item)
        }

        foreach ($item in @($needsEvidence | Select-Object -First 4))
        {
            [void]$evidenceItems.Add($item)
        }

        $staffDependency = $false
        if (@($labels | Where-Object { $_ -eq 'StaffDependency' }).Count -gt 0)
        {
            $staffDependency = $true
        }
        elseif ($edges.Count -gt 0 -and ($staffSystems -contains $a -or $staffSystems -contains $b))
        {
            [void]$labels.Add('StaffDependency')
            [void]$riskTypes.Add('StaffWorkflowRisk')
            $staffDependency = $true
        }

        if ($labels.Count -eq 0)
        {
            [void]$labels.Add('NoLink')
            [void]$summaries.Add('No generated dependency graph edge or Phase 9 manual review rule connects these systems.')
            [void]$riskTypes.Add('NoneGenerated')
            [void]$objectives.Add('No direct player objective relationship generated by this audit pass.')
            [void]$recommendations.Add('No action unless future source review finds a hidden dependency.')
            [void]$evidenceItems.Add('Negative evidence: no pair edge in dependency graph and no manual Phase 9 rule.')
        }

        $evidenceStrength = 'NoGeneratedEvidence'
        if ($hardCount -gt 0)
        {
            $evidenceStrength = 'SourceVerified'
        }
        elseif ($softCount -gt 0 -or $speculativeCount -gt 0)
        {
            $evidenceStrength = 'SoftOrSpeculative'
        }
        elseif ($rules.Count -gt 0)
        {
            $evidenceStrength = 'PhaseReviewSeed'
        }

        $hookCountA = 0
        $hookCountB = 0
        if ($hookCountBySystem.ContainsKey($a))
        {
            $hookCountA = $hookCountBySystem[$a]
        }
        if ($hookCountBySystem.ContainsKey($b))
        {
            $hookCountB = $hookCountBySystem[$b]
        }

        [void]$matrixRows.Add([pscustomobject]@{
            SystemA = $a
            SystemB = $b
            DomainsA = (@($domainMap[$a]) -join ';')
            DomainsB = (@($domainMap[$b]) -join ';')
            Labels = Join-Unique @($labels)
            RelationshipSummary = Trim-Text (Join-Unique @($summaries))
            EvidenceStrength = $evidenceStrength
            EdgeCount = $edges.Count
            HardEdgeCount = $hardCount
            SoftEdgeCount = $softCount
            SpeculativeEdgeCount = $speculativeCount
            GraphEdgeTypes = ($edgeTypes -join ';')
            RuntimeHookCount = ($hookCountA + $hookCountB)
            DocumentationRiskCount = $docRiskCount
            KnownNeedsReworkCount = $needsReworkCount
            RiskType = Join-Unique @($riskTypes)
            ObjectiveImpact = Trim-Text (Join-Unique @($objectives))
            StaffDependency = $staffDependency
            Recommendation = Trim-Text (Join-Unique @($recommendations))
            Evidence = Trim-Text (Join-Unique @($evidenceItems)) 500
            Verification = 'Generated from Phase 8 dependency graph, Phase 7 documentation truth table, Phase 5 runtime hook map, and explicit Phase 9 checklist rules.'
            ReviewStatus = if (@($labels | Where-Object { $_ -ne 'NoLink' }).Count -gt 0) { 'Reviewed' } else { 'NoGeneratedLink' }
        })
    }
}

$balanceRows = New-Object System.Collections.ArrayList
foreach ($row in @($matrixRows | Where-Object { $_.Labels -match 'BalanceRisk' }))
{
    [void]$balanceRows.Add([pscustomobject]@{
        RiskId = ('BAL-{0:0000}' -f ($balanceRows.Count + 1))
        Systems = ("{0} <-> {1}" -f $row.SystemA, $row.SystemB)
        Labels = $row.Labels
        RiskKind = $row.RiskType
        WhyBalanceNotCorrectness = 'This row flags pacing, reward, policy, or player-objective risk; it does not by itself prove a compiler, serializer, or runtime bug.'
        Recommendation = $row.Recommendation
        Evidence = $row.Evidence
        Verification = $row.Verification
    })
}

$staffRows = New-Object System.Collections.ArrayList
foreach ($row in @($matrixRows | Where-Object { $_.StaffDependency -eq $true }))
{
    [void]$staffRows.Add([pscustomobject]@{
        DependencyId = ('STAFF-{0:0000}' -f ($staffRows.Count + 1))
        Systems = ("{0} <-> {1}" -f $row.SystemA, $row.SystemB)
        Labels = $row.Labels
        RequiredStaffSurface = if ($row.SystemA -in $staffSystems) { $row.SystemA } elseif ($row.SystemB -in $staffSystems) { $row.SystemB } else { 'Policy or event override' }
        PlayerObjectiveImpact = $row.ObjectiveImpact
        Recommendation = $row.Recommendation
        Evidence = $row.Evidence
    })
}

$preservationRows = New-Object System.Collections.ArrayList
foreach ($row in @($matrixRows | Where-Object { $_.Labels -match 'Supports' }))
{
    [void]$preservationRows.Add([pscustomobject]@{
        NoteId = ('KEEP-{0:0000}' -f ($preservationRows.Count + 1))
        Systems = ("{0} <-> {1}" -f $row.SystemA, $row.SystemB)
        Synergy = $row.RelationshipSummary
        PreserveDuringReorg = $row.Recommendation
        Evidence = $row.Evidence
        Verification = $row.Verification
    })
}

$objectiveRows = New-Object System.Collections.ArrayList
foreach ($system in $systems)
{
    $objective = $objectiveMap[$system]
    if ($null -eq $objective)
    {
        $objective = @{
            Immediate = ''
            Medium = ''
            Long = ''
            SocialStaff = ''
            Exploration = ''
            CraftEconomy = ''
            Progression = ''
        }
    }

    [void]$objectiveRows.Add([pscustomobject]@{
        System = $system
        Domains = (@($domainMap[$system]) -join ';')
        ImmediateGoals = $objective.Immediate
        MediumTermGoals = $objective.Medium
        LongTermGoals = $objective.Long
        SocialOrStaffDrivenGoals = $objective.SocialStaff
        ExplorationGoals = $objective.Exploration
        CraftingEconomyGoals = $objective.CraftEconomy
        ProgressionGoals = $objective.Progression
        StaffDependencyVisible = if ($system -in $staffSystems) { 'Yes' } else { 'No' }
        Evidence = 'Phase 9 player objective review derived from system card scope, phase checklist examples, and domain bucket.'
    })
}

$matrixPath = Export-AuditCsv @($matrixRows) 'synergy-conflict-matrix.csv'
$phaseMatrixPath = Export-AuditCsv @($matrixRows) 'phase-09-synergy-conflict-matrix.csv'
$domainPath = Export-AuditCsv @($domainRows) 'phase-09-domain-buckets.csv'
$balancePath = Export-AuditCsv @($balanceRows) 'phase-09-balance-risk-list.csv'
$docRiskPath = Export-AuditCsv @($docRiskRows) 'phase-09-documentation-risk-list.csv'
$staffPath = Export-AuditCsv @($staffRows) 'phase-09-staff-dependency-list.csv'
$preservationPath = Export-AuditCsv @($preservationRows) 'phase-09-preservation-notes.csv'
$objectivePath = Export-AuditCsv @($objectiveRows) 'phase-09-player-objective-review.csv'

$labelCounts = @{}
foreach ($row in $matrixRows)
{
    foreach ($label in ($row.Labels -split ';'))
    {
        if (-not $labelCounts.ContainsKey($label))
        {
            $labelCounts[$label] = 0
        }
        $labelCounts[$label]++
    }
}

$summaryPath = Join-Path $OutputDir 'phase-09-summary.md'
$summary = New-Object System.Collections.ArrayList
[void]$summary.Add('# Phase 9 Synergy And Conflict Matrix Summary')
[void]$summary.Add('')
[void]$summary.Add(("Generated: {0}" -f (Get-Date -Format 'o')))
[void]$summary.Add('')
[void]$summary.Add('## Required Inputs')
[void]$summary.Add('')
[void]$summary.Add('| Input | Status |')
[void]$summary.Add('| --- | --- |')
[void]$summary.Add(('| Dependency Graph | Present: `dependency-graph.csv` with {0} rows |' -f $graph.Count))
[void]$summary.Add(('| System Cards | Present: `phase-04-system-card-index.csv` with {0} rows |' -f $cards.Count))
[void]$summary.Add(('| Documentation Truth Table | Present: `documentation-truth-table.csv` with {0} rows |' -f $docTruth.Count))
[void]$summary.Add(('| Runtime Hook Map | Present: `runtime-hook-map.csv` with {0} rows |' -f $hooks.Count))
[void]$summary.Add(("| Known Needs Rework Rows | Present: generated from docs markdown scan with {0} rows |" -f $needsReworkRows.Count))
[void]$summary.Add('| Player-Facing Progression Docs | Present through documentation truth table and docs markdown scan; source-trace gaps are listed as documentation risks. |')
[void]$summary.Add('')
[void]$summary.Add('## Generated Outputs')
[void]$summary.Add('')
[void]$summary.Add('| Output | Rows | Purpose |')
[void]$summary.Add('| --- | ---: | --- |')
[void]$summary.Add(('| `synergy-conflict-matrix.csv` | {0} | Canonical pairwise matrix for system-card systems. |' -f $matrixRows.Count))
[void]$summary.Add(('| `phase-09-synergy-conflict-matrix.csv` | {0} | Phase-scoped copy of the pairwise matrix. |' -f $matrixRows.Count))
[void]$summary.Add(('| `phase-09-domain-buckets.csv` | {0} | Domain grouping for Phase 9 review. |' -f $domainRows.Count))
[void]$summary.Add(('| `phase-09-balance-risk-list.csv` | {0} | Gameplay, pacing, reward, and policy balance risks separated from code correctness. |' -f $balanceRows.Count))
[void]$summary.Add(('| `phase-09-documentation-risk-list.csv` | {0} | Source-trace, stale-claim, alias, and missing-path documentation risks. |' -f $docRiskRows.Count))
[void]$summary.Add(('| `phase-09-staff-dependency-list.csv` | {0} | Relationships requiring staff tooling, staff events, or event override policy. |' -f $staffRows.Count))
[void]$summary.Add(('| `phase-09-preservation-notes.csv` | {0} | Positive synergies to preserve during later cleanup. |' -f $preservationRows.Count))
[void]$summary.Add(('| `phase-09-player-objective-review.csv` | {0} | Immediate, medium-term, long-term, staff, exploration, crafting/economy, and progression goals by system. |' -f $objectiveRows.Count))
[void]$summary.Add('')
[void]$summary.Add('## Matrix Label Counts')
[void]$summary.Add('')
[void]$summary.Add('| Label | Count |')
[void]$summary.Add('| --- | ---: |')
foreach ($label in @($labelCounts.Keys | Sort-Object))
{
    [void]$summary.Add(("| {0} | {1} |" -f $label, $labelCounts[$label]))
}
[void]$summary.Add('')
[void]$summary.Add('## Required Spot Checks')
[void]$summary.Add('')
[void]$summary.Add('| Pair | Expected Review | Generated Labels |')
[void]$summary.Add('| --- | --- | --- |')
$spotPairs = @(
    @('Character Level', 'Random Encounters', 'Character Level making Random Encounters objective-aware.'),
    @('PvP Consent', 'Government', 'Consent versus government wars/bans/civic enforcement.'),
    @('PvP Consent', 'XMLSpawner', 'XMLPoints or staff event overrides versus consent policy.'),
    @('Spell Framework', 'Magic Schools', 'Spell registry capacity versus high-ID magic families.'),
    @('Homestead', 'Crafting Core', 'Crafting/resource loops versus progression pacing.'),
    @('XMLSpawner', 'Invasion', 'XMLSpawner supporting staff events and quests.')
)
foreach ($spotPair in $spotPairs)
{
    $row = $matrixRows | Where-Object {
        ($_.SystemA -eq $spotPair[0] -and $_.SystemB -eq $spotPair[1]) -or
        ($_.SystemA -eq $spotPair[1] -and $_.SystemB -eq $spotPair[0])
    } | Select-Object -First 1
    [void]$summary.Add(("| {0} / {1} | {2} | {3} |" -f $spotPair[0], $spotPair[1], $spotPair[2], $row.Labels))
}
[void]$summary.Add('')
[void]$summary.Add('## Exit Criteria')
[void]$summary.Add('')
[void]$summary.Add('- Pairwise labels have evidence from the dependency graph, documentation truth table, runtime hook map, or explicit Phase 9 checklist rules.')
[void]$summary.Add('- Positive synergies are recorded in preservation notes.')
[void]$summary.Add('- Conflicts and balance risks are listed separately from compiler, serializer, or runtime correctness issues.')
[void]$summary.Add('- Staff dependencies and documentation risks are visible as focused registers.')
[void]$summary.Add('- Player objective review records immediate, medium-term, long-term, staff, exploration, crafting/economy, and progression goals for every system-card system.')

[System.IO.File]::WriteAllLines($summaryPath, [string[]]$summary, (New-Object System.Text.UTF8Encoding($false)))

[pscustomobject]@{
    Matrix = Convert-ToRepoPath $matrixPath
    PhaseMatrix = Convert-ToRepoPath $phaseMatrixPath
    DomainBuckets = Convert-ToRepoPath $domainPath
    BalanceRisks = Convert-ToRepoPath $balancePath
    DocumentationRisks = Convert-ToRepoPath $docRiskPath
    StaffDependencies = Convert-ToRepoPath $staffPath
    PreservationNotes = Convert-ToRepoPath $preservationPath
    PlayerObjectives = Convert-ToRepoPath $objectivePath
    Summary = Convert-ToRepoPath $summaryPath
    MatrixRows = $matrixRows.Count
    BalanceRiskRows = $balanceRows.Count
    DocumentationRiskRows = $docRiskRows.Count
    StaffDependencyRows = $staffRows.Count
    PreservationRows = $preservationRows.Count
}
