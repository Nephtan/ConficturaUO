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

function Get-RequiredInput
{
    param([string]$FileName)

    $path = Join-Path $OutputDir $FileName
    if (-not (Test-Path -LiteralPath $path))
    {
        throw "Required Phase 10 input is missing: $FileName"
    }

    return $path
}

function Convert-Severity
{
    param([string]$Value)

    switch -Regex ($Value)
    {
        'P0|Critical' { return 'P0' }
        'P1|High' { return 'P1' }
        'P2|Medium' { return 'P2' }
        'P3|Low|Balance' { return 'P3' }
        default { return 'P4' }
    }
}

$projectTruthPath = Get-RequiredInput 'project-truth-register.csv'
$projectBacklogPath = Get-RequiredInput 'project-cleanup-backlog.csv'
$runtimeHookPath = Get-RequiredInput 'runtime-hook-map.csv'
$globalHookPath = Get-RequiredInput 'phase-05-global-hook-risk-list.csv'
$commandPath = Get-RequiredInput 'phase-05-command-surface-register.csv'
$packetPath = Get-RequiredInput 'phase-05-packet-handler-register.csv'
$gumpPath = Get-RequiredInput 'phase-05-gump-response-risk-register.csv'
$timerWorldPath = Get-RequiredInput 'phase-05-timer-world-hook-register.csv'
$serializationPath = Get-RequiredInput 'serialization-register.csv'
$serializationBacklogPath = Get-RequiredInput 'phase-06-save-compatibility-repair-backlog.csv'
$serializationCommentsPath = Get-RequiredInput 'phase-06-serializer-comment-target-list.csv'
$dependencyPath = Get-RequiredInput 'dependency-graph.csv'
$systemCardsPath = Get-RequiredInput 'phase-04-system-card-index.csv'
$synergyPath = Get-RequiredInput 'synergy-conflict-matrix.csv'
$balancePath = Get-RequiredInput 'phase-09-balance-risk-list.csv'
$staffDependencyPath = Get-RequiredInput 'phase-09-staff-dependency-list.csv'
$documentationRiskPath = Get-RequiredInput 'phase-09-documentation-risk-list.csv'
$runtimeInventoryPath = Get-RequiredInput 'cross-tree-runtime-inventory.csv'
$configReferencePath = Get-RequiredInput 'phase-01-config-reference-inventory.csv'

$projectTruth = @(Import-Csv -LiteralPath $projectTruthPath)
$projectBacklog = @(Import-Csv -LiteralPath $projectBacklogPath)
$runtimeHooks = @(Import-Csv -LiteralPath $runtimeHookPath)
$globalHooks = @(Import-Csv -LiteralPath $globalHookPath)
$commands = @(Import-Csv -LiteralPath $commandPath)
$packets = @(Import-Csv -LiteralPath $packetPath)
$gumps = @(Import-Csv -LiteralPath $gumpPath)
$timerWorldHooks = @(Import-Csv -LiteralPath $timerWorldPath)
$serializers = @(Import-Csv -LiteralPath $serializationPath)
$serializerBacklog = @(Import-Csv -LiteralPath $serializationBacklogPath)
$serializerComments = @(Import-Csv -LiteralPath $serializationCommentsPath)
$dependencies = @(Import-Csv -LiteralPath $dependencyPath)
$systemCards = @(Import-Csv -LiteralPath $systemCardsPath)
$synergy = @(Import-Csv -LiteralPath $synergyPath)
$balanceRisks = @(Import-Csv -LiteralPath $balancePath)
$staffDependencies = @(Import-Csv -LiteralPath $staffDependencyPath)
$documentationRisks = @(Import-Csv -LiteralPath $documentationRiskPath)
$runtimeInventory = @(Import-Csv -LiteralPath $runtimeInventoryPath)
$configReferences = @(Import-Csv -LiteralPath $configReferencePath)

$pathToSystem = @{}
foreach ($row in $runtimeInventory)
{
    if (-not [string]::IsNullOrWhiteSpace($row.Path) -and -not $pathToSystem.ContainsKey($row.Path))
    {
        $pathToSystem[$row.Path] = $row.System
    }
}

$findings = New-Object System.Collections.ArrayList
$nonIssues = New-Object System.Collections.ArrayList
$repairItems = New-Object System.Collections.ArrayList
$acceptedRisks = New-Object System.Collections.ArrayList
$commentTargets = New-Object System.Collections.ArrayList
$trackStats = @{}

function Ensure-Track
{
    param([string]$Track)

    if (-not $trackStats.ContainsKey($Track))
    {
        $trackStats[$Track] = [ordered]@{
            Reviewed = 0
            Findings = 0
            NonIssues = 0
            BacklogItems = 0
            AcceptedRisks = 0
            CommentTargets = 0
        }
    }
}

