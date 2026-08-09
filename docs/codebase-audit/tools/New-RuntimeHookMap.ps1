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

function Get-HookType
{
    param([string]$Marker)

    switch ($Marker)
    {
        'Initialize' { return 'Initialize' }
        'CommandSystem.Register' { return 'Command' }
        'EventSink' { return 'Event' }
        'PacketHandlers.Register' { return 'Packet' }
        'Timer.DelayCall' { return 'Timer' }
        'CustomTimerSubclass' { return 'Timer' }
        'OnSpeech' { return 'Speech' }
        'OnMovement' { return 'Movement' }
        'OnLogin' { return 'Login' }
        'OnLogout' { return 'Logout' }
        'WorldSave' { return 'WorldSave' }
        'WorldLoad' { return 'WorldLoad' }
        'OnResponse' { return 'Gump' }
        'SendGump' { return 'Gump' }
        'RegionOverride' { return 'Region' }
        default { return $Marker }
    }
}

function Get-Risk
{
    param(
        [string]$HookType,
        [string]$Access,
        [string]$Guards
    )

    if ($HookType -eq 'Packet')
    {
        return 'Critical'
    }

    if ($HookType -in @('Event', 'WorldSave', 'WorldLoad', 'Movement', 'Speech', 'Region'))
    {
        return 'High'
    }

    if ($HookType -eq 'Command' -and $Access -match 'Administrator|Owner|Seer|GameMaster|GM|Counselor')
    {
        return 'High'
    }

    if ($HookType -eq 'Gump' -and ($Guards -notmatch 'BoundsGuard' -or $Guards -notmatch 'NullGuard'))
    {
        return 'High'
    }

    if ($HookType -in @('Command', 'Timer', 'Gump', 'Initialize', 'Login', 'Logout'))
    {
        return 'Medium'
    }

    return 'Low'
}

function Get-Trigger
{
    param(
        [string]$HookType,
        [object]$CommandRow
    )

    if ($HookType -eq 'Command')
    {
        if ($CommandRow -ne $null -and -not [string]::IsNullOrWhiteSpace($CommandRow.Command))
        {
            return "Command: $($CommandRow.Command)"
        }

        return 'Command invocation'
    }

    switch ($HookType)
    {
        'Initialize' { 'Shard script initialization' }
        'Event' { 'EventSink dispatch' }
        'Packet' { 'Client packet' }
        'Timer' { 'Timer tick or delayed callback' }
        'Speech' { 'Mobile speech' }
        'Movement' { 'Mobile movement' }
        'Login' { 'Login event' }
        'Logout' { 'Logout event' }
        'WorldSave' { 'World save event' }
        'WorldLoad' { 'World load event' }
        'Gump' { 'Gump send or response' }
        'Region' { 'Region override or policy check' }
        default { $HookType }
    }
}

function Get-Handler
{
    param(
        [string]$HookType,
        [string]$Evidence,
        [object]$CommandRow
    )

    if ($HookType -eq 'Command' -and $CommandRow -ne $null -and -not [string]::IsNullOrWhiteSpace($CommandRow.Handler))
    {
        return $CommandRow.Handler
    }

    if ($HookType -eq 'Gump' -and $Evidence -match 'OnResponse')
    {
        return 'OnResponse'
    }

    if ($Evidence -match 'new\s+[A-Za-z0-9_]+\s*\(\s*([A-Za-z0-9_.]+)\s*\)')
    {
        return $Matches[1]
    }

    if ($Evidence -match '\+=\s*([A-Za-z0-9_.]+)')
    {
        return $Matches[1]
    }

    return ''
}

function Get-GuardSummary
{
    param([string]$Text)

    $guards = New-Object System.Collections.Generic.List[string]

    if ($Text -match '==\s*null|!=\s*null|null\s*\)')
    {
        $guards.Add('NullGuard') | Out-Null
    }

    if ($Text -match '\bDeleted\b')
    {
        $guards.Add('DeletedGuard') | Out-Null
    }

    if ($Text -match '\bMap\b|Map\.Internal')
    {
        $guards.Add('MapGuard') | Out-Null
    }

    if ($Text -match 'InRange|GetDistance|Range|Utility\.InRange')
    {
        $guards.Add('RangeGuard') | Out-Null
    }

    if ($Text -match 'AccessLevel|CheckAccess|AccessCheck|from\.AccessLevel')
    {
        $guards.Add('AccessGuard') | Out-Null
    }

    if ($Text -match 'ButtonID|TextRelay|TextEntry|Entries|Count|Length|switch\s*\(')
    {
        $guards.Add('BoundsGuard') | Out-Null
    }

    if ($Text -match 'NetState|state\.Mobile|sender\.Mobile|Mobile from')
    {
        $guards.Add('StateGuard') | Out-Null
    }

    if ($Text -match 'PacketReader|reader\.Read')
    {
        $guards.Add('PacketReadGuard') | Out-Null
    }

    if ($guards.Count -eq 0)
    {
        return 'NeedsSourceReview'
    }

    return (($guards | Sort-Object -Unique) -join ';')
}

