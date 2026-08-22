param(
    [string]$RepoRoot,
    [string]$OutputDir,
    [string]$BaselineCommit = 'cbd03db1',
    [switch]$ValidateOnly
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

$auditFileName = 'player-facing-system-documentation-audit.csv'
$backlogFileName = 'in-game-documentation-backlog.csv'
$styleGuideFileName = 'in-game-documentation-style-guide.md'
$summaryFileName = 'in-game-documentation-audit-summary.md'

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

function Convert-ToFullPath
{
    param([string]$RepoPath)

    return [System.IO.Path]::GetFullPath((Join-Path $RepoRoot ($RepoPath -replace '/', '\')))
}

function Test-GeneratedPath
{
    param([string]$Path)

    return $Path -match '(^|/)(bin|obj)(/|$)'
}

function Join-Unique
{
    param([object[]]$Values)

    return (($Values | ForEach-Object { ([string]$_).Trim() } | Where-Object { -not [string]::IsNullOrWhiteSpace($_) } | Sort-Object -Unique) -join ';')
}

function Convert-ToSlug
{
    param([string]$Value)

    $slug = (($Value.ToLowerInvariant() -replace '[^a-z0-9]+', '-') -replace '(^-|-$)', '')

    if ($slug.Length -gt 64)
    {
        $slug = $slug.Substring(0, 64).TrimEnd('-')
    }

    return $slug
}

function Invoke-Git
{
    param([string[]]$Arguments)

    $output = & git -C $RepoRoot @Arguments 2>&1

    if ($LASTEXITCODE -ne 0)
    {
        throw "git $($Arguments -join ' ') failed: $($output -join [Environment]::NewLine)"
    }

    return @($output | ForEach-Object { [string]$_ })
}

function ConvertTo-DeterministicCsv
{
    param([object[]]$Rows)

    $lines = @($Rows | ConvertTo-Csv -NoTypeInformation)
    return (($lines -join "`n") + "`n")
}

function Write-Utf8Lf
{
    param(
        [string]$Path,
        [string]$Content
    )

    $normalized = ($Content -replace "`r`n", "`n") -replace "`r", "`n"

    if (-not $normalized.EndsWith("`n"))
    {
        $normalized += "`n"
    }

    [System.IO.File]::WriteAllText($Path, $normalized, (New-Object System.Text.UTF8Encoding($false)))
}

function Get-SourceSystem
{
    param(
        [string]$Path,
        [hashtable]$KnownSystemByPath
    )

    if ($KnownSystemByPath.ContainsKey($Path))
    {
        return $KnownSystemByPath[$Path]
    }

    $segments = $Path -split '/'

    if ($Path -like 'Data/System/Source/*')
    {
        return 'ServerCore'
    }

    if ($Path -like 'Data/Scripts/Custom/*' -and $segments.Count -ge 4)
    {
        return 'Custom:' + $segments[3]
    }

    if ($Path -like 'Data/Scripts/*' -and $segments.Count -ge 4)
    {
        return $segments[2] + ':' + $segments[3]
    }

    return 'Unclassified'
}

function Get-CurrentSourceFiles
{
    $roots = @(
        (Join-Path $RepoRoot 'Data\Scripts'),
        (Join-Path $RepoRoot 'Data\System\Source')
    )

    return @(
        Get-ChildItem -LiteralPath $roots -Recurse -File -Filter '*.cs' |
            ForEach-Object { Convert-ToRepoPath $_.FullName } |
            Where-Object { -not (Test-GeneratedPath $_) } |
            Sort-Object -Unique
    )
}

function Get-PlayerCommands
{
    param(
        [string[]]$SourceFiles,
        [hashtable]$SystemByPath
    )

    $commands = New-Object System.Collections.Generic.List[object]
    $pattern = 'CommandSystem\s*\.\s*Register\s*\(\s*"(?<Name>[^"]+)"\s*,\s*AccessLevel\.(?<Access>[A-Za-z]+)'

    foreach ($path in $SourceFiles)
    {
        $content = [System.IO.File]::ReadAllText((Convert-ToFullPath $path))

        foreach ($match in [regex]::Matches($content, $pattern, [System.Text.RegularExpressions.RegexOptions]::IgnoreCase))
        {
            if ($match.Groups['Access'].Value -ne 'Player')
            {
                continue
            }

            $commands.Add([pscustomobject]@{
                Name = $match.Groups['Name'].Value
                Access = 'Player'
                Path = $path
                System = $SystemByPath[$path]
            }) | Out-Null
        }
    }

    return @($commands.ToArray() | Sort-Object Name,Path -Unique)
}

function Get-CurrentCanonicalDocs
{
    param([string]$DocumentationTablePath)

    $seedByPath = @{}
    if (Test-Path -LiteralPath $DocumentationTablePath -PathType Leaf)
    {
        foreach ($row in Import-Csv -LiteralPath $DocumentationTablePath)
        {
            if ($row.CanonicalPage -eq 'Canonical')
            {
                $seedByPath[$row.DocPath] = $row
            }
        }
    }

    $indexPath = 'docs/wiki/INDEX.md'
    $category = ''
    $docs = New-Object System.Collections.Generic.List[object]

    foreach ($line in Get-Content -LiteralPath (Convert-ToFullPath $indexPath))
    {
        if ($line -match '^##\s+(?<Category>.+?)\s*$')
        {
            $category = $Matches['Category'].Trim()
            continue
        }

        if ($line -notmatch '^-\s+\[(?<Title>[^\]]+)\]\((?<Target>[^)#]+)(?:#[^)]+)?\)')
        {
            continue
        }

        $title = $Matches['Title'].Trim()
        if ($title -match '\(legacy slug\)')
        {
            continue
        }

        $target = [System.Uri]::UnescapeDataString($Matches['Target'].Trim())
        $docPath = 'docs/wiki/' + $target
        $verifiedSourceFiles = ''

        if ($seedByPath.ContainsKey($docPath))
        {
            $verifiedSourceFiles = $seedByPath[$docPath].VerifiedSourceFiles
        }
        elseif (Test-Path -LiteralPath (Convert-ToFullPath $docPath) -PathType Leaf)
        {
            $content = [System.IO.File]::ReadAllText((Convert-ToFullPath $docPath))
            $foundPaths = New-Object System.Collections.Generic.List[string]
            foreach ($match in [regex]::Matches($content, '(?<Path>(?:Data/(?:Scripts|System/Source|TurnBasedCombat)|Info)/[^`\s|,)]+(?:\s[^`|,)]+?\.cs|\.cs|\.xml|\.cfg|\.md)?)'))
            {
                $candidate = $match.Groups['Path'].Value.Trim().TrimEnd('.', ',', ';', ':')
                if (Test-Path -LiteralPath (Convert-ToFullPath $candidate))
                {
                    $foundPaths.Add($candidate) | Out-Null
                }
            }
            $verifiedSourceFiles = Join-Unique $foundPaths.ToArray()
        }

        $docs.Add([pscustomobject]@{
            DocPath = $docPath
            Title = $title
            IndexCategory = $category
            VerifiedSourceFiles = $verifiedSourceFiles
        }) | Out-Null
    }

    return @($docs.ToArray() | Sort-Object DocPath)
}

function Get-History
{
    param(
        [string]$ResolvedBaseline,
        [string]$AuditTarget,
        [hashtable]$SystemByPath
    )

    $historyBySystem = @{}
    $changedCurrentPaths = New-Object System.Collections.Generic.HashSet[string]([System.StringComparer]::OrdinalIgnoreCase)
    $historicalPaths = New-Object System.Collections.Generic.List[string]
    $currentCommit = ''

    $arguments = @(
        'log', '--date=short', '--format=@@@%H%x09%ad%x09%s', '--name-only',
        "$ResolvedBaseline..$AuditTarget", '--',
        'Data/Scripts', 'Data/System/Source', 'Data/Spawns', 'Data/XMLSpawn', 'Data/Decoration',
        'Info/settings.xml', 'Data/TurnBasedCombat/TurnBasedCombat.cfg'
    )

    foreach ($line in Invoke-Git $arguments)
    {
        if ($line.StartsWith('@@@'))
        {
            $currentCommit = $line.Substring(3).Trim()
            continue
        }

        $path = ($line.Trim() -replace '\\', '/')

        if ([string]::IsNullOrWhiteSpace($path) -or [string]::IsNullOrWhiteSpace($currentCommit))
        {
            continue
        }

        if ($SystemByPath.ContainsKey($path))
        {
            $system = $SystemByPath[$path]
            $changedCurrentPaths.Add($path) | Out-Null

            if (-not $historyBySystem.ContainsKey($system))
            {
                $historyBySystem[$system] = New-Object System.Collections.Generic.HashSet[string]
            }

            $historyBySystem[$system].Add($currentCommit) | Out-Null
        }
        else
        {
            $historicalPaths.Add($path) | Out-Null
        }
    }

    return [pscustomobject]@{
        BySystem = $historyBySystem
        ChangedCurrentPaths = @($changedCurrentPaths | Sort-Object)
        HistoricalPaths = @($historicalPaths.ToArray() | Sort-Object -Unique)
    }
}

function Get-HistorySummary
{
    param(
        [string]$System,
        [hashtable]$HistoryBySystem
    )

    if (-not $HistoryBySystem.ContainsKey($System))
    {
        return 'No post-baseline change found in current files for this bucket.'
    }

    $commits = @($HistoryBySystem[$System] | Sort-Object)
    $examples = @($commits | Select-Object -Last 3 | ForEach-Object {
        $parts = $_ -split "`t", 3
        if ($parts.Count -eq 3)
        {
            return ($parts[0].Substring(0, 8) + ' ' + $parts[1] + ' ' + $parts[2])
        }
        return $_
    })

    return "$($commits.Count) post-baseline commit(s); latest examples: $($examples -join ' | ')"
}

function Get-SystemProfile
{
    param(
        [string]$System,
        [string[]]$Commands
    )

    $audience = if ($Commands.Count -gt 0) { 'Player' } else { 'Mixed' }
    $activation = 'ActiveBySource'
    $entry = 'Varies by item use, speech, movement, combat, crafting, quest, vendor, region, or command.'
    $placement = 'Source-defined capability family; current source does not prove live-world placement.'
    $surfaces = 'Context-sensitive labels, messages, journals, books, or gumps vary within the family.'
    $reachability = 'RuntimeUnknown'
    $coverage = 'Partial'
    $decision = 'NoDocumentationNeeded'
    $surface = 'ContextualFeedback'
    $priority = 'P4'
    $risk = 'Low: ordinary variants are learned through their shared mechanic or direct feedback.'
    $brief = 'Keep variants grouped under their shared mechanic. Add a separate capability row only when controls, rules, cost, persistence, or discovery warrant it.'
    $spoiler = 'Do not enumerate ordinary content variants or reveal locations and solutions.'
    $test = 'Confirm each distinct mechanic is represented by a detailed capability or canonical-topic row; sample ordinary variants for self-explanatory feedback.'
    $notes = 'Boundary census row. Detailed rows carry incorporation work.'

    if ($System -eq 'ServerCore' -or $System -match '(AIOverhaul|OmniAI|Obsolete|Static Gump|XMLSpawner|Staff|Access Level|Pandora|Gump Studio|Diagnostics|Core$)')
    {
        $audience = 'InternalOrStaff'
        $entry = 'No intended direct player entry path, except separately audited shared framework surfaces.'
        $coverage = 'NotApplicable'
        $decision = 'NoDocumentationNeeded'
        $risk = 'None for player documentation: internal infrastructure, staff tooling, or framework behavior.'
        $brief = 'Exclude internal implementation and staff controls from player documentation; retain separate rows for any real player-facing capability built on the framework.'
        $notes = 'Explicit exclusion: staff-only or internal infrastructure.'
    }
    elseif ($System -like 'Magic:*')
    {
        $audience = 'Player'
        $entry = 'Learn or acquire the school, invoke spells or abilities, and use the school-specific spellbook or commands.'
        $coverage = 'Partial'
        $decision = 'Incorporate'
        $surface = 'LibraryReference'
        $priority = 'P3'
        $risk = 'Medium: substantial combat progression can be discoverable while rules, costs, and controls remain hard to reconstruct.'
        $brief = 'Provide one concise school reference covering access, controls, resource costs, limitations, and safe experimentation without listing secrets.'
        $spoiler = 'Explain the school and controls; omit hidden teachers, rare acquisition locations, and puzzle solutions.'
        $test = 'A player who has encountered the school can find a persistent in-game reference and identify how to use it safely.'
    }
    elseif ($System -like 'Trades:*')
    {
        $audience = 'Player'
        $entry = 'Use a trade tool, resource, work surface, harvesting target, or crafting gump.'
        $coverage = 'Partial'
        $decision = 'Incorporate'
        $surface = 'LibraryReference'
        $priority = 'P3'
        $risk = 'Medium: progression, materials, queues, and expensive craft decisions can be unclear.'
        $brief = 'Teach the shared trade loop, required tools, risk or consumption points, and where contextual help appears.'
        $spoiler = 'Do not publish rare recipes or hidden resource locations unless the existing system already teaches them.'
        $test = 'A new user can start, stop, and safely repeat the shared trade loop using in-game information.'
    }
    elseif ($System -like 'Quests:*')
    {
        $audience = 'Player'
        $entry = 'Speak to a quest giver, use a quest item, or follow quest-journal feedback.'
        $coverage = 'IntentionalDiscovery'
        $decision = 'KeepAsIs'
        $surface = 'QuestJournal'
        $priority = 'P4'
        $risk = 'Low when the journal states objectives and failure or consumption risks before commitment.'
        $brief = 'Preserve quest-led discovery; document only controls, irreversible choices, and failure risks that the journal does not make clear.'
        $spoiler = 'Keep locations, answers, passwords, and surprises in journals, books, and NPC clues.'
        $test = 'Sample quest start, progress, failure, and completion states for a clear next clue without revealing the solution.'
    }
    elseif ($System -like 'Items:*' -or $System -like 'Mobiles:*')
    {
        $audience = 'Player'
        $coverage = 'IntentionalDiscovery'
        $decision = 'NoDocumentationNeeded'
        $priority = 'P4'
        $risk = 'Low for ordinary item or creature variants governed by a documented shared mechanic.'
        $brief = 'Treat this content family as grouped variants. Document only its unique, costly, persistent, destructive, authority, or undiscoverable interactions separately.'
        $notes = 'Explicit exclusion for ordinary content variants; unique mechanics are separate rows.'
    }
    elseif ($System -like 'Custom:*')
    {
        $audience = 'PlayerOrMixed'
        $coverage = 'Partial'
        $decision = 'Incorporate'
        $surface = 'LibraryReference'
        $priority = 'P3'
        $risk = 'Medium until the custom system entry path, cost, persistence, and failure modes are checked against an in-game reference.'
        $brief = 'Add or refresh an in-game overview when this custom bucket exposes a distinct player mechanic; otherwise record an explicit exclusion in the detailed review.'
        $test = 'Verify the detailed row or exclusion for each player-facing custom capability and stage-test its teaching surface.'
    }

    if ($System -eq 'Custom:Government System')
    {
        $coverage = 'Partial'; $decision = 'RefreshExisting'; $surface = 'SystemHelpGump'; $priority = 'P1'
        $risk = 'High: founding, taxes, elections, authority, treasury, bans, wars, and civic purchases can create persistent loss or obligations.'
        $brief = 'Refresh the government help gump around the current founding, treasury, maintenance, election, authority, ban, war, and disband rules.'
        $test = 'On staging, exercise every help page from an ordinary player and verify current costs, permissions, persistence, and destructive confirmations.'
    }
    elseif ($System -match '(Homestead|PvP Consent|Offline.*Training)')
    {
        $coverage = 'Missing'; $decision = 'Incorporate'; $surface = 'Help'; $priority = 'P1'
        $risk = 'High: the system changes persistent progression, property, combat consent, or unattended character behavior.'
    }
    elseif ($System -match '(Progression|CharacterLevel)')
    {
        $coverage = 'Partial'; $decision = 'Incorporate'; $surface = 'LibraryReference'; $priority = 'P1'
        $risk = 'High: persistent character progression and service commands need an authoritative in-game explanation.'
    }
    elseif ($System -match '(RandomEncounters|MonsterNests|Invasion|Champions)')
    {
        $coverage = 'IntentionalDiscovery'; $decision = 'RefreshExisting'; $surface = 'LoreBook'; $priority = 'P3'
        $risk = 'Medium: danger and cleanup behavior should be foreshadowed while locations, rewards, and encounter solutions remain discoveries.'
        $spoiler = 'Teach warning signs, danger, persistence, and clue paths; hide spawn locations, reward tables, and solutions.'
    }

    if ($Commands.Count -gt 0 -and $decision -eq 'NoDocumentationNeeded')
    {
        $audience = 'PlayerOrMixed'
        $coverage = 'Partial'
        $decision = 'RefreshExisting'
        $surface = 'Help'
        $priority = 'P2'
        $risk = 'Medium: a direct Player-access command is undiscoverable unless listed or taught at its point of need.'
        $brief = 'Reconcile the Player-access commands in this bucket with the Help command list and system-specific teaching.'
        $test = 'An ordinary player can discover each command, syntax, prerequisites, and safe outcome entirely in game.'
        $notes = 'Boundary row promoted because current source registers Player-access commands.'
    }

    return [pscustomobject]@{
        Audience = $audience
        ActivationState = $activation
        EntryPath = $entry
        PlacementEvidence = $placement
        Surfaces = $surfaces
        Reachability = $reachability
        Coverage = $coverage
        Decision = $decision
        Surface = $surface
        Priority = $priority
        Risk = $risk
        Brief = $brief
        Spoiler = $spoiler
        Test = $test
        Notes = $notes
    }
}

function Get-CanonicalTopicProfile
{
    param([object]$Doc)

    $category = $Doc.IndexCategory
    $fileName = [System.IO.Path]::GetFileNameWithoutExtension($Doc.DocPath)
    $coverage = 'Partial'
    $decision = 'Incorporate'
    $surface = 'LibraryReference'
    $priority = 'P3'
    $risk = 'Medium: the external reference describes a substantial player capability that is not yet proven reachable in game.'
    $brief = "Adapt the source-verified essentials from $($Doc.Title) into the shard's established in-game teaching surfaces."
    $spoiler = 'Explain entry, controls, rules, risks, and clue paths; omit locations, solutions, passwords, and surprises.'
    $test = 'An ordinary player can reach the recommended surface and perform the documented capability without using the external wiki.'
    $notes = 'The wiki is research evidence only and does not count as in-game coverage.'

    if ($category -in @('Staff And Administration', 'Technical And Engine Reference'))
    {
        $coverage = 'NotApplicable'; $decision = 'NoDocumentationNeeded'; $surface = 'ContextualFeedback'; $priority = 'P4'
        $risk = 'None for player documentation: the canonical topic is staff-only or implementation-facing.'
        $brief = 'Exclude this staff or technical reference from the player documentation backlog; audit any separate player-visible behavior under its own capability row.'
        $spoiler = 'Not applicable.'
        $test = 'Confirm the topic exposes no intended Player-access entry path; if one exists, create a separate player capability row.'
        $notes = 'Explicit exclusion: staff-only or technical documentation.'
    }
    elseif ($category -eq 'Start Here')
    {
        $coverage = 'Stale'; $decision = 'RefreshExisting'; $surface = 'Help'; $priority = 'P1'
        $risk = 'High: onboarding errors affect every new player and can hide basic controls or point to unavailable acquisition paths.'
        $brief = 'Reconcile onboarding text, command syntax, guide-book acquisition, Help navigation, and Library discovery with current source and staging placement.'
        $test = 'A fresh ordinary character can complete the start-here path using only reachable in-game surfaces.'
    }
    elseif ($category -eq 'Player Commands And Account Tools')
    {
        $coverage = 'Missing'; $decision = 'Incorporate'; $surface = 'Help'; $priority = 'P2'
        $risk = 'Medium to high: commands and account controls are otherwise undiscoverable and may alter persistent settings or behavior.'
        $brief = 'Add command name, syntax, prerequisites, persistent effects, stop or undo path, and failure feedback to Help or the owning system help.'
        $spoiler = 'Command documentation is not a spoiler; omit only hidden content revealed by its output.'
    }
    elseif ($category -eq 'Crafting, Harvesting, Trades, And Economy')
    {
        $coverage = 'Partial'; $decision = 'Incorporate'; $surface = 'LibraryReference'; $priority = 'P2'
        $risk = 'Medium to high: materials, property, batch actions, and vendor transactions can cause meaningful loss.'
        $brief = 'Teach entry tools, material consumption, batch or queue controls, persistent selections, stop paths, and irreversible or costly actions.'
    }
    elseif ($category -eq 'World And Gameplay Systems')
    {
        $coverage = 'IntentionalDiscovery'; $decision = 'RefreshExisting'; $surface = 'LoreBook'; $priority = 'P3'
        $risk = 'Medium: the system should be discoverable through clues while its danger, commitments, and controls remain clear.'
        $brief = 'Use lore, NPC hints, journals, or contextual warnings to teach the clue path and meaningful risks without publishing the solution.'
    }

    $completeTopics = @(
        'Animal_Trainer_Stable_Claim', 'Auto_Sheath', 'Banker_Speech_Commands', 'Help_Request_System',
        'Magic_Toolbars_Guide', 'Magery_Spell_Color_Setting', 'NPC_Mage_Advice', 'NPC_Shipwright_Sailing_Guide',
        'Ranger_Survival_Training', 'Real_Estate_Broker_Appraisal', 'Skill_Lists'
    )

    $systemHelpTopics = @('Apiculture_Beekeeping', 'Gardening_System')
    $staleTopics = @('Guide_To_Adventure_Book', 'World_Basics_Commands', 'In_Game_Command_List', 'Government_System', 'Player_Government_System_Guide')
    $intentionalTopics = @('Champion_Spawns', 'Goliath_Monsters', 'Monster_Nest_System', 'Random_Encounter_Engine', 'The_One_Ring')

    if ($fileName -eq 'Turn_Based_Combat')
    {
        $coverage = 'NotApplicable'; $decision = 'NoDocumentationNeeded'; $surface = 'Help'; $priority = 'P4'
        $risk = 'Low while the checked-in configuration remains disabled; advertising unavailable controls would create confusion.'
        $brief = 'Do not incorporate player instructions while disabled. Require a fresh source, configuration, and staging documentation pass before enabling.'
        $spoiler = 'Not applicable.'
        $test = 'Confirm the checked-in gate is disabled and no reachable in-game surface advertises unavailable controls.'
        $notes = 'Explicit exclusion: disabled by checked-in configuration.'
        return [pscustomobject]@{ Coverage = $coverage; Decision = $decision; Surface = $surface; Priority = $priority; Risk = $risk; Brief = $brief; Spoiler = $spoiler; Test = $test; Notes = $notes }
    }

    if ($fileName -eq 'PvP_Consent_System')
    {
        $coverage = 'Missing'; $decision = 'Incorporate'; $surface = 'Help'; $priority = 'P1'
        $risk = 'High: combat consent, harmful and beneficial eligibility, persistence, region exceptions, and event gates can cause meaningful loss or conflict.'
        $brief = 'Add an authoritative in-game rules page covering opt-in or opt-out controls, persistence, restrictions, region and event behavior, feedback, and recovery.'
        $spoiler = 'PvP authority and consent rules are not secrets; omit staff-only controls.'
        $test = 'Use two ordinary staging accounts through hostile, beneficial, region, event, logout, save/restart, and invalid-state paths.'
        $notes = 'Required incorporation: PvP and authority rules must be reachable in game.'
        return [pscustomobject]@{ Coverage = $coverage; Decision = $decision; Surface = $surface; Priority = $priority; Risk = $risk; Brief = $brief; Spoiler = $spoiler; Test = $test; Notes = $notes }
    }

    if ($fileName -in $completeTopics)
    {
        $coverage = 'Complete'; $decision = 'KeepAsIs'; $priority = 'P4'
        $surface = if ($fileName -match 'NPC|Banker|Trainer|Broker|Ranger') { 'NPCSpeech' } elseif ($fileName -eq 'Skill_Lists') { 'LibraryReference' } else { 'Help' }
        $risk = 'Low: current source defines an established reachable teaching surface; staging still confirms placement and wording.'
        $brief = 'Retain the established in-game teaching and recheck it when the owning mechanic changes.'
        $test = 'Reach the existing surface as an ordinary player and confirm it still matches current behavior.'
    }
    elseif ($fileName -in $systemHelpTopics)
    {
        $coverage = 'Complete'; $decision = 'KeepAsIs'; $surface = 'SystemHelpGump'; $priority = 'P4'
        $risk = 'Low: the system exposes dedicated source-defined help at the point of use.'
        $brief = 'Keep the dedicated help gump synchronized with later mechanics changes.'
        $test = 'Open the system help from its normal entry path and verify every page and navigation control.'
    }
    elseif ($fileName -in $staleTopics)
    {
        $coverage = 'Stale'; $decision = 'RefreshExisting'; $priority = 'P1'
        $surface = if ($fileName -match 'Government') { 'SystemHelpGump' } else { 'Help' }
        $risk = 'High: an established authoritative surface can actively mislead players when its acquisition, commands, costs, or rules drift.'
        $brief = 'Source-review and refresh the existing in-game surface, preserving its established visual and narrative conventions.'
        $test = 'Exercise the entire entry and navigation path on staging and compare each behavior claim with current source and configuration.'
    }
    elseif ($fileName -in $intentionalTopics)
    {
        $coverage = 'IntentionalDiscovery'; $decision = 'KeepAsIs'; $surface = 'LoreBook'; $priority = 'P4'
        $risk = 'Low if current clues and contextual warnings remain reachable.'
        $brief = 'Preserve discovery-led teaching; add only non-spoiling safety or control clarification if staging finds a dead-end.'
        $test = 'Follow the clue path as an ordinary player without outside knowledge and confirm the next step and danger are legible.'
    }

    return [pscustomobject]@{
        Coverage = $coverage
        Decision = $decision
        Surface = $surface
        Priority = $priority
        Risk = $risk
        Brief = $brief
        Spoiler = $spoiler
        Test = $test
        Notes = $notes
    }
}

function New-AuditRow
{
    param(
        [string]$Id,
        [string]$RowKind,
        [string]$System,
        [string]$Capability,
        [string]$Audience,
        [string]$ActivationState,
        [string]$PlayerEntryPath,
        [string]$AcquisitionPlacementEvidence,
        [string]$PlayerCommands,
        [string]$SourceFiles,
        [string]$PostBaselineChanges,
        [string]$ExistingInGameSurfaces,
        [string]$Reachability,
        [string]$Coverage,
        [string]$Decision,
        [string]$RecommendedSurface,
        [string]$Priority,
        [string]$PlayerRisk,
        [string]$SourceEvidence,
        [string]$ContentBrief,
        [string]$SpoilerBoundary,
        [string]$AcceptanceTest,
        [string]$ExternalDocs,
        [string]$Notes
    )

    return [pscustomobject][ordered]@{
        Id = $Id
        RowKind = $RowKind
        System = $System
        Capability = $Capability
        Audience = $Audience
        ActivationState = $ActivationState
        PlayerEntryPath = $PlayerEntryPath
        AcquisitionPlacementEvidence = $AcquisitionPlacementEvidence
        PlayerCommands = $PlayerCommands
        SourceFiles = $SourceFiles
        PostBaselineChanges = $PostBaselineChanges
        ExistingInGameSurfaces = $ExistingInGameSurfaces
        Reachability = $Reachability
        Coverage = $Coverage
        Decision = $Decision
        RecommendedSurface = $RecommendedSurface
        Priority = $Priority
        PlayerRisk = $PlayerRisk
        SourceEvidence = $SourceEvidence
        ContentBrief = $ContentBrief
        SpoilerBoundary = $SpoilerBoundary
        AcceptanceTest = $AcceptanceTest
        ExternalDocs = $ExternalDocs
        Notes = $Notes
    }
}

function Add-CuratedCapability
{
    param(
        [System.Collections.Generic.List[object]]$Rows,
        [hashtable]$HistoryBySystem,
        [string]$Slug,
        [string]$System,
        [string]$Capability,
        [string]$Audience,
        [string]$ActivationState,
        [string]$EntryPath,
        [string]$PlacementEvidence,
        [string]$Commands,
        [string[]]$Files,
        [string]$Surfaces,
        [string]$Reachability,
        [string]$Coverage,
        [string]$Decision,
        [string]$Surface,
        [string]$Priority,
        [string]$Risk,
        [string]$Brief,
        [string]$Spoiler,
        [string]$Test,
        [string]$ExternalDocs = '',
        [string]$Notes = ''
    )

    $existingFiles = @($Files | Where-Object { Test-Path -LiteralPath (Convert-ToFullPath $_) })
    $sourceFiles = Join-Unique $Files
    $evidence = if ($existingFiles.Count -eq $Files.Count) { 'Current filesystem and named symbols reviewed by the audit generator.' } else { 'One or more planned evidence paths are absent; validation will fail.' }

    $Rows.Add((New-AuditRow -Id ('IGD-CAP-' + (Convert-ToSlug $Slug)) -RowKind 'Capability' -System $System -Capability $Capability -Audience $Audience -ActivationState $ActivationState -PlayerEntryPath $EntryPath -AcquisitionPlacementEvidence $PlacementEvidence -PlayerCommands $Commands -SourceFiles $sourceFiles -PostBaselineChanges (Get-HistorySummary $System $HistoryBySystem) -ExistingInGameSurfaces $Surfaces -Reachability $Reachability -Coverage $Coverage -Decision $Decision -RecommendedSurface $Surface -Priority $Priority -PlayerRisk $Risk -SourceEvidence $evidence -ContentBrief $Brief -SpoilerBoundary $Spoiler -AcceptanceTest $Test -ExternalDocs $ExternalDocs -Notes $Notes)) | Out-Null
}

function Get-StyleGuide
{
    param(
        [string]$HeadHash,
        [string]$BaselineHash,
        [int]$LoreBookCount,
        [int]$StaticLibraryCount
    )

    return @"
# Confictura In-Game Player Documentation Style Guide

Audit target: current main runtime/data snapshot, last changed at $HeadHash

Historical synchronization point: $BaselineHash (the 2023-10-25 informational-gump and talk-text revision)

This guide describes the player-facing documentation language already present in the game. It is a routing and writing standard for later incorporation batches, not authorization to expose secrets or to replace source review with prose.

## Core principles

- Teach in the world. A feature is documented only when an ordinary player can reach the explanation through Help, the Personal Library, a physical book, NPC speech, a system help gump, a quest or discovery journal, or contextual feedback.
- Put operational truth before flavor when loss is possible. Controls, costs, permissions, persistence, destructive effects, PvP rules, and stop or undo paths must be clear before commitment.
- Let discovery remain discovery. Explain clue paths, warning signs, and interaction vocabulary while withholding exact locations, passwords, solutions, hidden reward tables, and surprises.
- Write at the point of need. Use global Help for universal controls; Library references for persistent system knowledge; lore and NPC speech for world knowledge; system gumps for dense mechanics; journals and contextual feedback for stateful steps.
- Separate source-defined reachability from live reachability. This checkout contains checked-in spawn and configuration data but no world save. Never turn a source definition into a live-world availability claim without staging evidence.

## Established visual and interaction language

### Help and built-in reference gumps

Primary source: `Data/Scripts/System/Help/Gumps/HelpGump.cs`.

- The Help gump uses the shard's full-size art `9548`, tinted through `PlayerSettings.GetGumpHue`, with a left column of compact page buttons, an explicit header, a standard close button, and a large scrollable HTML reading pane. Preserve this layout instead of introducing a visually unrelated manual.
- Its voice is direct, second-person, and operational: what a player can do, the command or button to use, and the immediate result.
- Keep command syntax literal and case-faithful. Pair every persistent toggle or automated action with its current state and stop or undo path.
- Keep global Help shallow. Link or route substantial systems to a Library reference or dedicated help gump rather than adding a wall of text to the main page.

Use Help for universal commands, account or character settings, privacy behavior, help requests, movement or interface conventions, and links to deeper in-game references.

### Personal Library and unlock behavior

Primary sources: `Data/Scripts/System/Commands/Player/MyLibrary.cs`, `Data/Scripts/Mobiles/Base/PlayerSettings.cs`, and `Data/Scripts/Custom/LoreBooks/LoreBookCatalog.cs`.

- The Personal Library uses art `9546`, the player's configured gump hue, a four-column paged index, and Previous/Next controls. Its built-in entries are Basics, conditional Creature Help, Fame & Karma, Item Properties, Skills, and Weapon Abilities; additional entries appear only when their catalog IDs are unlocked.
- Built-in entries cover foundational mechanics; unlocked entries preserve the pleasure of collecting knowledge.
- A Library reference should have a stable title, compact subject grouping, short paragraphs, and enough mechanical detail to act without consulting an external page.
- Do not silently grant secret lore as a built-in reference. Unlock it through the established catalog or teach only the non-spoiling control layer globally.

The current catalog has $StaticLibraryCount static entry declarations and $LoreBookCount checked-in XML lore records. Later content batches must preserve unique identifiers and unlock semantics.

### Physical guide, learning, and lore books

Primary sources: `Data/Scripts/Items/Books/DynamicBook.cs`, `Data/Scripts/Custom/LoreBooks/LoreBookCatalog.cs`, and `Data/Scripts/Custom/LoreBooks/LoreBooks.xml`.

- Guide and learning books may be plain and instructional. Lore books should sound authored inside the world and may be unreliable in flavor, but controls and irreversible risk must not be misleading.
- Use short titled sections and page-sized passages. Let book discovery unlock the corresponding Library entry where the catalog supports it.
- Cite acquisition only when source or checked-in placement proves it. If live stock or placement depends on a world save, say that staging must confirm it.

### NPC dialogue and the shard greeter

Primary sources: `Data/Scripts/System/Misc/Talk.cs` and `Data/Scripts/Mobiles/Civilized/ShardGreeter.cs`.

- NPC teaching is conversational, local, and role-appropriate. A shipwright teaches sailing; a banker teaches banking speech; a greeter or sage routes newcomers to general help.
- Keep speech branches short and keyword-tolerant. Repeat the action word or phrase the player should use next.
- NPC hints may point toward a mystery but should not become an exhaustive mechanics manual. Route dense material to a book, Library entry, or system help gump.

### System-specific help gumps

Representative sources: `Data/Scripts/Custom/Government System/Gumps/GovHelpGump.cs`, `Data/Scripts/Trades/Apiculture/BeeHiveHelp.cs`, and `Data/Scripts/Trades/Gardening/Network/DisplayHelpTopic.cs`.

- Use a dedicated gump when a system has several pages of state, permissions, progression, costs, or terminology.
- Put the most dangerous or expensive rules early. Preserve stable navigation labels and page ordering so returning players can find information again.
- Derive numbers and state labels from shared constants when practical in the later implementation batch; otherwise record the source symbol beside the prose review.

### Quest, discovery, and contextual feedback

Representative sources: quest scripts under `Data/Scripts/Quests`, quest routing in `Data/Scripts/System/Help/Gumps/HelpGump.cs`, and interaction messages throughout items, mobiles, trades, and regions.

- Journals teach the current objective and next clue, not the complete solution.
- Contextual feedback should say why an action failed, whether anything was consumed, and what safe corrective action is available.
- Login notices and one-time announcements are reminders, not durable documentation. Pair important rules with Help or the Library.

## Surface routing matrix

| Need | Preferred surface | Why |
| --- | --- | --- |
| Universal control, command, setting, privacy, or support path | Help | Always reachable and operational. |
| Persistent mechanics reference, progression, crafting, harvesting, or combat school | LibraryReference | Durable and searchable after the player encounters the system. |
| World history, discovery clue, dangerous place, rare object, or secret-adjacent mechanic | LoreBook | Preserves voice and discovery. |
| Role-local introduction or keyword interaction | NPCSpeech | Teaches in context and can route deeper. |
| Multi-page rules, authority, cost, or stateful management | SystemHelpGump | Supports density and stable navigation. |
| Stateful objective or discovery chain | QuestJournal | Keeps next steps tied to player progress. |
| Immediate failure, consumption, cooldown, danger, or confirmation | ContextualFeedback | Appears at the exact decision point. |

## Content shape and acceptance

Every later incorporation item should answer, in this order: what this is; how the player encounters it; how to start; controls or syntax; costs and requirements; persistent or destructive effects; stop, undo, or recovery path; where to learn more. Use concise paragraphs, literal commands, and the vocabulary already shown by the owning gump or NPC.

Acceptance is player-path based. An ordinary staging character must be able to reach the surface without staff commands, follow every navigation control, reproduce the stated behavior, and see honest feedback for invalid, costly, destructive, and disabled states. Acquisition and placement require separate live verification because no world save is present in this checkout.
"@
}

function Get-Summary
{
    param(
        [object[]]$Rows,
        [object[]]$BacklogRows,
        [string]$HeadHash,
        [string]$BaselineHash,
        [string]$BaselineDate,
        [int]$SourceCount,
        [int]$SystemCount,
        [int]$CanonicalCount,
        [int]$CommandCount,
        [int]$XmlCount,
        [int]$LoreCount,
        [int]$StaticLibraryCount,
        [int]$ChangedCurrentCount,
        [int]$HistoricalPathCount
    )

    $coverage = @($Rows | Group-Object Coverage | Sort-Object Name | ForEach-Object { "| $($_.Name) | $($_.Count) |" }) -join "`n"
    $decisions = @($Rows | Group-Object Decision | Sort-Object Name | ForEach-Object { "| $($_.Name) | $($_.Count) |" }) -join "`n"
    $kinds = @($Rows | Group-Object RowKind | Sort-Object Name | ForEach-Object { "| $($_.Name) | $($_.Count) |" }) -join "`n"
    $priorities = @($BacklogRows | Group-Object Priority | Sort-Object Name | ForEach-Object { "| $($_.Name) | $($_.Count) |" }) -join "`n"
    $topRows = @($BacklogRows | Where-Object { $_.Priority -in @('P0', 'P1', 'P2') } | Select-Object -First 25 | ForEach-Object { "| $($_.Priority) | $($_.AuditId) | $($_.Capability) | $($_.Decision) | $($_.RecommendedSurface) |" }) -join "`n"

    return @"
# In-Game Player Documentation Audit Summary

Audit target: current main runtime/data snapshot, last changed at $HeadHash

Historical synchronization point: $BaselineHash ($BaselineDate), the last comprehensive informational-gump and talk-text revision

## Outcome

The current player-facing feature set has been reconciled into $($Rows.Count) audit rows and $($BacklogRows.Count) actionable incorporation, refresh, or runtime-evidence items. External wiki pages were treated only as candidate and source-trace evidence; they did not satisfy in-game coverage.

The audit scanned $SourceCount current runtime/source `.cs` files into $SystemCount system boundaries, reconciled $CanonicalCount canonical wiki topics, found $CommandCount direct `AccessLevel.Player` command registrations, parsed $XmlCount relevant checked-in XML files including $LoreCount lore records, validated $StaticLibraryCount static Library entry declarations, and mapped $ChangedCurrentCount current paths touched after the baseline. Another $HistoricalPathCount post-baseline paths no longer resolve in the current tree and are retained as historical drift evidence rather than current capability claims.

No live-world availability is claimed. The checkout has checked-in spawn, decoration, configuration, and lore data but no world save; rows that depend on actual placement, vendor stock, existing player unlocks, or server configuration remain `RuntimeUnknown` or require owner staging evidence.

## Census by row kind

| Row kind | Count |
| --- | ---: |
$kinds

## Coverage

| Coverage | Count |
| --- | ---: |
$coverage

## Decisions

| Decision | Count |
| --- | ---: |
$decisions

## Action backlog by priority

| Priority | Count |
| --- | ---: |
$priorities

## Highest-risk gaps

| Priority | Audit row | Capability | Decision | Surface |
| --- | --- | --- | --- | --- |
$topRows

The most urgent lane is authoritative information that can prevent persistent loss or confusion: onboarding and Guide acquisition, the comprehensive Player command list, government authority and treasury rules, persistent character progression, PvP consent, offline training, homestead/property decisions, crafting queues and batch consumption, and auto-loop start/stop controls. Those topics belong in direct Help or a system help gump before flavor expansion.

## Historical drift

The 2023-10-25 synchronization point remains useful because it marks the last broad Help/talk rewrite, but it is not the census boundary. Current system rows cover every current source bucket; their `PostBaselineChanges` fields identify later enhancements to both new and old systems. The explicit historical-drift row records changed paths that were deleted, renamed, reverted, generated, or otherwise absent at current HEAD so removed work is not advertised.

Notable current drift reviewed separately includes auto-loop taming and harvesting, crafting search/queue/batch/container controls, Character Level service commands, Random Encounters, Monster Nests, dynamic lore/library unlocks, dungeon difficulty help, race/creature-character options, government rules, Town Crier privacy behavior, and boss-taming restrictions. Internal AI changes, staff tooling, pure defect repairs, disabled Test Center/turn-combat configuration, and ordinary content variants are explicit exclusions unless they expose a separate player control or risk.

## Recommendations by surface

- **Help:** refresh onboarding, Guide acquisition, the direct Player command list, persistent settings, privacy behavior, automated-action stop controls, and disabled-state messaging.
- **LibraryReference:** add concise durable references for substantial progression, crafting/harvesting, combat schools, Searching/Hiding/Stealth, and other mechanics that are learnable but hard to reconstruct.
- **SystemHelpGump:** update government first; retain apiculture and gardening as the model for point-of-use help. Put authority, cost, persistence, and destructive rules before secondary detail.
- **LoreBook and NPCSpeech:** preserve discovery for encounters, nests, rare artifacts, and world systems. Teach clue paths and danger without exact locations, solutions, or reward tables.
- **QuestJournal and ContextualFeedback:** close state-specific gaps: explain the next clue, why an action failed, whether resources were consumed, and how to stop or recover.

## Owner-run verification checklist

1. Use a fresh ordinary account and character; open Help and traverse every page, button, command example, Library link, and support path.
2. Verify how a new player actually acquires the Guide to Adventure and whether the greeter, sage, librarian, or creation flow matches the text.
3. Run every direct Player command from the audit, including invalid syntax, prerequisites, persistent state, and stop or undo behavior.
4. Open the Personal Library before and after representative lore discoveries; confirm built-in entries, unlock IDs, duplicate prevention, persistence across save/restart, and inaccessible-entry behavior.
5. Confirm physical guide, learning, and lore book vendor stock or world placement; record facets, NPC types, item IDs/types, and acquisition requirements without using staff-only reachability as proof.
6. Exercise government founding, services, treasury, maintenance, elections, authority, bans, wars/alliances, and destructive actions on disposable staging data.
7. Exercise crafting search, batch amount, queue, source/destination container, house-container, cancel, failure, and resource-consumption paths with inexpensive materials first.
8. Exercise auto-loop harvesting and taming start, interruption, captcha/failure, logout/death/range behavior, and explicit stop commands.
9. Verify discovery systems through ordinary clues and contextual feedback; confirm the audit does not require exposing locations, passwords, solutions, or rewards.
10. Confirm disabled systems remain disabled and do not advertise unavailable player controls. Reclassify any configuration- or placement-dependent row with captured staging evidence.

## Reproduction

Generate the four artifacts:

    & ./docs/codebase-audit/tools/New-InGamePlayerDocumentationAudit.ps1

Validate the committed artifacts byte-for-byte without writing:

    & ./docs/codebase-audit/tools/New-InGamePlayerDocumentationAudit.ps1 -ValidateOnly

Validation fails on duplicate IDs, invalid controlled values, missing evidence paths, duplicate lore/library IDs, unreconciled system buckets, canonical topics, Player commands, current post-baseline paths, or output drift.
"@
}

function Assert-Audit
{
    param(
        [object[]]$Rows,
        [object[]]$BacklogRows,
        [string[]]$Systems,
        [object[]]$CanonicalDocs,
        [object[]]$PlayerCommands,
        [string[]]$ChangedCurrentPaths,
        [hashtable]$SystemByPath,
        [string[]]$XmlPaths,
        [object[]]$LoreBooks,
        [object[]]$StaticEntries
    )

    $errors = New-Object System.Collections.Generic.List[string]

    $enums = @{
        RowKind = @('SystemBoundary', 'CanonicalTopic', 'Capability', 'HistoricalDrift')
        ActivationState = @('ActiveBySource', 'DisabledByConfig', 'Historical', 'Internal', 'RuntimeUnknown')
        Reachability = @('SourceDefined', 'CheckedInPlacement', 'Acquirable', 'Disabled', 'RuntimeUnknown')
        Coverage = @('Complete', 'Partial', 'Stale', 'Missing', 'IntentionalDiscovery', 'NotApplicable', 'RuntimeUnknown')
        Decision = @('KeepAsIs', 'RefreshExisting', 'Incorporate', 'NoDocumentationNeeded', 'NeedsRuntimeEvidence')
        RecommendedSurface = @('Help', 'LibraryReference', 'LoreBook', 'NPCSpeech', 'SystemHelpGump', 'QuestJournal', 'ContextualFeedback')
        Priority = @('P0', 'P1', 'P2', 'P3', 'P4')
    }

    foreach ($group in $Rows | Group-Object Id | Where-Object Count -gt 1)
    {
        $errors.Add("Duplicate audit ID: $($group.Name)") | Out-Null
    }

    foreach ($group in $BacklogRows | Group-Object Id | Where-Object Count -gt 1)
    {
        $errors.Add("Duplicate backlog ID: $($group.Name)") | Out-Null
    }

    foreach ($row in $Rows)
    {
        foreach ($field in $enums.Keys)
        {
            if ($row.$field -notin $enums[$field])
            {
                $errors.Add("Invalid $field '$($row.$field)' in $($row.Id)") | Out-Null
            }
        }

        if ([string]::IsNullOrWhiteSpace($row.SourceEvidence))
        {
            $errors.Add("Missing source evidence in $($row.Id)") | Out-Null
        }

        foreach ($path in @($row.SourceFiles -split ';') + @($row.ExternalDocs -split ';'))
        {
            if ([string]::IsNullOrWhiteSpace($path))
            {
                continue
            }

            if (-not (Test-Path -LiteralPath (Convert-ToFullPath $path)))
            {
                $errors.Add("Missing evidence path '$path' in $($row.Id)") | Out-Null
            }
        }
    }

    $systemRows = @($Rows | Where-Object RowKind -eq 'SystemBoundary')
    foreach ($system in $Systems)
    {
        if (-not ($systemRows | Where-Object System -eq $system))
        {
            $errors.Add("Unreconciled current system bucket: $system") | Out-Null
        }
    }

    $docRows = @($Rows | Where-Object RowKind -eq 'CanonicalTopic')
    foreach ($doc in $CanonicalDocs)
    {
        if (-not ($docRows | Where-Object ExternalDocs -eq $doc.DocPath))
        {
            $errors.Add("Unreconciled canonical topic: $($doc.DocPath)") | Out-Null
        }
    }

    foreach ($command in $PlayerCommands)
    {
        if (-not ($Rows | Where-Object { (($_.PlayerCommands -split ';') -contains $command.Name) -and ($_.SourceFiles -split ';') -contains $command.Path }))
        {
            $errors.Add("Unreconciled Player command: $($command.Name) in $($command.Path)") | Out-Null
        }
    }

    foreach ($path in $ChangedCurrentPaths)
    {
        if (-not $SystemByPath.ContainsKey($path))
        {
            $errors.Add("Unreconciled current post-baseline path: $path") | Out-Null
        }
    }

    foreach ($path in $XmlPaths)
    {
        try
        {
            [xml]([System.IO.File]::ReadAllText((Convert-ToFullPath $path))) | Out-Null
        }
        catch
        {
            $errors.Add("Malformed checked-in XML '$path': $($_.Exception.Message)") | Out-Null
        }
    }

    foreach ($group in $LoreBooks | Group-Object Id | Where-Object { [string]::IsNullOrWhiteSpace($_.Name) -or $_.Count -gt 1 })
    {
        $errors.Add("Missing or duplicate LoreBooks.xml ID: '$($group.Name)'") | Out-Null
    }

    foreach ($book in $LoreBooks)
    {
        if ([string]::IsNullOrWhiteSpace($book.Title) -or [string]::IsNullOrWhiteSpace($book.Text))
        {
            $errors.Add("LoreBooks.xml record '$($book.Id)' has empty title or text") | Out-Null
        }
    }

    foreach ($group in $StaticEntries | Group-Object Id | Where-Object { [string]::IsNullOrWhiteSpace($_.Name) -or $_.Count -gt 1 })
    {
        $errors.Add("Missing or duplicate static Library entry ID: '$($group.Name)'") | Out-Null
    }

    if ($errors.Count -gt 0)
    {
        throw ("Audit validation failed:`n- " + ($errors -join "`n- "))
    }
}

if (-not (Test-Path -LiteralPath $OutputDir -PathType Container))
{
    throw "Output directory not found: $OutputDir"
}

$resolvedBaseline = ([string](Invoke-Git @('rev-parse', "$BaselineCommit^{commit}") | Select-Object -First 1)).Trim()
$auditScopePaths = @('Data/Scripts', 'Data/System/Source', 'Data/Spawns', 'Data/XMLSpawn', 'Data/Decoration', 'Info/settings.xml', 'Data/TurnBasedCombat/TurnBasedCombat.cfg')
$headHash = ([string](Invoke-Git (@('rev-list', '-1', 'HEAD', '--') + $auditScopePaths) | Select-Object -First 1)).Trim()
$baselineDate = ([string](Invoke-Git @('show', '-s', '--format=%cs', $resolvedBaseline) | Select-Object -First 1)).Trim()

$knownSystemByPath = @{}
$inventoryPath = Join-Path $OutputDir 'cross-tree-runtime-inventory.csv'

if (Test-Path -LiteralPath $inventoryPath -PathType Leaf)
{
    foreach ($row in Import-Csv -LiteralPath $inventoryPath)
    {
        $path = ($row.Path -replace '\\', '/')
        if (-not [string]::IsNullOrWhiteSpace($path) -and -not [string]::IsNullOrWhiteSpace($row.System) -and (Test-Path -LiteralPath (Convert-ToFullPath $path)))
        {
            $knownSystemByPath[$path] = $row.System
        }
    }
}

$sourceFiles = Get-CurrentSourceFiles
$systemByPath = @{}
$pathsBySystem = @{}

foreach ($path in $sourceFiles)
{
    $system = Get-SourceSystem -Path $path -KnownSystemByPath $knownSystemByPath
    $systemByPath[$path] = $system

    if (-not $pathsBySystem.ContainsKey($system))
    {
        $pathsBySystem[$system] = New-Object System.Collections.Generic.List[string]
    }

    $pathsBySystem[$system].Add($path) | Out-Null
}

$dataBuckets = [ordered]@{
    'Data:SpawnMaps' = 'Data/Spawns'
    'Data:XMLSpawn' = 'Data/XMLSpawn'
    'Data:Decoration' = 'Data/Decoration'
    'Data:RegionXML' = 'Data/System/XML'
    'Data:LoreCatalog' = 'Data/Scripts/Custom/LoreBooks/LoreBooks.xml'
    'Data:Configuration' = 'Info/settings.xml;Data/TurnBasedCombat/TurnBasedCombat.cfg'
}

foreach ($system in $dataBuckets.Keys)
{
    $paths = New-Object System.Collections.Generic.List[string]
    foreach ($candidate in $dataBuckets[$system] -split ';')
    {
        $fullPath = Convert-ToFullPath $candidate
        if (Test-Path -LiteralPath $fullPath -PathType Container)
        {
            Get-ChildItem -LiteralPath $fullPath -Recurse -File | ForEach-Object { $paths.Add((Convert-ToRepoPath $_.FullName)) | Out-Null }
        }
        elseif (Test-Path -LiteralPath $fullPath -PathType Leaf)
        {
            $paths.Add($candidate) | Out-Null
        }
    }
    $pathsBySystem[$system] = $paths

    foreach ($path in $paths)
    {
        $systemByPath[$path] = $system
    }
}

$scriptXmlPaths = New-Object System.Collections.Generic.List[string]
Get-ChildItem -LiteralPath (Convert-ToFullPath 'Data/Scripts') -Recurse -File -Filter '*.xml' |
    ForEach-Object {
        $path = Convert-ToRepoPath $_.FullName
        if ($path -ne 'Data/Scripts/Custom/LoreBooks/LoreBooks.xml')
        {
            $scriptXmlPaths.Add($path) | Out-Null
            $systemByPath[$path] = 'Data:ScriptXML'
        }
    }
$pathsBySystem['Data:ScriptXML'] = $scriptXmlPaths

$systems = @($pathsBySystem.Keys | Sort-Object)
$playerCommands = Get-PlayerCommands -SourceFiles $sourceFiles -SystemByPath $systemByPath
$history = Get-History -ResolvedBaseline $resolvedBaseline -AuditTarget $headHash -SystemByPath $systemByPath

$documentationTablePath = Join-Path $OutputDir 'documentation-truth-table.csv'
if (-not (Test-Path -LiteralPath $documentationTablePath -PathType Leaf))
{
    throw "Required canonical documentation seed is missing: $documentationTablePath"
}
$canonicalDocs = @(Get-CurrentCanonicalDocs -DocumentationTablePath $documentationTablePath)

[xml]$loreXml = Get-Content -LiteralPath (Convert-ToFullPath 'Data/Scripts/Custom/LoreBooks/LoreBooks.xml') -Raw
$loreBooks = @($loreXml.SelectNodes('//Book') | ForEach-Object {
    [pscustomobject]@{
        Id = [string]$_.id
        Title = [string]$_.title
        Text = [string]$_.text
    }
})

$catalogContent = [System.IO.File]::ReadAllText((Convert-ToFullPath 'Data/Scripts/Custom/LoreBooks/LoreBookCatalog.cs'))
$staticEntries = @([regex]::Matches($catalogContent, 'new\s+PlayerLibraryEntry\s*\(\s*"(?<Id>[^"]+)"') | ForEach-Object {
    [pscustomobject]@{ Id = $_.Groups['Id'].Value }
})
$relevantXmlPaths = @(
    @($pathsBySystem['Data:RegionXML']) +
    @($pathsBySystem['Data:XMLSpawn']) +
    @($pathsBySystem['Data:Decoration']) +
    @($pathsBySystem['Data:SpawnMaps']) +
    @($pathsBySystem['Data:LoreCatalog']) +
    @($pathsBySystem['Data:ScriptXML']) |
        Where-Object { $_ -like '*.xml' } |
        Sort-Object -Unique
)

$auditRows = New-Object System.Collections.Generic.List[object]

foreach ($system in $systems)
{
    $paths = @($pathsBySystem[$system] | Sort-Object -Unique)
    $commands = @($playerCommands | Where-Object System -eq $system | Select-Object -ExpandProperty Name -Unique | Sort-Object)

    if ($system -like 'Data:*')
    {
        $reachability = if ($system -in @('Data:SpawnMaps', 'Data:XMLSpawn', 'Data:Decoration')) { 'CheckedInPlacement' } elseif ($system -eq 'Data:Configuration') { 'RuntimeUnknown' } else { 'SourceDefined' }
        $activation = if ($system -eq 'Data:Configuration') { 'RuntimeUnknown' } else { 'ActiveBySource' }
        $coverage = if ($system -eq 'Data:LoreCatalog') { 'Complete' } else { 'RuntimeUnknown' }
        $decision = if ($system -eq 'Data:LoreCatalog') { 'KeepAsIs' } else { 'NeedsRuntimeEvidence' }
        $surface = if ($system -eq 'Data:LoreCatalog') { 'LoreBook' } else { 'ContextualFeedback' }
        $priority = if ($system -eq 'Data:LoreCatalog') { 'P4' } else { 'P3' }

        $auditRows.Add((New-AuditRow -Id ('IGD-SYS-' + (Convert-ToSlug $system)) -RowKind 'SystemBoundary' -System $system -Capability ("Checked-in data boundary: " + $system.Substring(5)) -Audience 'PlayerOrMixed' -ActivationState $activation -PlayerEntryPath 'Reached indirectly through world placement, spawning, decoration, lore acquisition, or configuration-gated behavior.' -AcquisitionPlacementEvidence "$($paths.Count) checked-in file(s); no world save is present, so live placement is not asserted." -PlayerCommands '' -SourceFiles (Join-Unique $paths) -PostBaselineChanges 'Checked against the post-baseline git history; individual live state remains outside the checkout.' -ExistingInGameSurfaces 'World objects, NPCs, books, regions, and contextual messages driven by checked-in data.' -Reachability $reachability -Coverage $coverage -Decision $decision -RecommendedSurface $surface -Priority $priority -PlayerRisk 'Medium where checked-in data does not establish that the intended teaching surface is live and reachable.' -SourceEvidence 'Current checked-in data enumerated directly; XML lore records are parsed and validated.' -ContentBrief 'Confirm source/data references against live staging placement before claiming availability; keep lore catalog entries synchronized with their unlock paths.' -SpoilerBoundary 'Record placement evidence internally without publishing secret locations or solutions in player prose.' -AcceptanceTest 'Owner stages an ordinary player path to representative placements and records type, map/facet, acquisition, and reachability evidence.' -ExternalDocs '' -Notes 'Data boundary row; checked-in placement is evidence, not proof of a loaded live world.')) | Out-Null
        continue
    }

    $profile = Get-SystemProfile -System $system -Commands $commands
    $commandPaths = @($playerCommands | Where-Object System -eq $system | Select-Object -ExpandProperty Path -Unique)
    $samplePaths = @(@($paths | Select-Object -First 12) + $commandPaths | Sort-Object -Unique)
    $sourceEvidence = "$($paths.Count) current source file(s) grouped by current path; first 12 plus every direct Player-command source are listed in SourceFiles. Existing inventory is ownership guidance only."

    $auditRows.Add((New-AuditRow -Id ('IGD-SYS-' + (Convert-ToSlug $system)) -RowKind 'SystemBoundary' -System $system -Capability "Current source boundary: $system" -Audience $profile.Audience -ActivationState $profile.ActivationState -PlayerEntryPath $profile.EntryPath -AcquisitionPlacementEvidence $profile.PlacementEvidence -PlayerCommands (Join-Unique $commands) -SourceFiles (Join-Unique $samplePaths) -PostBaselineChanges (Get-HistorySummary $system $history.BySystem) -ExistingInGameSurfaces $profile.Surfaces -Reachability $profile.Reachability -Coverage $profile.Coverage -Decision $profile.Decision -RecommendedSurface $profile.Surface -Priority $profile.Priority -PlayerRisk $profile.Risk -SourceEvidence $sourceEvidence -ContentBrief $profile.Brief -SpoilerBoundary $profile.Spoiler -AcceptanceTest $profile.Test -ExternalDocs '' -Notes $profile.Notes)) | Out-Null
}

foreach ($doc in $canonicalDocs)
{
    $profile = Get-CanonicalTopicProfile $doc
    $sourcePaths = @()
    foreach ($path in $doc.VerifiedSourceFiles -split ';')
    {
        $path = $path.Trim()
        if (-not [string]::IsNullOrWhiteSpace($path) -and (Test-Path -LiteralPath (Convert-ToFullPath $path)))
        {
            $sourcePaths += $path
        }
    }

    $system = 'CanonicalWiki:' + $doc.IndexCategory
    $audience = if ($doc.IndexCategory -in @('Staff And Administration', 'Technical And Engine Reference')) { 'InternalOrStaff' } else { 'Player' }
    $reachability = if ($profile.Coverage -eq 'Complete') { 'SourceDefined' } else { 'RuntimeUnknown' }
    $sourceEvidence = if ($sourcePaths.Count -gt 0) { 'Current source paths from the canonical documentation truth table were revalidated against the filesystem.' } else { 'Canonical external topic retained as a candidate; no current source path is asserted.' }
    $auditRows.Add((New-AuditRow -Id ('IGD-DOC-' + (Convert-ToSlug $doc.DocPath)) -RowKind 'CanonicalTopic' -System $system -Capability $doc.Title -Audience $audience -ActivationState 'RuntimeUnknown' -PlayerEntryPath 'External topic identifies a candidate player capability; the listed in-game surface and owning source determine actual entry.' -AcquisitionPlacementEvidence 'External wiki does not prove acquisition or placement; current source paths are retained as research evidence.' -PlayerCommands '' -SourceFiles (Join-Unique $sourcePaths) -PostBaselineChanges 'Reviewed against current source and the historical synchronization point through the owning system rows.' -ExistingInGameSurfaces 'Classified from current Help, Library, books, NPC, system-gump, quest, and feedback sources; staging remains required for reachability.' -Reachability $reachability -Coverage $profile.Coverage -Decision $profile.Decision -RecommendedSurface $profile.Surface -Priority $profile.Priority -PlayerRisk $profile.Risk -SourceEvidence $sourceEvidence -ContentBrief $profile.Brief -SpoilerBoundary $profile.Spoiler -AcceptanceTest $profile.Test -ExternalDocs $doc.DocPath -Notes $profile.Notes)) | Out-Null
}

Add-CuratedCapability -Rows $auditRows -HistoryBySystem $history.BySystem -Slug 'onboarding-help-guide-library' -System 'System:Help' -Capability 'New-player onboarding, Help, Guide to Adventure, and Personal Library routing' -Audience 'Player' -ActivationState 'ActiveBySource' -EntryPath 'Open Help, use the Personal Library command, speak to the shard greeter, or acquire and read the Guide to Adventure.' -PlacementEvidence 'Help and Library are source-defined. Guide acquisition text and greeter/vendor placement require staging confirmation because no world save is present.' -Commands 'MyLibrary' -Files @('Data/Scripts/System/Help/Gumps/HelpGump.cs','Data/Scripts/System/Commands/Player/MyLibrary.cs','Data/Scripts/Items/Books/DynamicBook.cs','Data/Scripts/Mobiles/Civilized/ShardGreeter.cs') -Surfaces 'Help gump, Guide to Adventure book, Personal Library, shard greeter dialogue/gump.' -Reachability 'RuntimeUnknown' -Coverage 'Stale' -Decision 'RefreshExisting' -Surface 'Help' -Priority 'P1' -Risk 'High: the authoritative onboarding path appears to make acquisition assumptions that current source alone does not prove.' -Brief 'Reconcile how a fresh player reaches Help, the Guide, the Library, basic commands, and deeper references. Correct only after owner staging confirms actual stock or placement.' -Spoiler 'Onboarding is not secret; do not reveal unrelated world discoveries.' -Test 'A fresh ordinary character can find and traverse every onboarding surface without staff intervention or an external wiki.' -ExternalDocs 'docs/wiki/Confictura_Introduction.md;docs/wiki/Guide_To_Adventure_Book.md;docs/wiki/World_Basics_Commands.md'

Add-CuratedCapability -Rows $auditRows -HistoryBySystem $history.BySystem -Slug 'direct-player-command-discovery' -System 'System:Commands' -Capability 'Complete direct Player command discovery and syntax' -Audience 'Player' -ActivationState 'ActiveBySource' -EntryPath 'Open Help or encounter system-specific teaching before using a direct Player-access command.' -PlacementEvidence 'Direct command registrations are source-defined; the audit census lists them by owning source boundary.' -Commands (Join-Unique ($playerCommands | Select-Object -ExpandProperty Name)) -Files @('Data/Scripts/System/Help/Gumps/HelpGump.cs') -Surfaces 'Help command/reference pages and contextual system gumps.' -Reachability 'SourceDefined' -Coverage 'Stale' -Decision 'RefreshExisting' -Surface 'Help' -Priority 'P1' -Risk 'High: undiscoverable commands include persistent settings, automated actions, service controls, and stop paths.' -Brief 'Rebuild the in-game command reference from the current direct Player registration census, grouping aliases and routing dense systems to their own help.' -Spoiler 'List commands and safe effects; omit secret output content.' -Test 'Every direct Player command is discoverable in game with syntax, prerequisites, persistent effects, error behavior, and stop or undo path.' -ExternalDocs 'docs/wiki/In_Game_Command_List.md'

Add-CuratedCapability -Rows $auditRows -HistoryBySystem $history.BySystem -Slug 'auto-loop-taming' -System 'System:Skills' -Capability 'Auto-loop animal taming and explicit stop controls' -Audience 'Player' -ActivationState 'ActiveBySource' -EntryPath 'Use Animal Taming on a target; stop through the registered taming stop command or interruption behavior.' -PlacementEvidence 'Skill and command source are runtime-visible; live shard configuration is not required for registration.' -Commands 'StopTame;StopTaming' -Files @('Data/Scripts/System/Skills/Taming.cs','Data/Scripts/System/Skills/TamingLoopController.cs') -Surfaces 'Skill targeting and contextual messages; no durable in-game reference was found.' -Reachability 'SourceDefined' -Coverage 'Missing' -Decision 'Incorporate' -Surface 'Help' -Priority 'P1' -Risk 'High: an automated repeated action needs an obvious stop path, interruption rules, and failure feedback.' -Brief 'Teach how looping starts, how to stop it, and what movement, range, target state, success, failure, death, or logout does.' -Spoiler 'No spoiler restriction; do not publish hidden creature locations.' -Test 'Start and stop the loop as an ordinary player and verify all interruption and invalid-target messages.'

Add-CuratedCapability -Rows $auditRows -HistoryBySystem $history.BySystem -Slug 'auto-loop-harvesting' -System 'Trades:Harvest' -Capability 'Auto-loop harvesting, captcha/failure behavior, and stop control' -Audience 'Player' -ActivationState 'ActiveBySource' -EntryPath 'Use an eligible harvesting tool or weapon on a resource target; stop with the registered command or interruption path.' -PlacementEvidence 'Harvest tools, controller, and command are runtime-visible; resource placement remains live-world dependent.' -Commands 'StopHarvest' -Files @('Data/Scripts/Trades/Harvest/HarvestLoopController.cs','Data/Scripts/Trades/Harvest/HarvestSystem.cs','Data/Scripts/Items/Trades/Harvest Tools/BaseHarvestTool.cs') -Surfaces 'Tool targeting, captcha or failure feedback, and status messages; no durable reference was found.' -Reachability 'RuntimeUnknown' -Coverage 'Missing' -Decision 'Incorporate' -Surface 'Help' -Priority 'P1' -Risk 'High: repeated resource actions, anti-automation checks, and explicit stop behavior must be clear.' -Brief 'Teach eligible tools, how looping begins and ends, stop syntax, resource depletion, range/movement, captcha, logout, death, and failure behavior.' -Spoiler 'Explain resource categories, not rare node locations.' -Test 'Exercise every start, stop, interruption, captcha, depletion, and invalid-target state on disposable staging resources.'

Add-CuratedCapability -Rows $auditRows -HistoryBySystem $history.BySystem -Slug 'crafting-queue-batch-containers-search' -System 'Trades:Core' -Capability 'Crafting search, batch amount, queue, source/destination containers, and cancellation' -Audience 'Player' -ActivationState 'ActiveBySource' -EntryPath 'Open a supported craft gump and use search, batch, queue, and container controls.' -PlacementEvidence 'Craft gumps and controllers are runtime-visible; individual recipes and tools are grouped under the shared mechanic.' -Commands '' -Files @('Data/Scripts/Trades/Core/Gumps/CraftGump.cs','Data/Scripts/Trades/Core/Gumps/CraftQueueGump.cs','Data/Scripts/Trades/Core/Gumps/CraftBatchStatusGump.cs','Data/Scripts/Trades/Core/Gumps/CraftGumpItem.cs') -Surfaces 'Craft gump labels and status gumps provide contextual feedback but no complete durable reference.' -Reachability 'SourceDefined' -Coverage 'Partial' -Decision 'Incorporate' -Surface 'SystemHelpGump' -Priority 'P1' -Risk 'High: batch and queue operations consume materials and persistent container choices can produce costly mistakes.' -Brief 'Add point-of-use help covering search, quantity limits, queue order, material consumption, source/destination selection, house containers, cancellation, failure, and persistence.' -Spoiler 'Do not enumerate secret or rare recipes.' -Test 'Craft inexpensive samples through search, batch, queue, container, cancel, failure, close/reopen, logout, and insufficient-material paths.' -ExternalDocs 'docs/wiki/Crafting_Core.md;docs/wiki/Crafting_Queue_Design.md;docs/wiki/Crafting_QoL_Roadmap.md;docs/wiki/Standard_Crafting.md'

Add-CuratedCapability -Rows $auditRows -HistoryBySystem $history.BySystem -Slug 'character-level-service' -System 'Custom:Progression' -Capability 'Character Level persistent progression and player service commands' -Audience 'Player' -ActivationState 'ActiveBySource' -EntryPath 'Use the registered service commands or encounter Character Level feedback while progressing.' -PlacementEvidence 'Current runtime-visible source defines the service; live configuration and existing-character state require staging.' -Commands '' -Files @('Data/Scripts/Custom/Progression/CharacterLevel/CharacterLevelService.cs','Data/Scripts/Custom/Progression/CharacterLevel/CharacterLevelCommands.cs') -Surfaces 'Command feedback and progression messages; no complete persistent in-game reference was found.' -Reachability 'RuntimeUnknown' -Coverage 'Missing' -Decision 'Incorporate' -Surface 'LibraryReference' -Priority 'P1' -Risk 'High: persistent progression, rewards, caps, and service actions affect long-lived characters.' -Brief 'Explain activation, progression sources, caps, rewards, commands, persistence, respec or recovery behavior, and interactions with older characters.' -Spoiler 'Do not publish hidden reward locations or encounter solutions.' -Test 'Use fresh and existing staging characters through gain, cap, command, save/restart, and invalid-state paths.' -ExternalDocs 'docs/wiki/Character_Level_Recon_Report.md'

Add-CuratedCapability -Rows $auditRows -HistoryBySystem $history.BySystem -Slug 'government-authority-economy' -System 'Custom:Government System' -Capability 'Player government founding, authority, elections, treasury, maintenance, conflict, and destructive actions' -Audience 'Player' -ActivationState 'ActiveBySource' -EntryPath 'Use government stones, deeds, city structures, vendors, regions, and the dedicated help gump.' -PlacementEvidence 'System types and help gump are source-defined; live city objects, treasury state, and placement require staging.' -Commands '' -Files @('Data/Scripts/Custom/Government System/Gumps/GovHelpGump.cs','Data/Scripts/Custom/Government System') -Surfaces 'Dedicated multi-page government help gump plus contextual gumps and messages.' -Reachability 'RuntimeUnknown' -Coverage 'Stale' -Decision 'RefreshExisting' -Surface 'SystemHelpGump' -Priority 'P1' -Risk 'High: persistent towns, treasury funds, maintenance, elections, bans, wars, permissions, and disbanding can cause material loss.' -Brief 'Source-review every help page and place current costs, authority, persistence, maintenance, elections, conflict, and destructive confirmations before secondary detail.' -Spoiler 'Government rules are not secrets; do not expose staff-only controls.' -Test 'Use disposable staging towns to exercise founding through save/restart, election, treasury, authority, conflict, and disband paths.' -ExternalDocs 'docs/wiki/Government_System.md;docs/wiki/Player_Government_System_Guide.md'

Add-CuratedCapability -Rows $auditRows -HistoryBySystem $history.BySystem -Slug 'dynamic-lore-library-unlocks' -System 'Custom:LoreBooks' -Capability 'Dynamic lore catalog, physical discoveries, and Personal Library unlocks' -Audience 'Player' -ActivationState 'ActiveBySource' -EntryPath 'Discover or read a cataloged lore book, then open the Personal Library.' -PlacementEvidence "$($loreBooks.Count) XML records and $($staticEntries.Count) static entries are checked in; live book placement and existing unlock state require staging." -Commands 'MyLibrary' -Files @('Data/Scripts/Custom/LoreBooks/LoreBooks.xml','Data/Scripts/Custom/LoreBooks/LoreBookCatalog.cs','Data/Scripts/System/Commands/Player/MyLibrary.cs') -Surfaces 'Physical lore books and unlocked Personal Library entries.' -Reachability 'RuntimeUnknown' -Coverage 'Complete' -Decision 'KeepAsIs' -Surface 'LoreBook' -Priority 'P4' -Risk 'Low: the established surface matches the shard tradition and preserves discovery.' -Brief 'Keep catalog IDs unique, preserve unlock semantics, and add new lore through the same physical-book-to-Library path.' -Spoiler 'Do not globally list undiscovered lore or placement.' -Test 'Acquire representative books normally, verify one-time unlock, Library visibility, duplicate handling, and save/restart persistence.'

Add-CuratedCapability -Rows $auditRows -HistoryBySystem $history.BySystem -Slug 'random-encounters-monster-nests' -System 'Custom:PvE' -Capability 'Random Encounters and Monster Nests discovery, danger, persistence, and cleanup' -Audience 'Player' -ActivationState 'ActiveBySource' -EntryPath 'Encounter the systems through exploration and contextual world clues.' -PlacementEvidence 'Engines, definitions, and checked-in configuration are present; actual live spawn coverage and cleanup state require staging.' -Commands '' -Files @('Data/Scripts/Custom/PvE/RandomEncounters/EncounterEngine.cs','Data/Scripts/Custom/PvE/RandomEncounters/RandomEncounters.xml','Data/Scripts/Custom/PvE/MonsterNests/MonsterNest.cs','Data/Scripts/Custom/PvE/MonsterNests/MonsterNestEntity.cs') -Surfaces 'World clues, encounter feedback, loot, and external wiki research; no complete in-game mechanics reference is required.' -Reachability 'RuntimeUnknown' -Coverage 'IntentionalDiscovery' -Decision 'RefreshExisting' -Surface 'LoreBook' -Priority 'P3' -Risk 'Medium: players should recognize danger, persistence, and clue paths without receiving spawn or reward spoilers.' -Brief 'Add or verify non-spoiling lore and contextual warnings for how encounters appear, persist, escalate, and safely conclude.' -Spoiler 'Hide spawn coordinates, trigger solutions, exact compositions, and reward tables.' -Test 'Find representative encounters through ordinary play, verify clue/warning/cleanup feedback, and confirm no prose reveals exact solutions.' -ExternalDocs 'docs/wiki/Random_Encounter_Engine.md;docs/wiki/Monster_Nest_System.md'

Add-CuratedCapability -Rows $auditRows -HistoryBySystem $history.BySystem -Slug 'dungeon-difficulty-help' -System 'System:Regions' -Capability 'Dungeon difficulty levels and region feedback' -Audience 'Player' -ActivationState 'ActiveBySource' -EntryPath 'Enter affected dungeon regions or read the current Help difficulty explanation.' -PlacementEvidence 'Region and Help source are checked in; current live region loading and exact placement require staging.' -Commands '' -Files @('Data/Scripts/System/Help/Gumps/HelpGump.cs','Data/System/XML/Regions.xml') -Surfaces 'Help difficulty text and contextual region behavior.' -Reachability 'RuntimeUnknown' -Coverage 'Stale' -Decision 'RefreshExisting' -Surface 'Help' -Priority 'P2' -Risk 'Medium to high: stale danger tiers can lead to avoidable death and loss.' -Brief 'Reconcile every displayed difficulty name and consequence with current Regions.xml and source behavior; route secrets to world clues.' -Spoiler 'Describe danger tiers, not exact monster or treasure placements.' -Test 'Enter representative regions at each difficulty and compare displayed help, entry feedback, and actual modifiers.'

Add-CuratedCapability -Rows $auditRows -HistoryBySystem $history.BySystem -Slug 'race-creature-character-options' -System 'System:Misc' -Capability 'Race and creature-character creation or transformation options' -Audience 'Player' -ActivationState 'RuntimeUnknown' -EntryPath 'Create a character, use a race token, or use an established creature transformation path.' -PlacementEvidence 'Settings and transformation types are checked in; the enabled creation menu and live acquisition paths require staging.' -Commands '' -Files @('Info/settings.xml','Data/Scripts/System/Misc/Settings.cs') -Surfaces 'Character creation/options, race token feedback, transformation books or items, and Library references.' -Reachability 'RuntimeUnknown' -Coverage 'Partial' -Decision 'RefreshExisting' -Surface 'LibraryReference' -Priority 'P2' -Risk 'Medium to high: identity and persistent character choices need clear permanence, restrictions, and recovery rules.' -Brief 'Explain available choices, permanence, eligibility, equipment/body implications, and recovery or reversal without exposing staff-only configuration.' -Spoiler 'Do not reveal hidden transformation acquisition locations.' -Test 'Exercise each enabled option on disposable staging characters, including save/restart and invalid/repeat use.' -ExternalDocs 'docs/wiki/Race_Token_System.md;docs/wiki/Creature_Transformation_Guide.md'

Add-CuratedCapability -Rows $auditRows -HistoryBySystem $history.BySystem -Slug 'town-crier-privacy-bridge' -System 'System:Help' -Capability 'Town Crier private speech and external publication boundary' -Audience 'Player' -ActivationState 'ActiveBySource' -EntryPath 'Use Town Crier chat or the documented private speech form and receive contextual delivery feedback.' -PlacementEvidence 'Help and bridge source define behavior; live NPC placement and webhook delivery require owner staging.' -Commands '' -Files @('Data/Scripts/System/Help/Gumps/HelpGump.cs','Data/Scripts/System/Misc/Talk.cs') -Surfaces 'Help privacy text, Town Crier dialogue, and contextual feedback.' -Reachability 'RuntimeUnknown' -Coverage 'Partial' -Decision 'RefreshExisting' -Surface 'Help' -Priority 'P1' -Risk 'High: players need an accurate boundary between local/private speech and externally published content.' -Brief 'State exactly which Town Crier interactions can leave the game, how to use private/local speech, what content is sanitized, and what delivery feedback means.' -Spoiler 'No spoiler restriction; do not expose webhook or staff configuration.' -Test 'On staging, exercise public, private, offline, rejected, and delivered cases with a test endpoint and ordinary player account.'

Add-CuratedCapability -Rows $auditRows -HistoryBySystem $history.BySystem -Slug 'searching-hiding-stealth' -System 'System:Skills' -Capability 'Searching, Hiding, Stealth, reveal tools, and detection interactions' -Audience 'Player' -ActivationState 'ActiveBySource' -EntryPath 'Use Search, Hiding, Stealth, Tracking, Reveal, or specialized detection items and abilities.' -PlacementEvidence 'Skill and item behavior is runtime-visible; item acquisition and live availability require staging.' -Commands '' -Files @('Data/Scripts/System/Skills/Searching.cs','Data/Scripts/System/Skills/Hiding.cs','Data/Scripts/System/Skills/Stealth.cs') -Surfaces 'Skill feedback and scattered item/ability messages; no complete in-game mechanics reference was found.' -Reachability 'RuntimeUnknown' -Coverage 'Partial' -Decision 'Incorporate' -Surface 'LibraryReference' -Priority 'P2' -Risk 'Medium: layered detection, armor restrictions, ranges, and consumable tools are difficult to infer and can affect PvP or item loss.' -Brief 'Add a concise mechanics reference for ordinary search, hiding/stealth restrictions, reveal interactions, tracking, and specialized detection tools.' -Spoiler 'Explain mechanics and tool categories without publishing hidden locations or encounter solutions.' -Test 'Use ordinary and edge-case skill values, armor, ranges, tools, and abilities in staging; compare every claim to current source.' -ExternalDocs 'docs/wiki/Search_System.md;docs/wiki/Searching_Hiding_Stealth.md'

Add-CuratedCapability -Rows $auditRows -HistoryBySystem $history.BySystem -Slug 'boss-taming-restrictions' -System 'Mobiles:Bosses' -Capability 'Boss and prison-scaled creature taming restrictions' -Audience 'Player' -ActivationState 'ActiveBySource' -EntryPath 'Attempt Animal Taming on boss-class or prison-scaled creatures and receive failure feedback.' -PlacementEvidence 'Representative boss and shared prison scaling source are runtime-visible; live spawn availability is not asserted.' -Commands '' -Files @('Data/Scripts/Custom/Mobiles/Dragons/DarkDragonAvatar.cs','Data/Scripts/Custom/Mobiles/Dragons/DragonHydra.cs','Data/Scripts/Quests/Summon/SummonPrison.cs') -Surfaces 'Animal Taming failure feedback; no durable rule summary was found.' -Reachability 'RuntimeUnknown' -Coverage 'Partial' -Decision 'RefreshExisting' -Surface 'ContextualFeedback' -Priority 'P2' -Risk 'Medium: players may spend substantial time preparing for a tame that the system intentionally forbids.' -Brief 'Ensure the taming attempt gives a clear non-spoiling reason or category rule; optionally mention boss-class exclusions in the taming Library reference.' -Spoiler 'Do not enumerate every boss or reveal spawn locations.' -Test 'Attempt taming on representative boss, scaled prison, ordinary wild, and existing previously tamed creatures; confirm feedback and compatibility.'

Add-CuratedCapability -Rows $auditRows -HistoryBySystem $history.BySystem -Slug 'disabled-turn-combat-test-center' -System 'Configuration:Gates' -Capability 'Disabled turn-based combat and Test Center player controls' -Audience 'Player' -ActivationState 'DisabledByConfig' -EntryPath 'No player entry should be advertised while the checked-in gates are disabled.' -PlacementEvidence 'Checked-in configuration disables turn-based combat; Test Center defaults off in current settings source.' -Commands '' -Files @('Data/TurnBasedCombat/TurnBasedCombat.cfg','Data/Scripts/System/Misc/Settings.cs') -Surfaces 'Any legacy or external references are research evidence only.' -Reachability 'Disabled' -Coverage 'NotApplicable' -Decision 'NoDocumentationNeeded' -Surface 'Help' -Priority 'P4' -Risk 'Low while disabled; misleading Help would become a medium-risk availability error.' -Brief 'Do not add player instructions while disabled. If enabled later, require a dedicated source and staging documentation pass before release.' -Spoiler 'Not applicable.' -Test 'Confirm gates are disabled and no reachable Help page advertises unavailable controls; repeat the audit before enabling.' -Notes 'Explicit exclusion: current checked-in gates disable the feature.'

$historicalExamples = @($history.HistoricalPaths)
$auditRows.Add((New-AuditRow -Id 'IGD-HIST-post-baseline-removed-renamed' -RowKind 'HistoricalDrift' -System 'HistoricalDrift' -Capability 'Post-baseline changed paths absent from current HEAD' -Audience 'Mixed' -ActivationState 'Historical' -PlayerEntryPath 'No current player entry path is asserted.' -AcquisitionPlacementEvidence 'Absent from the current filesystem; paths may have been renamed, removed, reverted, generated, or deleted.' -PlayerCommands '' -SourceFiles '' -PostBaselineChanges "$($history.HistoricalPaths.Count) non-current path(s) observed in post-baseline history; all are listed in Notes." -ExistingInGameSurfaces 'None counted from historical-only paths.' -Reachability 'RuntimeUnknown' -Coverage 'NotApplicable' -Decision 'NoDocumentationNeeded' -RecommendedSurface 'ContextualFeedback' -Priority 'P4' -PlayerRisk 'Avoids advertising removed or reverted work as a current feature.' -SourceEvidence "git log --name-only $resolvedBaseline..$headHash over runtime scripts and checked-in player data." -ContentBrief 'Retain historical drift as review evidence only; current capabilities must be supported by current files.' -SpoilerBoundary 'Not applicable.' -AcceptanceTest 'Confirm every historical-only candidate is absent from current source or represented by its renamed/current owning-system row.' -ExternalDocs '' -Notes (Join-Unique $historicalExamples))) | Out-Null

$orderedRows = @($auditRows.ToArray() | Sort-Object @{ Expression = { switch ($_.RowKind) { 'SystemBoundary' { 1 } 'Capability' { 2 } 'CanonicalTopic' { 3 } default { 4 } } } }, System,Capability,Id)

$capabilityActions = @($orderedRows | Where-Object { $_.RowKind -eq 'Capability' -and $_.Decision -in @('RefreshExisting', 'Incorporate', 'NeedsRuntimeEvidence') })
$coveredSystems = @($capabilityActions | Select-Object -ExpandProperty System -Unique)
$coveredExternalDocs = @($capabilityActions | ForEach-Object { $_.ExternalDocs -split ';' } | Where-Object { -not [string]::IsNullOrWhiteSpace($_) } | Sort-Object -Unique)
$actionRows = @(
    $orderedRows |
        Where-Object {
            if ($_.Decision -notin @('RefreshExisting', 'Incorporate', 'NeedsRuntimeEvidence'))
            {
                return $false
            }

            if ($_.RowKind -eq 'SystemBoundary' -and $_.System -in $coveredSystems)
            {
                return $false
            }

            if ($_.RowKind -eq 'CanonicalTopic' -and $_.ExternalDocs -in $coveredExternalDocs)
            {
                return $false
            }

            return $true
        } |
        Sort-Object Priority,System,Capability,Id
)
$backlogRows = New-Object System.Collections.Generic.List[object]
$index = 0
foreach ($row in $actionRows)
{
    $index++
    $backlogRows.Add([pscustomobject][ordered]@{
        Id = ('IGDB-{0:D4}' -f $index)
        AuditId = $row.Id
        Priority = $row.Priority
        Status = if ($row.Decision -eq 'NeedsRuntimeEvidence') { 'Blocked' } else { 'Ready' }
        System = $row.System
        Capability = $row.Capability
        Decision = $row.Decision
        RecommendedSurface = $row.RecommendedSurface
        Files = $row.SourceFiles
        Evidence = $row.SourceEvidence
        Risk = $row.PlayerRisk
        ContentBrief = $row.ContentBrief
        SpoilerBoundary = $row.SpoilerBoundary
        AcceptanceTest = $row.AcceptanceTest
        RuntimeOwnerCheck = if ($row.Reachability -eq 'RuntimeUnknown' -or $row.Decision -eq 'NeedsRuntimeEvidence') { 'Required: ordinary-player staging placement/accessibility evidence; no world save is present in this checkout.' } else { 'Required for final player-visible implementation acceptance.' }
        Notes = $row.Notes
    }) | Out-Null
}

$orderedBacklogRows = @($backlogRows.ToArray())
$styleGuide = Get-StyleGuide -HeadHash $headHash -BaselineHash $resolvedBaseline -LoreBookCount $loreBooks.Count -StaticLibraryCount $staticEntries.Count
$summary = Get-Summary -Rows $orderedRows -BacklogRows $orderedBacklogRows -HeadHash $headHash -BaselineHash $resolvedBaseline -BaselineDate $baselineDate -SourceCount $sourceFiles.Count -SystemCount $systems.Count -CanonicalCount $canonicalDocs.Count -CommandCount $playerCommands.Count -XmlCount $relevantXmlPaths.Count -LoreCount $loreBooks.Count -StaticLibraryCount $staticEntries.Count -ChangedCurrentCount $history.ChangedCurrentPaths.Count -HistoricalPathCount $history.HistoricalPaths.Count

Assert-Audit -Rows $orderedRows -BacklogRows $orderedBacklogRows -Systems $systems -CanonicalDocs $canonicalDocs -PlayerCommands $playerCommands -ChangedCurrentPaths $history.ChangedCurrentPaths -SystemByPath $systemByPath -XmlPaths $relevantXmlPaths -LoreBooks $loreBooks -StaticEntries $staticEntries

$outputs = [ordered]@{
    $auditFileName = ConvertTo-DeterministicCsv $orderedRows
    $backlogFileName = ConvertTo-DeterministicCsv $orderedBacklogRows
    $styleGuideFileName = $styleGuide
    $summaryFileName = $summary
}

if ($ValidateOnly)
{
    $drift = New-Object System.Collections.Generic.List[string]
    foreach ($fileName in $outputs.Keys)
    {
        $path = Join-Path $OutputDir $fileName
        if (-not (Test-Path -LiteralPath $path -PathType Leaf))
        {
            $drift.Add("Missing generated output: $fileName") | Out-Null
            continue
        }

        $expected = (($outputs[$fileName] -replace "`r`n", "`n") -replace "`r", "`n")
        if (-not $expected.EndsWith("`n")) { $expected += "`n" }
        $actual = (([System.IO.File]::ReadAllText($path) -replace "`r`n", "`n") -replace "`r", "`n")
        if ($actual -cne $expected)
        {
            $drift.Add("Generated output differs: $fileName") | Out-Null
        }
    }

    if ($drift.Count -gt 0)
    {
        throw ("Validation-only output comparison failed:`n- " + ($drift -join "`n- "))
    }

    Write-Host "In-game documentation audit validation passed: $($orderedRows.Count) audit rows; $($orderedBacklogRows.Count) backlog rows; $($systems.Count) systems; $($canonicalDocs.Count) canonical topics; $($playerCommands.Count) Player commands."
    exit 0
}

foreach ($fileName in $outputs.Keys)
{
    Write-Utf8Lf -Path (Join-Path $OutputDir $fileName) -Content $outputs[$fileName]
}

Write-Host "Generated in-game documentation audit: $($orderedRows.Count) audit rows; $($orderedBacklogRows.Count) backlog rows; $($systems.Count) systems; $($canonicalDocs.Count) canonical topics; $($playerCommands.Count) Player commands."