function Add-Finding
{
    param(
        [string]$Track,
        [string]$System,
        [string]$File,
        [string]$Evidence,
        [string]$Impact,
        [string]$Severity,
        [string]$Recommendation,
        [string]$Verification,
        [string]$SourceRegister,
        [string]$Notes
    )

    Ensure-Track $Track
    $stats = $trackStats[$Track]
    $stats['Reviewed'] = [int]$stats['Reviewed'] + 1
    $stats['Findings'] = [int]$stats['Findings'] + 1
    $stats['BacklogItems'] = [int]$stats['BacklogItems'] + 1

    $findingId = 'P10-F{0:00000}' -f ($findings.Count + 1)
    $finding = [pscustomobject]@{
        FindingId = $findingId
        Track = $Track
        System = $System
        File = $File
        Evidence = Trim-Text $Evidence 500
        Impact = $Impact
        Severity = $Severity
        Recommendation = Trim-Text $Recommendation 400
        Verification = Trim-Text $Verification 260
        SourceRegister = $SourceRegister
        ReviewStatus = 'NeedsManualReview'
        Notes = Trim-Text $Notes 400
    }

    [void]$findings.Add($finding)

    [void]$repairItems.Add([pscustomobject]@{
        BacklogId = 'P10-BL-{0:00000}' -f ($repairItems.Count + 1)
        SourceFindingId = $findingId
        Priority = $Severity
        Status = 'Open'
        Track = $Track
        System = $System
        File = $File
        Evidence = Trim-Text $Evidence 500
        Risk = $Impact
        Recommendation = Trim-Text $Recommendation 400
        Verification = Trim-Text $Verification 260
        Notes = 'Generated by Phase 10 risk-specific review; confirm against source before source edits.'
    })
}

function Add-NonIssue
{
    param(
        [string]$Track,
        [string]$System,
        [string]$File,
        [string]$Evidence,
        [string]$Reason,
        [string]$Verification,
        [int]$ReviewedCount
    )

    Ensure-Track $Track
    $stats = $trackStats[$Track]
    $stats['Reviewed'] = [int]$stats['Reviewed'] + $ReviewedCount
    $stats['NonIssues'] = [int]$stats['NonIssues'] + 1

    [void]$nonIssues.Add([pscustomobject]@{
        NonIssueId = 'P10-NI-{0:0000}' -f ($nonIssues.Count + 1)
        Track = $Track
        System = $System
        File = $File
        Evidence = Trim-Text $Evidence 500
        Reason = Trim-Text $Reason 400
        Verification = Trim-Text $Verification 260
    })
}

function Add-AcceptedRisk
{
    param(
        [string]$Track,
        [string]$System,
        [string]$Risk,
        [string]$Rationale,
        [string]$Evidence,
        [string]$NextReview
    )

    Ensure-Track $Track
    $stats = $trackStats[$Track]
    $stats['AcceptedRisks'] = [int]$stats['AcceptedRisks'] + 1

    [void]$acceptedRisks.Add([pscustomobject]@{
        AcceptedRiskId = 'P10-AR-{0:0000}' -f ($acceptedRisks.Count + 1)
        Track = $Track
        System = $System
        Risk = Trim-Text $Risk 400
        Rationale = Trim-Text $Rationale 400
        Evidence = Trim-Text $Evidence 500
        NextReview = Trim-Text $NextReview 260
        Status = 'AcceptedForAuditStageOnly'
    })
}

function Add-CommentTarget
{
    param(
        [string]$Track,
        [string]$FindingId,
        [string]$File,
        [string]$Class,
        [string]$Location,
        [string]$CommentType,
        [string]$Reason,
        [string]$DraftComment,
        [string]$Evidence
    )

    Ensure-Track $Track
    $stats = $trackStats[$Track]
    $stats['CommentTargets'] = [int]$stats['CommentTargets'] + 1

    [void]$commentTargets.Add([pscustomobject]@{
        CommentTargetId = 'P10-COM-{0:0000}' -f ($commentTargets.Count + 1)
        SourceFindingId = $FindingId
        Track = $Track
        File = $File
        Class = $Class
        Location = $Location
        CommentType = $CommentType
        Reason = Trim-Text $Reason 400
        DraftComment = $DraftComment
        SourcePhase = 'Phase 10'
        Status = 'CandidateForPhase11'
        Evidence = Trim-Text $Evidence 500
    })
}

$trackBuild = '10.1 Build Inclusion Drift'
foreach ($row in $projectBacklog)
{
    Add-Finding $trackBuild $row.System '' ("{0}; count={1}; evidence={2}" -f $row.DriftClass, $row.Count, $row.Evidence) 'Build' $row.Priority $row.RecommendedFix $row.Verification 'project-cleanup-backlog.csv' $row.Notes
}
$keptProjectRows = @($projectTruth | Where-Object { $_.Action -eq 'Keep' -and $_.ExistsOnDisk -eq 'True' -and $_.IncludedInScriptsProject -eq 'True' }).Count
Add-NonIssue $trackBuild 'Scripts Project' 'Data/Scripts/Scripts.csproj' ("Keep rows={0}" -f $keptProjectRows) 'Rows marked Keep exist on disk and are included in Scripts.csproj, so they are not Phase 10 drift findings.' 'Re-run New-ProjectTruthRegister.ps1 after project edits.' $keptProjectRows