function Get-FileText
{
    param(
        [string]$RepoRoot,
        [string]$RepoPath,
        [hashtable]$Cache
    )

    if ($Cache.ContainsKey($RepoPath))
    {
        return $Cache[$RepoPath]
    }

    $fullPath = Join-Path $RepoRoot ($RepoPath -replace '/', '\')
    $text = ''

    if (Test-Path -LiteralPath $fullPath -PathType Leaf)
    {
        $text = Get-Content -Raw -LiteralPath $fullPath
    }

    $Cache[$RepoPath] = $text
    return $text
}

New-Item -ItemType Directory -Force -Path $OutputDir | Out-Null

$runtimeMarkers = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'phase-01-runtime-marker-inventory.csv'))
$commands = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'phase-01-command-registration-inventory.csv'))
$inventory = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'cross-tree-runtime-inventory.csv'))
$ownerMap = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'system-owner-map.csv'))

$inventoryByFile = @{}
$ownerByFile = @{}
$commandByFileLine = @{}
$textCache = @{}

foreach ($row in $inventory)
{
    $inventoryByFile[$row.Path] = $row
}

foreach ($row in $ownerMap)
{
    if (-not $ownerByFile.ContainsKey($row.File))
    {
        $ownerByFile[$row.File] = $row.System
    }
}

foreach ($row in $commands)
{
    $commandByFileLine["$($row.File):$($row.Line)"] = $row
}

$hookRows = New-Object System.Collections.Generic.List[object]

foreach ($marker in $runtimeMarkers)
{
    $file = $marker.File
    $inventoryRow = if ($inventoryByFile.ContainsKey($file)) { $inventoryByFile[$file] } else { $null }
    $system = if ($ownerByFile.ContainsKey($file)) { $ownerByFile[$file] } elseif ($inventoryRow -ne $null) { $inventoryRow.System } else { 'Unknown' }
    $hookType = Get-HookType $marker.Marker
    $commandRow = if ($commandByFileLine.ContainsKey("$($file):$($marker.Line)")) { $commandByFileLine["$($file):$($marker.Line)"] } else { $null }
    $access = if ($commandRow -ne $null -and -not [string]::IsNullOrWhiteSpace($commandRow.AccessLevel)) { $commandRow.AccessLevel } elseif ($hookType -eq 'Command') { 'Unknown' } elseif ($hookType -in @('Event', 'Packet', 'Timer', 'WorldSave', 'WorldLoad', 'Movement', 'Speech', 'Region', 'Initialize')) { 'GlobalOrInternal' } else { 'Internal' }
    $text = Get-FileText -RepoRoot $RepoRoot -RepoPath $file -Cache $textCache
    $guards = Get-GuardSummary $text
    $risk = Get-Risk -HookType $hookType -Access $access -Guards $guards
    $handler = Get-Handler -HookType $hookType -Evidence $marker.Notes -CommandRow $commandRow
    $commentNeeded = if ($risk -in @('Critical', 'High') -and $hookType -in @('Packet', 'Event', 'WorldSave', 'WorldLoad', 'Movement', 'Speech', 'Region')) { 'Yes' } else { 'ReviewLater' }

    $hookRows.Add([pscustomobject]@{
        System = $system
        File = $file
        HookType = $hookType
        Marker = $marker.Marker
        Line = $marker.Line
        Registration = $marker.Notes
        Handler = $handler
        Access = $access
        Trigger = Get-Trigger -HookType $hookType -CommandRow $commandRow
        Guards = $guards
        Risk = $risk
        Docs = if ($system -ne 'Unknown') { "docs/codebase-audit/outputs/system-cards/$(($system.ToLowerInvariant() -replace '[^a-z0-9]+', '-').Trim('-')).md" } else { '' }
        CommentNeeded = $commentNeeded
    }) | Out-Null
}

$globalRows = @($hookRows | Where-Object { $_.Risk -in @('Critical', 'High') -or $_.Access -eq 'GlobalOrInternal' })
$commandRows = foreach ($row in $commands)
{
    $hook = @($hookRows | Where-Object { $_.File -eq $row.File -and $_.Line -eq $row.Line } | Select-Object -First 1)

    [pscustomobject]@{
        Command = $row.Command
        AccessLevel = $row.AccessLevel
        Handler = $row.Handler
        File = $row.File
        Line = $row.Line
        System = if ($hook.Count -gt 0) { $hook[0].System } else { 'Unknown' }
        Guards = if ($hook.Count -gt 0) { $hook[0].Guards } else { 'NeedsSourceReview' }
        Risk = if ($hook.Count -gt 0) { $hook[0].Risk } else { 'Medium' }
        DuplicateCommandCount = @($commands | Where-Object { $_.Command -eq $row.Command -and -not [string]::IsNullOrWhiteSpace($_.Command) }).Count
        RegistrationLine = $row.RegistrationLine
    }
}

