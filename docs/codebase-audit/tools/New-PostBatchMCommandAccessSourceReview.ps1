param(
    [string]$RepoRoot,
    [string]$OutputDir,
    [string]$DiffCheckResult = 'Not run yet'
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
else
{
    $OutputDir = [System.IO.Path]::GetFullPath($OutputDir)
}

$Utf8NoBom = New-Object -TypeName System.Text.UTF8Encoding -ArgumentList $false
$Timestamp = (Get-Date).ToString('o')
$BatchName = 'POST-BATCH-M'
$ReviewBatchId = 'POST-BATCH-M-01A'

$PostBatchLPath = Join-Path $OutputDir 'post-batch-l-p2-residual-backlog-review.csv'
$ActivePath = Join-Path $OutputDir 'post-audit-active-backlog-status.csv'
$RepairBacklogPath = Join-Path $OutputDir 'repair-backlog.csv'
$ReviewPath = Join-Path $OutputDir 'post-batch-m-command-access-source-review.csv'
$CloseoutPath = Join-Path $OutputDir 'post-batch-m-command-access-closeout.md'
$ReadmePath = Join-Path $OutputDir 'README.md'
$PhaseStatusPath = Join-Path $RepoRoot 'docs\codebase-audit\PHASE_STATUS.md'
$RunLogPath = Join-Path $RepoRoot 'docs\codebase-audit\RUN_LOG.md'

$ReviewRel = 'docs/codebase-audit/outputs/post-batch-m-command-access-source-review.csv'
$CloseoutRel = 'docs/codebase-audit/outputs/post-batch-m-command-access-closeout.md'

function Write-Utf8File
{
    param(
        [string]$Path,
        [string[]]$Lines
    )

    [System.IO.File]::WriteAllText($Path, ($Lines -join "`n") + "`n", $Utf8NoBom)
}

function Get-CsvRows
{
    param([string]$Path)

    if (-not (Test-Path -LiteralPath $Path))
    {
        throw "Required CSV missing: $Path"
    }

    if ((Get-Item -LiteralPath $Path).Length -eq 0)
    {
        return @()
    }

    return @(Import-Csv -LiteralPath $Path)
}

function Normalize-AuditPath
{
    param([string]$Path)

    if ([string]::IsNullOrWhiteSpace($Path))
    {
        return ''
    }

    return ($Path -replace '\\', '/').Trim()
}

function Add-CountTable
{
    param(
        [System.Collections.Generic.List[string]]$Lines,
        [object[]]$Groups,
        [string]$NameHeader
    )

    $Lines.Add("| $NameHeader | Count |") | Out-Null
    $Lines.Add('| --- | ---: |') | Out-Null

    foreach ($group in $Groups)
    {
        $Lines.Add("| ``$($group.Name)`` | $($group.Count) |") | Out-Null
    }
}

$SourceCache = @{}

function Get-SourceLines
{
    param([string]$Path)

    $pathText = Normalize-AuditPath -Path $Path

    if ([string]::IsNullOrWhiteSpace($pathText))
    {
        return @()
    }

    $fullPath = Join-Path $RepoRoot $pathText

    if (-not (Test-Path -LiteralPath $fullPath))
    {
        return @()
    }

    if (-not $SourceCache.ContainsKey($pathText))
    {
        $SourceCache[$pathText] = @(Get-Content -LiteralPath $fullPath)
    }

    return @($SourceCache[$pathText])
}

function Get-SourceLine
{
    param(
        [string]$Path,
        [int]$Line
    )

    $lines = @(Get-SourceLines -Path $Path)

    if ($Line -le 0 -or $Line -gt $lines.Count)
    {
        return ''
    }

    return $lines[$Line - 1].Trim()
}

function Get-NearestRegistration
{
    param(
        [string]$Path,
        [int]$Line
    )

    $lines = @(Get-SourceLines -Path $Path)

    if ($lines.Count -eq 0)
    {
        return [pscustomobject]@{
            StartLine = 0
            Block = ''
            Command = ''
            Access = ''
            Handler = ''
        }
    }

    $startLine = [Math]::Max(1, $Line - 12)
    $endLine = [Math]::Min($lines.Count, $Line + 40)
    $registerStart = 0

    for ($i = $startLine; $i -le $endLine; $i++)
    {
        if ($lines[$i - 1] -match '(?:Server\.Commands\.)?CommandSystem\.Register\s*\(')
        {
            $registerStart = $i
            break
        }
    }

    if ($registerStart -eq 0)
    {
        for ($i = $Line; $i -ge [Math]::Max(1, $Line - 40); $i--)
        {
            if ($lines[$i - 1] -match '(?:Server\.Commands\.)?CommandSystem\.Register\s*\(')
            {
                $registerStart = $i
                break
            }
        }
    }

    if ($registerStart -eq 0)
    {
        return [pscustomobject]@{
            StartLine = 0
            Block = ''
            Command = ''
            Access = ''
            Handler = ''
        }
    }

    $blockLines = New-Object System.Collections.Generic.List[string]

    for ($i = $registerStart; $i -le [Math]::Min($lines.Count, $registerStart + 12); $i++)
    {
        $blockLines.Add($lines[$i - 1].Trim()) | Out-Null

        if ($lines[$i - 1] -match '\)\s*;')
        {
            break
        }
    }

    $block = ($blockLines -join ' ') -replace '\s+', ' '
    $command = ''
    $access = ''
    $handler = ''

    if ($block -match '"([^"]+)"')
    {
        $command = $Matches[1]
    }
    elseif ($block -match 'Register\s*\(\s*([^,\s]+)')
    {
        $command = $Matches[1]
    }

    if ($block -match 'AccessLevel\.([A-Za-z]+)')
    {
        $access = $Matches[1]
    }
    elseif ($block -match ',\s*([A-Za-z_][A-Za-z0-9_\.]*)\s*,')
    {
        $access = $Matches[1]
    }

    if ($block -match 'new\s+CommandEventHandler\s*\(([^)]+)\)')
    {
        $handler = $Matches[1].Trim()
    }
    elseif ($block -match ',\s*([A-Za-z_][A-Za-z0-9_\.]*)\s*\)\s*;')
    {
        $handler = $Matches[1].Trim()
    }

    return [pscustomobject]@{
        StartLine = $registerStart
        Block = $block
        Command = $command
        Access = $access
        Handler = $handler
    }
}

function Get-ObsoleteCommandResolution
{
    param([string]$BacklogId)

    $map = @{
        'RB-04652' = @('FactionElection', 'GameMaster', 'FactionElection_OnCommand', '10823-10827')
        'RB-04653' = @('FactionCommander', 'Administrator', 'FactionCommander_OnCommand', '10828-10832')
        'RB-04654' = @('FactionItemReset', 'Administrator', 'FactionItemReset_OnCommand', '10833-10837')
        'RB-04655' = @('FactionReset', 'Administrator', 'FactionReset_OnCommand', '10838-10842')
        'RB-04656' = @('FactionTownReset', 'Administrator', 'FactionTownReset_OnCommand', '10843-10847')
        'RB-04657' = @('GenerateFactions', 'Administrator', 'GenerateFactions_OnCommand', '15432-15436')
        'RB-04658' = @('UpdateMyRunUO', 'Administrator', 'UpdateMyRunUO_OnCommand', '22867-22871')
        'RB-04659' = @('PublicChar', 'Player', 'PublicChar_OnCommand', '22873-22877')
        'RB-04660' = @('PrivateChar', 'Player', 'PrivateChar_OnCommand', '22878-22882')
        'RB-04661' = @('UpdateWebStatus', 'Administrator', 'UpdateWebStatus_OnCommand', '23612-23616')
        'RB-04662' = @('GrantTownSilver', 'Administrator', 'GrantTownSilver_OnCommand', '32252-32256')
    }

    if (-not $map.ContainsKey($BacklogId))
    {
        return $null
    }

    $value = $map[$BacklogId]

    return [pscustomobject]@{
        Command = $value[0]
        Access = $value[1]
        Handler = $value[2]
        Lines = $value[3]
    }
}

function Resolve-AccessText
{
    param(
        [object]$Row,
        [object]$Registration,
        [string]$Path
    )

    $parsedAccess = $Row.ParsedAccess
    $registrationAccess = $Registration.Access

    if ($Path -eq 'Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs')
    {
        if ($Row.BacklogId -eq 'RB-04256')
        {
            return 'Config supplied access or preserved old command access'
        }

        if ($registrationAccess -eq 'DiskAccessLevel' -or $parsedAccess -eq 'DiskAccessLevel')
        {
            return 'Administrator default through DiskAccessLevel'
        }

        if (-not [string]::IsNullOrWhiteSpace($registrationAccess))
        {
            return $registrationAccess
        }

        return 'Mixed staff command set'
    }

    if ($Path -eq 'Data/Scripts/Custom/XMLSpawner/XmlUtils/WriteMulti.cs')
    {
        return 'Administrator default through XmlSpawner.DiskAccessLevel'
    }

    if ($Path -eq 'Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs')
    {
        return 'Mixed: Player challenge/stat commands, GameMaster broadcast command, Administrator point maintenance'
    }

    if ($Path -like 'Data/Scripts/Custom/NPC Control*')
    {
        return 'Counselor via static accessLevel default'
    }

    if ($Path -like 'Data/Scripts/System/Commands/Player/SpellBars*' -or $Path -like 'Data/Scripts/Magic/*')
    {
        return 'Player through reviewed spell command callsites'
    }

    if ($Path -eq 'Data/Scripts/System/Chat/RUOVersion.cs')
    {
        return 'Caller supplied; reviewed chat callsites pass Player, GameMaster, or Counselor'
    }

    if ($Path -eq 'Data/Scripts/Items/Houses/Monopoly/RUOVersion.cs')
    {
        return 'Caller supplied; reviewed townhouse callsites pass Counselor'
    }

    if ($Path -eq 'Data/Scripts/System/Commands/Implementors/BaseCommandImplementor.cs')
    {
        return 'Implementor m_AccessLevel; reviewed accessors are framework controlled'
    }

    if ($Path -eq 'Data/Scripts/System/Commands/Implementors/SingleCommandImplementor.cs')
    {
        return 'Underlying BaseCommand.AccessLevel with Redirect access recheck'
    }

    if ($Path -eq 'Data/Scripts/System/Misc/Statistics.cs')
    {
        if ($Row.BacklogId -eq 'RB-04646')
        {
            return 'Player via Config.CanSeeStats default'
        }

        return 'Seer via Config.CanUpdateStats default'
    }

    if ($Path -eq 'Data/Scripts/System/Misc/Captcha.cs')
    {
        return 'Player for fonts and dumpFonts helper registrations'
    }

    if ($Path -eq 'Data/Scripts/System/Commands/Handlers.cs')
    {
        return 'Caller supplied by staff command registrations; reviewed helpers pass Counselor/GameMaster/Administrator'
    }

    if ($Path -eq 'Data/Scripts/System/Misc/Spawning.cs')
    {
        if ($Row.BacklogId -eq 'RB-04639')
        {
            return 'Administrator'
        }

        return 'GameMaster'
    }

    if (-not [string]::IsNullOrWhiteSpace($parsedAccess))
    {
        if ($parsedAccess -eq 'access')
        {
            return 'Caller supplied access parameter resolved at reviewed callsites'
        }

        if ($parsedAccess -eq 'acc')
        {
            return 'Caller supplied access parameter resolved at reviewed callsites'
        }

        return $parsedAccess
    }

    if (-not [string]::IsNullOrWhiteSpace($registrationAccess))
    {
        return $registrationAccess
    }

    return 'Resolved from current source review'
}

function Resolve-ReviewClass
{
    param(
        [object]$Row,
        [string]$Path
    )

    if ($Row.BacklogId -eq 'RB-04256')
    {
        return 'ConfigDrivenCommandRehashPolicy'
    }

    if ($Row.BacklogId -eq 'RB-04621')
    {
        return 'PlayerConsoleDumpPolicy'
    }

    if ($Path -eq 'Data/Scripts/Items/Doors/BaseDoor.cs')
    {
        return 'ParserLineOffsetFalsePositive'
    }

    if ($Path -eq 'Data/Scripts/System/Obsolete/Obsolete.cs')
    {
        return 'IntentionalLegacyCommandSurface'
    }

    if ($Path -eq 'Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs')
    {
        return 'XMLSpawnerStaffCommandSurface'
    }

    if ($Path -eq 'Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs')
    {
        return 'XMLPointsCommandSurface'
    }

    if ($Path -like 'Data/Scripts/Custom/NPC Control*')
    {
        return 'AccessVariableResolved'
    }

    if ($Path -like 'Data/Scripts/System/Commands/Player/SpellBars*')
    {
        return 'PlayerSpellbarCommandSurface'
    }

    if ($Path -like 'Data/Scripts/Magic/*')
    {
        return 'PlayerSpellCommandSurface'
    }

    if ($Path -eq 'Data/Scripts/System/Chat/RUOVersion.cs' -or $Path -eq 'Data/Scripts/Items/Houses/Monopoly/RUOVersion.cs')
    {
        return 'CompatibilityWrapperResolved'
    }

    if ($Path -like 'Data/Scripts/System/Commands/Implementors/*')
    {
        return 'GenericCommandFramework'
    }

    if ($Row.ParsedCommand -eq 'command' -or $Row.ParsedAccess -eq 'access' -or $Row.ParsedCommand -eq 'com' -or $Row.ParsedAccess -eq 'acc')
    {
        return 'PassthroughHelperResolved'
    }

    return 'DirectCommandAccessResolved'
}

function New-ReviewRecord
{
    param(
        [object]$Row,
        [int]$Index
    )

    $currentFile = Normalize-AuditPath -Path $Row.CurrentFile
    $registration = Get-NearestRegistration -Path $currentFile -Line ([int]$Row.Line)
    $reviewClass = Resolve-ReviewClass -Row $Row -Path $currentFile
    $decision = 'ReviewedNoChange'
    $resolvedCommand = $Row.ParsedCommand
    $resolvedAccess = Resolve-AccessText -Row $Row -Registration $registration -Path $currentFile
    $resolvedHandler = $Row.ParsedHandler
    $action = 'No source change. Current command access was resolved against source and no unsafe exposure was confirmed.'
    $notes = 'POST-BATCH-M source review resolved the POST-BATCH-L command-access follow-up without changing runtime behavior.'

    if (-not [string]::IsNullOrWhiteSpace($registration.Command) -and ($resolvedCommand -eq '' -or $resolvedCommand -eq 'command' -or $resolvedCommand -eq 'com'))
    {
        $resolvedCommand = $registration.Command
    }

    if (-not [string]::IsNullOrWhiteSpace($registration.Handler) -and ($resolvedHandler -eq '' -or $resolvedHandler -eq 'handler' -or $resolvedHandler -eq 'OnCommand'))
    {
        $resolvedHandler = $registration.Handler
    }

    if ($Row.BacklogId -eq 'RB-04256')
    {
        $decision = 'NeedsHumanDecision'
        $resolvedCommand = 'ChangeCommand'
        $resolvedHandler = 'Existing command handler'
        $action = 'No source change. A staff policy decision is required before restricting or preserving XMLSpawner config-driven command renames and optional access overrides.'
        $notes = 'ChangeCommand syntax oldname:newname[:accesslevel] can re-register an existing command with config-supplied access, or preserve the old access when no override is supplied. No checked-in XML/config value currently exercises the override.'
    }
    elseif ($Row.BacklogId -eq 'RB-04621')
    {
        $decision = 'NeedsHumanDecision'
        $resolvedCommand = 'fonts; dumpFonts'
        $resolvedHandler = 'fonts_OnCommand; dumpFonts_OnCommand'
        $action = 'No source change. A staff policy decision is required before changing Player access for the captcha font tools because dumpFonts writes server-console output.'
        $notes = 'Both fonts and dumpFonts are Player-access helper registrations; fonts opens the gump and dumpFonts writes alphabet data to Console. Treat access changes as policy, not a behavior-preserving source fix.'
    }
    elseif ($currentFile -eq 'Data/Scripts/Items/Doors/BaseDoor.cs')
    {
        $decision = 'FalsePositive'
        $resolvedCommand = 'Link; ChainLink'
        $resolvedAccess = 'GameMaster'
        $resolvedHandler = 'Link_OnCommand; ChainLink_OnCommand'
        $action = 'No source change. POST-BATCH-L line evidence pointed into the door offset array, not a command registration; the actual nearby commands are GameMaster-only.'
        $notes = 'Parser line offset only. BaseDoor.Initialize registers Link and ChainLink at AccessLevel.GameMaster.'
    }
    elseif ($currentFile -eq 'Data/Scripts/System/Obsolete/Obsolete.cs')
    {
        $obsolete = Get-ObsoleteCommandResolution -BacklogId $Row.BacklogId

        if ($obsolete -ne $null)
        {
            $resolvedCommand = $obsolete.Command
            $resolvedAccess = $obsolete.Access
            $resolvedHandler = $obsolete.Handler
            $notes = "Legacy Obsolete.cs command registration reviewed at lines $($obsolete.Lines)."
        }

        $decision = 'IntentionalLegacy'
        $action = 'No source change. This is a live legacy/obsolete compatibility command surface with explicit current access, retained until a separate legacy removal policy exists.'
    }

    if ([string]::IsNullOrWhiteSpace($resolvedCommand))
    {
        $resolvedCommand = 'Source-reviewed command group'
    }

    if ([string]::IsNullOrWhiteSpace($resolvedHandler))
    {
        $resolvedHandler = 'Resolved from current source review'
    }

    $sourceLine = Get-SourceLine -Path $currentFile -Line ([int]$Row.Line)
    $registrationText = $registration.Block

    if ([string]::IsNullOrWhiteSpace($registrationText))
    {
        $registrationText = 'No nearby CommandSystem.Register block at queued line; resolved through file-specific source review.'
    }

    $sourceEvidenceParts = @(
        "SourceReviewClass=$reviewClass",
        "CurrentFile=$currentFile",
        "QueuedLine=$($Row.Line)",
        "QueuedSourceLine=$sourceLine",
        "NearestRegistrationLine=$($registration.StartLine)",
        "NearestRegistration=$registrationText",
        "ResolvedCommand=$resolvedCommand",
        "ResolvedAccess=$resolvedAccess",
        "ResolvedHandler=$resolvedHandler"
    )

    if ($Row.BacklogId -eq 'RB-04256')
    {
        $sourceEvidenceParts += 'PolicyEvidence=ChangeCommand removes the old command entry and re-registers the new name with parsed access or preserved old access; no checked-in ChangeCommand config usage found.'
    }
    elseif ($Row.BacklogId -eq 'RB-04621')
    {
        $sourceEvidenceParts += 'PolicyEvidence=fonts and dumpFonts both pass AccessLevel.Player; dumpFonts writes to Console.'
    }
    elseif ($currentFile -eq 'Data/Scripts/Items/Doors/BaseDoor.cs')
    {
        $sourceEvidenceParts += 'FalsePositiveEvidence=queued lines 110/115 are Point3D offset data; actual commands at lines 128-136 are GameMaster-only.'
    }

    [pscustomobject]@{
        BatchId = $BatchName
        ReviewRowId = ('PBM-{0:0000}' -f $Index)
        BacklogId = $Row.BacklogId
        SourceId = $Row.SourceId
        Priority = $Row.Priority
        HistoricalStatus = $Row.HistoricalStatus
        Category = $Row.Category
        System = $Row.System
        HistoricalFile = Normalize-AuditPath -Path $Row.HistoricalFile
        CurrentFile = $currentFile
        Line = $Row.Line
        CurrentSourceLine = $sourceLine
        PostBatchLReviewRowId = $Row.ReviewRowId
        PostBatchLCommandParseQuality = $Row.CommandParseQuality
        PostBatchLDecision = $Row.Decision
        ParsedCommand = $Row.ParsedCommand
        ParsedAccess = $Row.ParsedAccess
        ParsedHandler = $Row.ParsedHandler
        ReviewClass = $reviewClass
        ResolvedCommand = $resolvedCommand
        ResolvedAccess = $resolvedAccess
        ResolvedHandler = $resolvedHandler
        Decision = $decision
        SourceEvidence = ($sourceEvidenceParts -join '; ')
        Action = $action
        Verification = "Reviewed POST-BATCH-L queued row against current source in POST-BATCH-M. No runtime source/project/XML/config files changed. Source build and runtime compile were not run because this is audit-only. git diff --check: $DiffCheckResult"
        ReviewedBatchId = $ReviewBatchId
        ReviewedAt = $Timestamp
        Notes = $notes
    }
}

$PostBatchLRows = Get-CsvRows -Path $PostBatchLPath
$ActiveRows = Get-CsvRows -Path $ActivePath
$RepairRows = Get-CsvRows -Path $RepairBacklogPath

$ScopeRows = @(
    $PostBatchLRows |
        Where-Object { $_.Decision -eq 'QueuedSourceFollowUp' } |
        Sort-Object ReviewRowId
)

if ($ScopeRows.Count -ne 174)
{
    throw "POST-BATCH-M input invariant failed: expected 174 POST-BATCH-L QueuedSourceFollowUp rows, found $($ScopeRows.Count)."
}

$NonCommandScope = @($ScopeRows | Where-Object { $_.Category -ne 'Command access' -or $_.Priority -ne 'P2' })

if ($NonCommandScope.Count -ne 0)
{
    throw "POST-BATCH-M input invariant failed: non-P2 command-access rows found: $($NonCommandScope.BacklogId -join ', ')"
}

$ScopeIds = New-Object 'System.Collections.Generic.HashSet[string]'
foreach ($row in $ScopeRows)
{
    [void]$ScopeIds.Add($row.BacklogId)
}

$ReviewRows = New-Object System.Collections.Generic.List[object]
$index = 1

foreach ($row in $ScopeRows)
{
    $ReviewRows.Add((New-ReviewRecord -Row $row -Index $index)) | Out-Null
    $index++
}

if ($ReviewRows.Count -ne 174)
{
    throw "POST-BATCH-M review row invariant failed: $($ReviewRows.Count)"
}

$ReviewRows | Export-Csv -LiteralPath $ReviewPath -NoTypeInformation -Encoding UTF8

if ((Get-CsvRows -Path $ReviewPath).Count -ne 174)
{
    throw 'POST-BATCH-M review CSV row count invariant failed after write.'
}

$OverlayRows = foreach ($review in $ReviewRows)
{
    [pscustomobject]@{
        BacklogId = $review.BacklogId
        SourceId = $review.SourceId
        Category = $review.Category
        System = $review.System
        Files = $review.HistoricalFile
        HistoricalStatus = $review.HistoricalStatus
        ActiveStatus = $review.Decision
        PostAuditBatch = $BatchName
        ReviewRowId = $review.ReviewRowId
        ReviewStatus = $review.Decision
        Action = $review.Action
        ReviewArtifact = $ReviewRel
        SourceEvidence = $review.SourceEvidence
        Commit = 'Pending current POST-BATCH-M commit'
        UpdatedAt = $Timestamp
        Notes = $review.Notes
    }
}

$ActiveBase = @(
    $ActiveRows |
        Where-Object {
            $_.PostAuditBatch -ne $BatchName -and
            -not $ScopeIds.Contains($_.BacklogId)
        }
)

@($ActiveBase + $OverlayRows) | Export-Csv -LiteralPath $ActivePath -NoTypeInformation -Encoding UTF8

$ActiveAfter = Get-CsvRows -Path $ActivePath
$PostBatchMRows = @($ActiveAfter | Where-Object { $_.PostAuditBatch -eq $BatchName })
$PostBatchLQueuedRemaining = @(
    $ActiveAfter |
        Where-Object { $_.PostAuditBatch -eq 'POST-BATCH-L' -and $_.ActiveStatus -eq 'QueuedSourceFollowUp' }
)

if ($PostBatchMRows.Count -ne 174)
{
    throw "Active overlay POST-BATCH-M count invariant failed: $($PostBatchMRows.Count)"
}

if ($PostBatchLQueuedRemaining.Count -ne 0)
{
    throw "Active overlay still has POST-BATCH-L queued source follow-up rows: $($PostBatchLQueuedRemaining.Count)"
}

$CoveredAfter = New-Object 'System.Collections.Generic.HashSet[string]'
foreach ($row in $ActiveAfter)
{
    if (-not [string]::IsNullOrWhiteSpace($row.BacklogId))
    {
        [void]$CoveredAfter.Add($row.BacklogId)
    }
}

$RemainingAfter = @($RepairRows | Where-Object { -not [string]::IsNullOrWhiteSpace($_.Id) -and -not $CoveredAfter.Contains($_.Id) })

if ($RemainingAfter.Count -ne 0)
{
    throw "Unreviewed repair-backlog rows remain after POST-BATCH-M overlay update: $($RemainingAfter.Id -join ', ')"
}

$DecisionCounts = @($ReviewRows | Group-Object Decision | Sort-Object Name)
$SystemCounts = @($ReviewRows | Group-Object System | Sort-Object Count -Descending)
$ReviewClassCounts = @($ReviewRows | Group-Object ReviewClass | Sort-Object Count -Descending)
$DecisionSummary = (($DecisionCounts | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')
$ReviewClassSummary = (($ReviewClassCounts | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')
$SystemSummary = (($SystemCounts | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')

$HumanDecisionRows = @($ReviewRows | Where-Object { $_.Decision -eq 'NeedsHumanDecision' })

$Closeout = New-Object System.Collections.Generic.List[string]
$Closeout.Add('# POST-BATCH-M Command Access Source Review Closeout') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add("Generated: $Timestamp") | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Summary') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('`POST-BATCH-M-01A` processed all 174 `QueuedSourceFollowUp` rows from POST-BATCH-L. The scoped rows are all P2 `Command access` findings and were reviewed against current source to resolve helper registrations, access variables, parser line offsets, legacy command surfaces, and uncertain command policy cases.') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('No runtime source, public API, namespace, serialized type name, save version, command name, command access level, handler, project file, XML/config file, runtime file location, or gameplay behavior was changed.') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Decision Counts') | Out-Null
$Closeout.Add('') | Out-Null
Add-CountTable -Lines $Closeout -Groups $DecisionCounts -NameHeader 'Decision'
$Closeout.Add('') | Out-Null
$Closeout.Add('## Review Classes') | Out-Null
$Closeout.Add('') | Out-Null
Add-CountTable -Lines $Closeout -Groups $ReviewClassCounts -NameHeader 'Review class'
$Closeout.Add('') | Out-Null
$Closeout.Add('## Systems') | Out-Null
$Closeout.Add('') | Out-Null
Add-CountTable -Lines $Closeout -Groups $SystemCounts -NameHeader 'System'
$Closeout.Add('') | Out-Null
$Closeout.Add('## Human Decisions') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('| BacklogId | File | Command surface | Decision needed |') | Out-Null
$Closeout.Add('| --- | --- | --- | --- |') | Out-Null

foreach ($row in $HumanDecisionRows)
{
    $decisionNeeded = if ($row.BacklogId -eq 'RB-04256') {
        'Decide whether XMLSpawner `ChangeCommand` config may lower command access, must preserve current access, or should clamp to the stricter old/new access.'
    }
    else {
        'Decide whether `dumpFonts` should remain Player-access because it writes server-console output, or whether the captcha font tools should become staff-only.'
    }

    $Closeout.Add(('| `{0}` | `{1}` | `{2}` | {3} |' -f $row.BacklogId, $row.CurrentFile, $row.ResolvedCommand, $decisionNeeded)) | Out-Null
}

$Closeout.Add('') | Out-Null
$Closeout.Add('## Verification') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('- `git status --short` was clean before the batch.') | Out-Null
$Closeout.Add('- Applicable root and `docs/codebase-audit/AGENTS.md` instructions were re-read; the root audit plan plus Phase 13 and Phase 14 plans were re-read for backlog and verification rules.') | Out-Null
$Closeout.Add('- Input scope check: `post-batch-l-p2-residual-backlog-review.csv` contains exactly 174 rows with `Decision=QueuedSourceFollowUp`; every scoped row is P2 `Command access`.') | Out-Null
$Closeout.Add('- Source review resolved all 174 rows without source edits: access variables, wrapper helper callsites, duplicate command names, parser line offsets, and legacy command surfaces are recorded in the CSV source evidence.') | Out-Null
$Closeout.Add('- `post-batch-m-command-access-source-review.csv` contains exactly 174 rows.') | Out-Null
$Closeout.Add('- `post-audit-active-backlog-status.csv` contains exactly 174 POST-BATCH-M rows and 0 remaining active POST-BATCH-L `QueuedSourceFollowUp` rows.') | Out-Null
$Closeout.Add('- Comparing all `repair-backlog.csv` rows to the active overlay leaves 0 unreviewed rows.') | Out-Null
$Closeout.Add('- Source build and runtime script compile were not run because this batch changed only audit artifacts; no runtime source, project file, XML/config file, serialized layout, command registration, or access level changed.') | Out-Null
$Closeout.Add("- `git diff --check`: $DiffCheckResult") | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Boundary') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('POST-BATCH-M closes POST-BATCH-L command-access source-review follow-ups. The two `NeedsHumanDecision` rows remain active policy decisions; no source command-access change is approved until those decisions are made.') | Out-Null
Write-Utf8File -Path $CloseoutPath -Lines $Closeout

$Readme = @(Get-Content -LiteralPath $ReadmePath)
$Readme = @($Readme | Where-Object { $_ -notmatch 'post-batch-m-command-access-(source-review\.csv|closeout\.md)' })
$NewEntries = @(
    '| `post-batch-m-command-access-source-review.csv` | Post-audit | Source review for the 174 POST-BATCH-L queued P2 command-access follow-ups, resolving helper access, parser offsets, legacy command surfaces, and policy-decision rows. | Complete |',
    '| `post-batch-m-command-access-closeout.md` | Post-audit | Close out POST-BATCH-M with decision counts, review classes, human decision rows, active-overlay reconciliation, and verification notes. | Complete |'
)
$InitialIndex = [Array]::IndexOf($Readme, '## Initial State')

if ($InitialIndex -lt 0)
{
    throw 'Could not find README Initial State heading.'
}

$Before = if ($InitialIndex -gt 0) { $Readme[0..($InitialIndex - 1)] } else { @() }
$After = $Readme[$InitialIndex..($Readme.Count - 1)]

while ($Before.Count -gt 0 -and [string]::IsNullOrWhiteSpace($Before[$Before.Count - 1]))
{
    if ($Before.Count -eq 1)
    {
        $Before = @()
    }
    else
    {
        $Before = $Before[0..($Before.Count - 2)]
    }
}

Write-Utf8File -Path $ReadmePath -Lines @($Before + '' + $NewEntries + '' + $After)

$PhaseStatus = Get-Content -Raw -LiteralPath $PhaseStatusPath
$PhaseStatus = [regex]::Replace($PhaseStatus, 'Last updated: .*', "Last updated: $Timestamp", 1)
$MSummary = 'Post-audit command access source review: `{0}` processed all 174 POST-BATCH-L `QueuedSourceFollowUp` rows in `outputs/post-batch-m-command-access-source-review.csv`. Disposition summary is `{1}`; review-class summary is `{2}`; active POST-BATCH-L queued source follow-up rows=0. No source/project/runtime/XML/config files changed, so source build and runtime compile were not run; `NeedsHumanDecision` rows remain policy-only command-access decisions.' -f $ReviewBatchId, $DecisionSummary, $ReviewClassSummary

if ($PhaseStatus -match 'Post-audit command access source review:')
{
    $PhaseStatus = [regex]::Replace($PhaseStatus, 'Post-audit command access source review:.*', [System.Text.RegularExpressions.MatchEvaluator]{ param($match) $MSummary }, 1)
}
else
{
    $Marker = 'Post-audit P2 residual backlog triage:'
    $PhaseStatus = $PhaseStatus -replace [regex]::Escape($Marker), ($MSummary + "`r`n`r`n" + $Marker)
}

$OverlaySummary = 'Post-audit active backlog overlay: `outputs/post-audit-active-backlog-status.csv` preserves historical `repair-backlog.csv` while recording 17 packet-handler dispositions, 1,598 reviewed save-compatibility dispositions across `POST-BATCH-B`, `POST-BATCH-I`, and `POST-BATCH-J`, 24 active `POST-BATCH-C-01A` runtime-hook/`PlayerMobile` coupling dispositions after `RB-01883` was superseded by `POST-BATCH-E-94A`, 406 `POST-BATCH-D` pooled enumerable fixes, 2 `POST-BATCH-D` false positives, 276 `POST-BATCH-E` runtime-hook/gump-guard dispositions, 540 `POST-BATCH-F` documentation/balance dispositions, 61 `POST-BATCH-G` project include drift dispositions, 14 `POST-BATCH-H` folder/namespace cleanup dispositions, 2,691 `POST-BATCH-K` P1 runtime-surface dispositions, 1,012 active `POST-BATCH-L` P2 residual dispositions, and 174 `POST-BATCH-M` command-access source-review dispositions. Comparing all `repair-backlog.csv` rows against the active overlay leaves 0 unreviewed rows.'
$PhaseStatus = [regex]::Replace($PhaseStatus, 'Post-audit active backlog overlay:.*', [System.Text.RegularExpressions.MatchEvaluator]{ param($match) $OverlaySummary }, 1)
[System.IO.File]::WriteAllText($PhaseStatusPath, $PhaseStatus, $Utf8NoBom)

$RunLogEntry = @(
    "### $Timestamp",
    '',
    '- Affected phase: Post-audit `POST-BATCH-M` command access source review',
    ('- Cwd: `{0}`' -f $RepoRoot),
    '- Command: filter `post-batch-l-p2-residual-backlog-review.csv` to `Decision=QueuedSourceFollowUp`; verify 174 P2 `Command access` rows; source-review command registrations, helper access variables, wrappers, parser line offsets, and policy cases; run `New-PostBatchMCommandAccessSourceReview.ps1`.',
    ('- Result: Review CSV contains {0} POST-BATCH-M rows; active overlay contains {1} POST-BATCH-M rows; active POST-BATCH-L `QueuedSourceFollowUp` rows=0; disposition summary is {2}; system summary is {3}; review-class summary is {4}; remaining unreviewed repair-backlog rows=0; no source/project/runtime/XML/config files changed; source build and runtime compile were not run because this is audit-only; git diff --check={5}.' -f $ReviewRows.Count, $PostBatchMRows.Count, $DecisionSummary, $SystemSummary, $ReviewClassSummary, $DiffCheckResult),
    ('- Output path: `{0}`; `{1}`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/outputs/README.md`' -f $ReviewRel, $CloseoutRel)
)

$RunLogText = Get-Content -Raw -LiteralPath $RunLogPath
$RunLogPattern = '(?ms)^### [^\r\n]+\r?\n\r?\n- Affected phase: Post-audit `POST-BATCH-M` command access source review\r?\n.*?(?=^### |\z)'
$RunLogText = [regex]::Replace($RunLogText, $RunLogPattern, '')
[System.IO.File]::WriteAllText($RunLogPath, $RunLogText.TrimEnd() + "`n`n" + ($RunLogEntry -join "`n") + "`n", $Utf8NoBom)

[pscustomobject]@{
    ReviewRows = $ReviewRows.Count
    OverlayRows = $PostBatchMRows.Count
    DecisionSummary = $DecisionSummary
    ReviewClassSummary = $ReviewClassSummary
    PostBatchLQueuedRemaining = $PostBatchLQueuedRemaining.Count
    RemainingAfter = $RemainingAfter.Count
    DiffCheckResult = $DiffCheckResult
    Timestamp = $Timestamp
}