$trackSerialization = '10.2 Serialization Order And Versioning'
foreach ($row in $serializerBacklog)
{
    Add-Finding $trackSerialization $row.System $row.File ("{0}; class={1}; category={2}" -f $row.Evidence, $row.Class, $row.Category) 'Save' (Convert-Severity $row.Priority) $row.Recommendation $row.Verification 'phase-06-save-compatibility-repair-backlog.csv' $row.Notes
}
$alignedSerializers = @($serializers | Where-Object { $_.FieldAlignment -eq 'AlignedByCountAndKnownTypes' -and $_.HasSerialConstructor -eq 'True' -and $_.BaseCallStatus -eq 'BaseCallsFirst' }).Count
Add-NonIssue $trackSerialization 'World Save Loader' '' ("Aligned serializer rows={0}" -f $alignedSerializers) 'These serializer rows have detected Serial constructors, base calls first, and aligned known write/read counts; no Phase 10 finding was generated for this aggregate.' 'Re-run New-SerializationRegister.ps1 after serializer edits.' $alignedSerializers
foreach ($target in $serializerComments)
{
    Add-CommentTarget $trackSerialization '' $target.File $target.Class $target.Location $target.CommentType $target.Reason $target.DraftComment $target.Evidence
}

$trackHooks = '10.3 Global Hooks And Startup Side Effects'
foreach ($row in @($globalHooks | Where-Object { $_.Risk -eq 'High' -or $_.Risk -eq 'Critical' }))
{
    Add-Finding $trackHooks $row.System $row.File ("{0}:L{1}:{2}; guards={3}" -f $row.HookType, $row.Line, $row.Registration, $row.Guards) 'Runtime' (Convert-Severity $row.Risk) 'Review hook owner, startup side effects, duplicate subscription risk, and guard coverage before changing this entry point.' 'Re-run New-RuntimeHookMap.ps1 and run the narrowest affected build after source edits.' 'phase-05-global-hook-risk-list.csv' 'Marker-derived high-risk global hook row.'
    if ($row.HookType -in @('Event', 'WorldSave', 'WorldLoad', 'Login', 'Logout'))
    {
        Add-CommentTarget $trackHooks '' $row.File '' ("{0} line {1}" -f $row.HookType, $row.Line) 'GlobalHook' 'Global hooks and world lifecycle side effects can be non-obvious to maintainers.' '// Runtime hook: keep this registration aligned with the owning system card and guard assumptions.' ("{0}:L{1}:{2}" -f $row.File, $row.Line, $row.Registration)
    }
}
$mediumGuardedHooks = @($globalHooks | Where-Object { $_.Risk -eq 'Medium' -and $_.Guards -match 'NullGuard|StateGuard' }).Count
Add-NonIssue $trackHooks 'Runtime Hook Map' '' ("Medium guarded hook rows={0}" -f $mediumGuardedHooks) 'Medium-risk hook rows with detected guard markers are tracked but not promoted to Phase 10 findings.' 'Re-run New-RuntimeHookMap.ps1 after hook edits.' $mediumGuardedHooks

$trackPackets = '10.4 Packet Handlers'
foreach ($row in $packets)
{
    Add-Finding $trackPackets $row.System $row.File ("PacketId={0}; Length={1}; Handler={2}; L{3}; Guards={4}; Evidence={5}" -f $row.PacketId, $row.Length, $row.Handler, $row.Line, $row.Guards, $row.Evidence) 'Runtime;Network' 'P0' 'Manually verify packet id, length, access control, NetState validation, malformed packet handling, and override conflicts.' 'Re-run New-RuntimeHookMap.ps1; run narrow build after packet-handler edits; perform manual packet-path review.' 'phase-05-packet-handler-register.csv' 'All packet handlers are critical network entry points.'
    Add-CommentTarget $trackPackets '' $row.File '' ("Packet handler line {0}" -f $row.Line) 'PacketHandler' 'Packet handlers need explicit id, length, access, and state assumptions when the behavior is not obvious.' '// Packet handler: keep packet id, length, access checks, and NetState validation in sync with client protocol assumptions.' ("{0}:L{1}:{2}" -f $row.File, $row.Line, $row.Evidence)
}
$guardedPackets = @($packets | Where-Object { $_.Guards -match 'NullGuard' -and $_.Guards -match 'PacketReadGuard' }).Count
Add-NonIssue $trackPackets 'Packet Handler Register' '' ("Guarded packet rows={0}" -f $guardedPackets) 'Packet rows with detected NullGuard and PacketReadGuard markers remain findings because packet entry points are critical, but guard evidence is recorded as a non-issue aggregate.' 'Manual source review required before changing packet handlers.' $guardedPackets