$packetRows = foreach ($row in ($hookRows | Where-Object { $_.HookType -eq 'Packet' }))
{
    $packetId = ''
    $length = ''

    if ($row.Registration -match 'Register\s*\(\s*([^,]+)\s*,\s*([^,]+)')
    {
        $packetId = $Matches[1].Trim()
        $length = $Matches[2].Trim()
    }

    [pscustomobject]@{
        System = $row.System
        File = $row.File
        Line = $row.Line
        PacketId = $packetId
        Length = $length
        Handler = $row.Handler
        Guards = $row.Guards
        Risk = $row.Risk
        Evidence = $row.Registration
    }
}

$gumpRows = foreach ($row in ($hookRows | Where-Object { $_.HookType -eq 'Gump' }))
{
    [pscustomobject]@{
        System = $row.System
        File = $row.File
        Line = $row.Line
        GumpHook = $row.Marker
        Guards = $row.Guards
        Risk = $row.Risk
        Evidence = $row.Registration
        NeedsBoundsReview = if ($row.Guards -match 'BoundsGuard') { 'No' } else { 'Yes' }
        NeedsNullStateReview = if ($row.Guards -match 'NullGuard|StateGuard') { 'No' } else { 'Yes' }
    }
}

$timerWorldRows = @($hookRows | Where-Object { $_.HookType -in @('Timer', 'WorldSave', 'WorldLoad') })

Export-AuditCsv -Rows $hookRows -FileName 'phase-05-runtime-hook-map.csv' | Out-Null
Export-AuditCsv -Rows $hookRows -FileName 'runtime-hook-map.csv' | Out-Null
Export-AuditCsv -Rows $globalRows -FileName 'phase-05-global-hook-risk-list.csv' | Out-Null
Export-AuditCsv -Rows $commandRows -FileName 'phase-05-command-surface-register.csv' | Out-Null
Export-AuditCsv -Rows $packetRows -FileName 'phase-05-packet-handler-register.csv' | Out-Null
Export-AuditCsv -Rows $gumpRows -FileName 'phase-05-gump-response-risk-register.csv' | Out-Null
Export-AuditCsv -Rows $timerWorldRows -FileName 'phase-05-timer-world-hook-register.csv' | Out-Null

$summaryPath = Join-Path $OutputDir 'phase-05-summary.md'
$summaryLines = @(
    '# Phase 5 Runtime Hook Map Summary',
    '',
    "Generated: $(Get-Date -Format o)",
    '',
    '## Required Inputs',
    '',
    '| Input | Status |',
    '| --- | --- |',
    '| Runtime marker scans | Present: `phase-01-runtime-marker-inventory.csv` |',
    '| CrossTreeRuntimeInventory | Present: `cross-tree-runtime-inventory.csv` |',
    '| System Cards | Present: `outputs/system-cards/` and `system-owner-map.csv` |',
    '| Project Truth Register | Present: `project-truth-register.csv` |',
    '',
    '## Generated Outputs',
    '',
    '| Output | Rows | Purpose |',
    '| --- | ---: | --- |',
    "| ``runtime-hook-map.csv`` | $($hookRows.Count) | Canonical runtime hook map. |",
    "| ``phase-05-runtime-hook-map.csv`` | $($hookRows.Count) | Phase-scoped hook map. |",
    "| ``phase-05-global-hook-risk-list.csv`` | $($globalRows.Count) | Global, high-risk, and critical hook rows. |",
    "| ``phase-05-command-surface-register.csv`` | $($commandRows.Count) | Command registration surface with access and duplicate counts. |",
    "| ``phase-05-packet-handler-register.csv`` | $($packetRows.Count) | Packet handler rows treated as critical network entry points. |",
    "| ``phase-05-gump-response-risk-register.csv`` | $($gumpRows.Count) | Gump send/response rows with guard-review flags. |",
    "| ``phase-05-timer-world-hook-register.csv`` | $($timerWorldRows.Count) | Timer and world save/load hook rows. |",
    '',
    '## Hook Type Counts',
    '',
    '| Hook Type | Count |',
    '| --- | ---: |'
)

foreach ($group in ($hookRows | Group-Object HookType | Sort-Object Name))
{
    $summaryLines += "| $($group.Name) | $($group.Count) |"
}

$summaryLines += @(
    '',
    '## Risk Counts',
    '',
    '| Risk | Count |',
    '| --- | ---: |'
)

foreach ($group in ($hookRows | Group-Object Risk | Sort-Object Name))
{
    $summaryLines += "| $($group.Name) | $($group.Count) |"
}

$summaryLines += @(
    '',
    '## Exit Criteria',
    '',
    '- Runtime triggers are mapped to system, file, hook type, trigger, access, guard evidence, risk, and documentation target.',
    '- Commands, packet handlers, gump paths, global hooks, timers, and world hooks have focused registers.',
    '- Guard fields are conservative marker scans; rows marked `NeedsSourceReview` require manual code review before repair.',
    '- Source comments are deferred to Phase 11 comment targets rather than added in this phase.'
)

Set-Content -LiteralPath $summaryPath -Value $summaryLines -Encoding UTF8

[pscustomobject]@{
    HookRows = $hookRows.Count
    GlobalRiskRows = $globalRows.Count
    CommandRows = $commandRows.Count
    PacketRows = $packetRows.Count
    GumpRows = $gumpRows.Count
    TimerWorldRows = $timerWorldRows.Count
}