$trackGumps = '10.5 Gump Response Validation'
foreach ($row in @($gumps | Where-Object { $_.Risk -eq 'High' -or $_.NeedsBoundsReview -eq 'Yes' -or $_.NeedsNullStateReview -eq 'Yes' }))
{
    $reasons = @()
    if ($row.NeedsBoundsReview -eq 'Yes') { $reasons += 'bounds/button review' }
    if ($row.NeedsNullStateReview -eq 'Yes') { $reasons += 'null NetState/Mobile review' }
    if ($reasons.Count -eq 0) { $reasons += 'high-risk gump response path' }

    Add-Finding $trackGumps $row.System $row.File ("L{0}:{1}; guards={2}; review={3}" -f $row.Line, $row.Evidence, $row.Guards, ($reasons -join ',')) 'Runtime;Player;Staff' (Convert-Severity $row.Risk) 'Manually verify NetState/Mobile null guards, deleted target guards, stale list handling, button bounds, text entry parsing, and access checks.' 'Re-run New-RuntimeHookMap.ps1 after gump edits; run Scripts.csproj build for source changes.' 'phase-05-gump-response-risk-register.csv' 'Marker-derived gump response review row.'
}
$guardedGumps = @($gumps | Where-Object { $_.NeedsBoundsReview -eq 'No' -and $_.NeedsNullStateReview -eq 'No' }).Count
Add-NonIssue $trackGumps 'Gump Response Register' '' ("Rows with no generated bounds/null-state review need={0}" -f $guardedGumps) 'Rows without generated bounds/null-state flags remain tracked in Phase 5 but are not additional Phase 10 findings unless high-risk.' 'Manual source review remains required before behavior edits.' $guardedGumps

$trackCommands = '10.6 Command Access And Input Validation'
foreach ($row in $commands)
{
    $needsFinding = $false
    $reason = New-Object System.Collections.ArrayList
    if ([string]::IsNullOrWhiteSpace($row.AccessLevel)) { $needsFinding = $true; [void]$reason.Add('missing parsed access level') }
    if ($row.DuplicateCommandCount -ne '0') { $needsFinding = $true; [void]$reason.Add('duplicate command name') }
    if ($row.Risk -eq 'High' -or $row.Risk -eq 'Critical') { $needsFinding = $true; [void]$reason.Add('high-risk command row') }

    if ($needsFinding)
    {
        Add-Finding $trackCommands $row.System $row.File ("Command={0}; Access={1}; Handler={2}; L{3}; DuplicateCount={4}; Reason={5}; Registration={6}" -f $row.Command, $row.AccessLevel, $row.Handler, $row.Line, $row.DuplicateCommandCount, (Join-Unique @($reason)), $row.RegistrationLine) 'Runtime;Staff;Player' (Convert-Severity $row.Risk) 'Manually verify command access level, usage metadata, argument parsing, target validation, and side effects.' 'Re-run New-RuntimeHookMap.ps1 after command edits; run Scripts.csproj build for source changes.' 'phase-05-command-surface-register.csv' 'Marker-derived command registration review row.'
    }
}
$commandsWithNoDuplicate = @($commands | Where-Object { $_.DuplicateCommandCount -eq '0' }).Count
Add-NonIssue $trackCommands 'Command Register' '' ("Rows with duplicate count 0={0}" -f $commandsWithNoDuplicate) 'These command rows have no generated duplicate-command signal; access and argument review may still be required before source edits.' 'Re-run command register after command edits.' $commandsWithNoDuplicate

$trackPooled = '10.7 Pooled Enumerable Ownership'
$pooledRows = New-Object System.Collections.ArrayList
$sourceRoots = @(
    (Join-Path $RepoRoot 'Data\Scripts'),
    (Join-Path $RepoRoot 'Data\System\Source')
) | Where-Object { Test-Path -LiteralPath $_ }

foreach ($root in $sourceRoots)
{
    foreach ($file in @(Get-ChildItem -LiteralPath $root -Recurse -File -Filter '*.cs' | Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' }))
    {
        $repoPath = Convert-ToRepoPath $file.FullName
        $lines = [System.IO.File]::ReadAllLines($file.FullName)
        $freeCallPresent = $false
        foreach ($line in $lines)
        {
            if ($line -match '\.Free\s*\(')
            {
                $freeCallPresent = $true
                break
            }
        }

        for ($index = 0; $index -lt $lines.Length; $index++)
        {
            $line = $lines[$index]
            if ($line -match 'GetItemsInRange|GetMobilesInRange|GetClientsInRange|GetObjectsInRange|GetItemsInBounds|GetObjectsInBounds|GetMultiTilesAt|IPooledEnumerable')
            {
                $usageKind = if ($line -match 'foreach\s*\(') { 'DirectForeachRangeScan' } elseif ($line -match 'IPooledEnumerable') { 'ExplicitIPooledEnumerable' } else { 'RangeScan' }
                $system = if ($pathToSystem.ContainsKey($repoPath)) { $pathToSystem[$repoPath] } else { 'Unknown' }
                [void]$pooledRows.Add([pscustomobject]@{
                    System = $system
                    File = $repoPath
                    Line = $index + 1
                    UsageKind = $usageKind
                    FreeCallPresentInFile = if ($freeCallPresent) { 'Yes' } else { 'No' }
                    Evidence = Trim-Text $line 220
                })
            }
        }
    }
}

foreach ($row in @($pooledRows | Where-Object { $_.FreeCallPresentInFile -eq 'No' -or $_.UsageKind -eq 'DirectForeachRangeScan' }))
{
    Add-Finding $trackPooled $row.System $row.File ("L{0}:{1}; FreeCallPresentInFile={2}; UsageKind={3}" -f $row.Line, $row.Evidence, $row.FreeCallPresentInFile, $row.UsageKind) 'Runtime;Memory' 'P1' 'Manually verify whether the pooled enumerable is freed on all paths; replace direct foreach range scans or add try/finally Free where required.' 'Manual source review; rerun Phase 10 pooled scan; run Scripts.csproj build for source edits.' 'source pooled-enumerable scan' 'File-level Free detection is conservative and must be confirmed by method-level source review.'
    Add-CommentTarget $trackPooled '' $row.File '' ("Range scan line {0}" -f $row.Line) 'PooledEnumerable' 'Pooled enumerable ownership is easy to break when range scans are edited.' '// Pooled enumerable ownership: keep this range scan paired with Free on every path.' ("{0}:L{1}:{2}" -f $row.File, $row.Line, $row.Evidence)
}
$pooledWithFree = @($pooledRows | Where-Object { $_.FreeCallPresentInFile -eq 'Yes' -and $_.UsageKind -ne 'DirectForeachRangeScan' }).Count
Add-NonIssue $trackPooled 'Pooled Enumerable Scan' '' ("Rows with file-level Free call and no direct foreach={0}" -f $pooledWithFree) 'These rows have a file-level Free marker and are not promoted unless method-level review later finds a leak.' 'Manual method-level review remains required before source edits.' $pooledWithFree

$trackRegion = '10.8 Region And Map Assumptions'
foreach ($row in @($runtimeHooks | Where-Object { $_.HookType -eq 'Region' -or $_.Trigger -match 'Region|Map' }))
{
    Add-Finding $trackRegion $row.System $row.File ("{0}:L{1}:{2}; Guards={3}" -f $row.HookType, $row.Line, $row.Registration, $row.Guards) 'Runtime;Policy;Travel;PvP' (Convert-Severity $row.Risk) 'Review map-specific behavior, guarded/town checks, travel restrictions, city regions, housing regions, and PvP/event policy interactions.' 'Re-run New-RuntimeHookMap.ps1 after region edits; run affected build.' 'runtime-hook-map.csv' 'Region/map policy row.'
}
foreach ($row in @($synergy | Where-Object { ($_.SystemA -eq 'Regions' -or $_.SystemB -eq 'Regions') -and $_.Labels -notmatch 'NoLink' }))
{
    Add-Finding $trackRegion ("{0}<->{1}" -f $row.SystemA, $row.SystemB) '' ("Labels={0}; Evidence={1}" -f $row.Labels, $row.Evidence) 'Runtime;Policy;Gameplay' 'P2' 'Review region precedence with the paired system before changing region, travel, PvP, or government policy.' 'Manual source review plus dependency graph rerun after source edits.' 'synergy-conflict-matrix.csv' 'Phase 9 region relationship.'
}
$regionNoLink = @($synergy | Where-Object { ($_.SystemA -eq 'Regions' -or $_.SystemB -eq 'Regions') -and $_.Labels -eq 'NoLink' }).Count
Add-NonIssue $trackRegion 'Regions' '' ("NoLink region matrix rows={0}" -f $regionNoLink) 'Phase 9 generated no direct relationship for these region pairs; no Phase 10 region finding was generated for those rows.' 'Re-run dependency graph and synergy matrix after source edits.' $regionNoLink

$trackPlayerMobile = '10.9 PlayerMobile Field Coupling'
foreach ($edge in @($dependencies | Where-Object { $_.SourceSystem -eq 'PlayerMobile Core' -or $_.TargetSystem -eq 'PlayerMobile Core' -or $_.Evidence -match 'PlayerMobile' }))
{
    $severity = 'P1'
    if ($edge.Impact -match 'Save')
    {
        $severity = 'P0'
    }

    Add-Finding $trackPlayerMobile $edge.SourceSystem '' ("{0}->{1}; {2}; {3}" -f $edge.SourceSystem, $edge.TargetSystem, $edge.EdgeType, $edge.Evidence) 'Save;Runtime;Player' $severity 'Review shared player fields, save compatibility, and policy ordering before changing PlayerMobile or the dependent system.' 'Manual source review; rerun serialization register, dependency graph, and affected build after PlayerMobile edits.' 'dependency-graph.csv' 'PlayerMobile coupling edge.'
}
$playerMobileSerializer = $serializers | Where-Object { $_.File -eq 'Data/Scripts/Mobiles/Base/PlayerMobile.cs' } | Select-Object -First 1
if ($null -ne $playerMobileSerializer)
{
    Add-CommentTarget $trackPlayerMobile '' $playerMobileSerializer.File $playerMobileSerializer.Class 'Serialize/Deserialize' 'PlayerMobile is a central save and cross-system coupling surface.' '// Save and policy coupling: PlayerMobile serialized fields are shared by multiple systems; preserve read/write order and review dependent policies before edits.' $playerMobileSerializer.Evidence
}
Add-NonIssue $trackPlayerMobile 'PlayerMobile Core' 'Data/Scripts/Mobiles/Base/PlayerMobile.cs' 'PlayerMobile tracked as DoNotMove in Phase 6.' 'The audit already marks PlayerMobile as a do-not-move save surface, so Phase 10 does not approve any rename/move.' 'Manual migration plan and save-load test required before any rename or move.' 1

$trackEconomy = '10.10 Economy And Reward Loops'
foreach ($row in $balanceRisks)
{
    Add-Finding $trackEconomy $row.Systems '' ("Labels={0}; RiskKind={1}; Evidence={2}" -f $row.Labels, $row.RiskKind, $row.Evidence) 'Balance;Player;Economy' 'P3' $row.Recommendation 'Manual balance review; rerun synergy matrix after reward/economy source edits.' 'phase-09-balance-risk-list.csv' $row.WhyBalanceNotCorrectness
}
$economySupports = @($synergy | Where-Object { $_.Labels -match 'Supports' -and ($_.DomainsA -match 'Economy|Crafting|Housing' -or $_.DomainsB -match 'Economy|Crafting|Housing') }).Count
Add-NonIssue $trackEconomy 'Economy Systems' '' ("Positive economy/crafting/housing support rows={0}" -f $economySupports) 'Positive synergies are preservation notes rather than repair findings unless also labeled BalanceRisk.' 'Rerun Phase 9 matrix after economy edits.' $economySupports

$trackStaff = '10.11 Staff Tooling'
foreach ($row in $staffDependencies)
{
    Add-Finding $trackStaff $row.RequiredStaffSurface '' ("Systems={0}; Labels={1}; Evidence={2}" -f $row.Systems, $row.Labels, $row.Evidence) 'Staff;Runtime;Player' 'P2' $row.Recommendation 'Manual staff workflow review; verify command access and event override docs after edits.' 'phase-09-staff-dependency-list.csv' 'Phase 9 staff dependency row.'
}
foreach ($row in @($commands | Where-Object { $_.System -match 'Staff|XMLSpawner|Invasion|Static Gump|Character Swap|ChangeSeason' -and [string]::IsNullOrWhiteSpace($_.AccessLevel) }))
{
    Add-Finding $trackStaff $row.System $row.File ("Command={0}; Access={1}; L{2}; Registration={3}" -f $row.Command, $row.AccessLevel, $row.Line, $row.RegistrationLine) 'Staff;Runtime' 'P2' 'Verify staff-only access and command metadata before exposing or reorganizing this tool.' 'Re-run command register and Scripts.csproj build after command edits.' 'phase-05-command-surface-register.csv' 'Staff tool command access review.'
}
Add-NonIssue $trackStaff 'Staff Dependency Register' '' ("Staff dependency rows={0}" -f $staffDependencies.Count) 'Staff dependencies are visible in a focused register; Phase 10 does not require source edits for visibility alone.' 'Manual staff workflow review remains required before tool changes.' $staffDependencies.Count

$trackLegacy = '10.12 Legacy Compatibility'
foreach ($row in @($projectTruth | Where-Object { $_.Path -match '/Obsolete/|\\Obsolete\\|Obsolete' -or $_.LikelySystem -match 'Obsolete|Legacy' }))
{
    Add-Finding $trackLegacy $row.LikelySystem $row.Path ("RecordType={0}; Action={1}; RuntimeMarkers={2}; Types={3}" -f $row.RecordType, $row.Action, $row.RuntimeMarkers, $row.Types) 'Build;Runtime;Save' 'P2' 'Review whether this legacy file is still compiled, still registers hooks or commands, or is needed for old saves before removal.' 'Project truth rerun; hook scan; serializer register rerun; build after source/project edits.' 'project-truth-register.csv' 'Legacy/obsolete project truth row.'
}
foreach ($row in @($runtimeHooks | Where-Object { $_.System -match 'Obsolete|Legacy' }))
{
    Add-Finding $trackLegacy $row.System $row.File ("{0}:L{1}:{2}" -f $row.HookType, $row.Line, $row.Registration) 'Runtime' (Convert-Severity $row.Risk) 'Review active legacy hook before deleting, moving, or disabling legacy code.' 'Re-run runtime hook map and build after source edits.' 'runtime-hook-map.csv' 'Legacy runtime hook row.'
}
Add-NonIssue $trackLegacy 'Legacy Compatibility' '' 'No source deletion was performed in Phase 10.' 'Legacy review remains analysis-only; no compatibility shim was removed.' 'Future deletion requires project truth, serializer, hook, and build verification.' 1

$trackXml = '10.13 XML And Config Schemas'
foreach ($edge in @($dependencies | Where-Object { $_.EdgeType -eq 'XMLConfig' }))
{
    Add-Finding $trackXml $edge.SourceSystem '' ("{0}->{1}; {2}" -f $edge.SourceSystem, $edge.TargetSystem, $edge.Evidence) 'Runtime;Data;Docs' 'P2' 'Document XML/config schema, missing/default behavior, reload assumptions, and invalid node handling before changing data-driven behavior.' 'Rerun inventory/config reference scan and affected build after source edits.' 'dependency-graph.csv' 'XML/config dependency edge.'
}
foreach ($row in $configReferences)
{
    Add-Finding $trackXml $row.LikelySystem $row.File ("Reference={0}; L{1}; Evidence={2}" -f $row.Reference, $row.Line, $row.Evidence) 'Runtime;Data' 'P3' 'Confirm config path, schema expectations, defaults, and reload behavior for this data-driven reference.' 'Rerun Invoke-CodebaseAuditInventory.ps1 after config/source edits.' 'phase-01-config-reference-inventory.csv' 'Config reference inventory row.'
}
$configEdgeCount = @($dependencies | Where-Object { $_.EdgeType -eq 'XMLConfig' }).Count
Add-NonIssue $trackXml 'XML/config dependencies' '' ("XMLConfig dependency edges={0}" -f $configEdgeCount) 'XML/config references are explicitly visible as dependency graph edges; no source edit is required to make the relationship visible.' 'Rerun dependency graph after config/source edits.' $configEdgeCount

$trackDocs = '10.14 Documentation Contradictions'
foreach ($row in $documentationRisks)
{
    Add-Finding $trackDocs $row.System $row.DocPath ("{0}; SourceTrace={1}; MissingSource={2}; Backlog={3}" -f $row.RiskReason, $row.SourceTracePresent, $row.MissingSourceFiles, $row.GeneratedBacklogId) 'Docs;Player;Staff' 'P3' $row.Recommendation 'Rerun New-DocumentationTruthAudit.ps1 after documentation fixes.' 'phase-09-documentation-risk-list.csv' 'Phase 9 documentation risk row.'
}
$sourceTracedDocs = @($documentationRisks | Where-Object { $_.SourceTracePresent -eq 'Yes' }).Count
Add-NonIssue $trackDocs 'Documentation' '' ("Documentation risk rows already source-traced={0}" -f $sourceTracedDocs) 'Source-traced rows can still carry stale or alias risks, but source trace presence is recorded as a non-issue signal.' 'Rerun documentation truth audit after docs edits.' $sourceTracedDocs

Add-AcceptedRisk 'Phase 10 Overall' 'Audit Program' 'No source code, project file, or serialization layout was changed in Phase 10.' 'Phase 10 is an evidence classification pass; repair implementation is deferred to Phase 13+ backlog batches.' 'git status and staged file list for Phase 10 should show only audit docs/tools/outputs.' 'Phase 13 repair backlog and later focused implementation batches.'
Add-AcceptedRisk 'Phase 10 Overall' 'Audit Program' 'Marker-derived findings may include false positives until manual source review.' 'The audit preserves conservative findings as Open/NeedsManualReview rather than silently discarding risky rows.' 'Findings cite source registers and source-scan evidence for independent verification.' 'Manual source review before code edits.'
Add-AcceptedRisk 'Phase 10 Overall' 'Audit Program' 'Build verification is not run for Phase 10 generated audit outputs.' 'No source or project files are edited by this phase; docs-only verification is the appropriate risk level.' 'Phase 10 generated outputs and generator only.' 'Run narrow MSBuild targets when Phase 13+ repair batches touch source/project files.'

$pooledScanPath = Export-AuditCsv @($pooledRows) 'phase-10-pooled-enumerable-review.csv'
$findingsPath = Export-AuditCsv @($findings) 'risk-track-findings.csv'
$phaseFindingsPath = Export-AuditCsv @($findings) 'phase-10-risk-track-findings.csv'
$nonIssuePath = Export-AuditCsv @($nonIssues) 'phase-10-non-issue-records.csv'
$repairPath = Export-AuditCsv @($repairItems) 'phase-10-repair-backlog-items.csv'
$acceptedPath = Export-AuditCsv @($acceptedRisks) 'phase-10-accepted-risk-notes.csv'
$commentPath = Export-AuditCsv @($commentTargets) 'phase-10-comment-target-additions.csv'

$coverageRows = New-Object System.Collections.ArrayList
foreach ($track in @($trackStats.Keys | Sort-Object))
{
    $stats = $trackStats[$track]
    [void]$coverageRows.Add([pscustomobject]@{
        Track = $track
        ReviewedRows = $stats['Reviewed']
        Findings = $stats['Findings']
        NonIssues = $stats['NonIssues']
        BacklogItems = $stats['BacklogItems']
        AcceptedRisks = $stats['AcceptedRisks']
        CommentTargets = $stats['CommentTargets']
        Status = 'Complete'
        Evidence = 'Generated by Phase 10 risk-specific review script from phase registers and source pooled-enumerable scan.'
    })
}
$coveragePath = Export-AuditCsv @($coverageRows) 'phase-10-track-coverage.csv'

$severityCounts = @{}
foreach ($finding in $findings)
{
    if (-not $severityCounts.ContainsKey($finding.Severity))
    {
        $severityCounts[$finding.Severity] = 0
    }
    $severityCounts[$finding.Severity]++
}

$trackCounts = @{}
foreach ($finding in $findings)
{
    if (-not $trackCounts.ContainsKey($finding.Track))
    {
        $trackCounts[$finding.Track] = 0
    }
    $trackCounts[$finding.Track]++
}

$summaryPath = Join-Path $OutputDir 'phase-10-summary.md'
$summary = New-Object System.Collections.ArrayList
[void]$summary.Add('# Phase 10 Risk-Specific Code Review Tracks Summary')
[void]$summary.Add('')
[void]$summary.Add(("Generated: {0}" -f (Get-Date -Format 'o')))
[void]$summary.Add('')
[void]$summary.Add('## Required Inputs')
[void]$summary.Add('')
[void]$summary.Add('| Input | Status |')
[void]$summary.Add('| --- | --- |')
[void]$summary.Add(('| Project Truth Register | Present: `project-truth-register.csv` with {0} rows |' -f $projectTruth.Count))
[void]$summary.Add(('| Runtime Hook Map | Present: `runtime-hook-map.csv` with {0} rows |' -f $runtimeHooks.Count))
[void]$summary.Add(('| Serialization Register | Present: `serialization-register.csv` with {0} rows |' -f $serializers.Count))
[void]$summary.Add(('| Dependency Graph | Present: `dependency-graph.csv` with {0} rows |' -f $dependencies.Count))
[void]$summary.Add(('| System Cards | Present: `phase-04-system-card-index.csv` with {0} rows |' -f $systemCards.Count))
[void]$summary.Add(('| Synergy and Conflict Matrix | Present: `synergy-conflict-matrix.csv` with {0} rows |' -f $synergy.Count))
[void]$summary.Add('')
[void]$summary.Add('## Generated Outputs')
[void]$summary.Add('')
[void]$summary.Add('| Output | Rows | Purpose |')
[void]$summary.Add('| --- | ---: | --- |')
[void]$summary.Add(('| `risk-track-findings.csv` | {0} | Canonical Phase 10 findings across all risk tracks. |' -f $findings.Count))
[void]$summary.Add(('| `phase-10-risk-track-findings.csv` | {0} | Phase-scoped findings copy. |' -f $findings.Count))
[void]$summary.Add(('| `phase-10-non-issue-records.csv` | {0} | Track-level non-issues and aggregate non-finding evidence. |' -f $nonIssues.Count))
[void]$summary.Add(('| `phase-10-repair-backlog-items.csv` | {0} | One open follow-up item per Phase 10 finding. |' -f $repairItems.Count))
[void]$summary.Add(('| `phase-10-accepted-risk-notes.csv` | {0} | Risks accepted only for the audit stage, not for implementation. |' -f $acceptedRisks.Count))
[void]$summary.Add(('| `phase-10-comment-target-additions.csv` | {0} | Candidate Phase 11 source-comment targets. |' -f $commentTargets.Count))
[void]$summary.Add(('| `phase-10-pooled-enumerable-review.csv` | {0} | Source-scan rows for range scans and pooled enumerable ownership. |' -f $pooledRows.Count))
[void]$summary.Add(('| `phase-10-track-coverage.csv` | {0} | Per-track reviewed row, finding, backlog, non-issue, accepted-risk, and comment-target counts. |' -f $coverageRows.Count))
[void]$summary.Add('')
[void]$summary.Add('## Finding Counts By Severity')
[void]$summary.Add('')
[void]$summary.Add('| Severity | Count |')
[void]$summary.Add('| --- | ---: |')
foreach ($severity in @($severityCounts.Keys | Sort-Object))
{
    [void]$summary.Add(("| {0} | {1} |" -f $severity, $severityCounts[$severity]))
}
[void]$summary.Add('')
[void]$summary.Add('## Finding Counts By Track')
[void]$summary.Add('')
[void]$summary.Add('| Track | Findings |')
[void]$summary.Add('| --- | ---: |')
foreach ($track in @($trackCounts.Keys | Sort-Object))
{
    [void]$summary.Add(("| {0} | {1} |" -f $track, $trackCounts[$track]))
}
[void]$summary.Add('')
[void]$summary.Add('## Exit Criteria')
[void]$summary.Add('')
[void]$summary.Add('- Every Phase 10 risk track has coverage, findings or explicit non-issue records, and follow-up tasks.')
[void]$summary.Add('- Findings cite generated registers or source-scan evidence and are marked `NeedsManualReview` before code edits.')
[void]$summary.Add('- Balance and documentation risks remain separated from build, save, runtime, and network correctness findings.')
[void]$summary.Add('- Candidate source comments are deferred to Phase 11 review; no source comments were added in Phase 10.')
[void]$summary.Add('- No high-risk finding remains only in chat memory; findings and backlog rows are durable repository outputs.')

[System.IO.File]::WriteAllLines($summaryPath, [string[]]$summary, (New-Object System.Text.UTF8Encoding($false)))

[pscustomobject]@{
    Findings = Convert-ToRepoPath $findingsPath
    PhaseFindings = Convert-ToRepoPath $phaseFindingsPath
    NonIssues = Convert-ToRepoPath $nonIssuePath
    RepairBacklog = Convert-ToRepoPath $repairPath
    AcceptedRisks = Convert-ToRepoPath $acceptedPath
    CommentTargets = Convert-ToRepoPath $commentPath
    PooledEnumerableReview = Convert-ToRepoPath $pooledScanPath
    TrackCoverage = Convert-ToRepoPath $coveragePath
    Summary = Convert-ToRepoPath $summaryPath
    FindingRows = $findings.Count
    NonIssueRows = $nonIssues.Count
    RepairRows = $repairItems.Count
    AcceptedRiskRows = $acceptedRisks.Count
    CommentTargetRows = $commentTargets.Count
    PooledRows = $pooledRows.Count
    TrackRows = $coverageRows.Count
}
