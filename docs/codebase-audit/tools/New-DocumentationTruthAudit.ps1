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

function Normalize-Slug
{
    param([string]$Value)

    $withoutExtension = [System.IO.Path]::GetFileNameWithoutExtension($Value)
    return (($withoutExtension.ToLowerInvariant() -replace '[^a-z0-9]+', '-') -replace '(^-|-$)', '')
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

function Resolve-RepoRelativeLink
{
    param(
        [string]$BaseRepoPath,
        [string]$Target
    )

    if ([string]::IsNullOrWhiteSpace($Target))
    {
        return ''
    }

    if ($Target -match '^[a-zA-Z][a-zA-Z0-9+.-]*:')
    {
        return ''
    }

    $withoutAnchor = ($Target -split '#')[0]

    if ([string]::IsNullOrWhiteSpace($withoutAnchor))
    {
        return ''
    }

    $withoutAnchor = [System.Uri]::UnescapeDataString($withoutAnchor)
    $baseDirectory = Split-Path -Path $BaseRepoPath -Parent
    $combined = Join-Path (Join-Path $RepoRoot ($baseDirectory -replace '/', '\')) ($withoutAnchor -replace '/', '\')

    return Convert-ToRepoPath $combined
}

function Get-MarkdownLinks
{
    param(
        [string]$BaseRepoPath,
        [string]$Content
    )

    $links = New-Object System.Collections.Generic.List[object]

    foreach ($match in [regex]::Matches($Content, '\[([^\]]+)\]\(([^)]+)\)'))
    {
        $target = $match.Groups[2].Value.Trim()
        $resolved = Resolve-RepoRelativeLink -BaseRepoPath $BaseRepoPath -Target $target

        if (-not [string]::IsNullOrWhiteSpace($resolved))
        {
            $links.Add([pscustomobject]@{
                Text = $match.Groups[1].Value.Trim()
                Target = $target
                Resolved = $resolved
            }) | Out-Null
        }
    }

    return @($links.ToArray())
}

function Get-SourcePaths
{
    param(
        [string]$BaseRepoPath,
        [string]$Content
    )

    $paths = New-Object System.Collections.Generic.List[string]

    foreach ($match in [regex]::Matches($Content, '`([^`]*(?:Data/Scripts|Data/System/Source)[^`]*)`'))
    {
        $value = $match.Groups[1].Value.Trim()
        $value = $value -replace '\\', '/'

        if ($value -match '(Data/(?:Scripts|System/Source)/.+)$')
        {
            $candidate = [System.Uri]::UnescapeDataString($Matches[1]).Trim().TrimEnd('.', ',', ';', ':')

            if (-not $paths.Contains($candidate))
            {
                $paths.Add($candidate) | Out-Null
            }
        }
    }

    foreach ($link in Get-MarkdownLinks -BaseRepoPath $BaseRepoPath -Content $Content)
    {
        if ($link.Resolved -match '^Data/(?:Scripts|System/Source)/')
        {
            $candidate = $link.Resolved.Trim().TrimEnd('.', ',', ';', ':')

            if (-not $paths.Contains($candidate))
            {
                $paths.Add($candidate) | Out-Null
            }
        }
    }

    return @($paths.ToArray())
}

function Get-Title
{
    param([string]$Content)

    foreach ($line in ($Content -split "`r?`n"))
    {
        if ($line -match '^\s*#\s+(.+)$')
        {
            return $Matches[1].Trim()
        }
    }

    return ''
}

function Get-DocScope
{
    param([string]$Path)

    $fileName = [System.IO.Path]::GetFileName($Path)

    if ($Path -eq 'docs/SystemAudit.md')
    {
        return 'SystemAudit'
    }

    if ($Path -match '^docs/wiki/')
    {
        if ($fileName -in @('INDEX.md', 'wikibacklog.md', 'wikiautomationmemory.md', 'wikihandoff.md', 'wikicommitprompt.md', 'auditautomationprompt.md', 'fixautomationprompt.md'))
        {
            return 'WikiSupport'
        }

        return 'WikiPage'
    }

    if ($Path -match '^docs/CCWM/')
    {
        return 'MapSupport'
    }

    return 'Documentation'
}

function Get-BacklogMap
{
    param([string]$BacklogPath)

    $rows = New-Object System.Collections.Generic.List[object]

    if (-not (Test-Path -LiteralPath $BacklogPath -PathType Leaf))
    {
        return @($rows.ToArray())
    }

    foreach ($line in Get-Content -LiteralPath $BacklogPath)
    {
        if ($line -notmatch '^\|\s*WIKI-[0-9]+')
        {
            continue
        }

        $cells = @($line.Trim().Trim('|') -split '\|')

        if ($cells.Count -lt 9)
        {
            continue
        }

        $rows.Add([pscustomobject]@{
            Id = $cells[0].Trim()
            Status = $cells[1].Trim()
            Priority = $cells[2].Trim()
            Category = $cells[3].Trim()
            Files = $cells[4].Trim()
            Evidence = $cells[5].Trim()
            RecommendedFix = $cells[6].Trim()
            LastSeen = $cells[7].Trim()
            FixNotes = $cells[8].Trim()
        }) | Out-Null
    }

    return @($rows.ToArray())
}

function Find-BacklogIdsForDoc
{
    param(
        [object[]]$BacklogRows,
        [string]$DocPath
    )

    $fileName = [System.IO.Path]::GetFileName($DocPath)
    $ids = New-Object System.Collections.Generic.List[string]

    foreach ($row in $BacklogRows)
    {
        if ($row.Files -match [regex]::Escape($DocPath) -or $row.Files -match [regex]::Escape($fileName))
        {
            $ids.Add($row.Id) | Out-Null
        }
    }

    return ($ids -join ';')
}

function Get-IndexMap
{
    param([string]$IndexPath)

    $map = @{}

    if (-not (Test-Path -LiteralPath $IndexPath -PathType Leaf))
    {
        return $map
    }

    $currentCategory = 'Uncategorized'
    $indexRepoPath = Convert-ToRepoPath $IndexPath

    foreach ($line in Get-Content -LiteralPath $IndexPath)
    {
        if ($line -match '^##\s+(.+)$')
        {
            $currentCategory = $Matches[1].Trim()
            continue
        }

        foreach ($match in [regex]::Matches($line, '\[([^\]]+)\]\(([^)]+\.md(?:#[^)]+)?)\)'))
        {
            $resolved = Resolve-RepoRelativeLink -BaseRepoPath $indexRepoPath -Target $match.Groups[2].Value.Trim()

            if (-not [string]::IsNullOrWhiteSpace($resolved))
            {
                $map[$resolved] = [pscustomobject]@{
                    Category = $currentCategory
                    LinkText = $match.Groups[1].Value.Trim()
                }
            }
        }
    }

    return $map
}

function Get-CoverageByFile
{
    param(
        [object[]]$Rows,
        [string]$PathProperty,
        [string]$ValueProperty
    )

    $map = @{}

    foreach ($row in $Rows)
    {
        $path = [string]$row.$PathProperty

        if ([string]::IsNullOrWhiteSpace($path))
        {
            continue
        }

        if (-not $map.ContainsKey($path))
        {
            $map[$path] = New-Object System.Collections.Generic.List[string]
        }

        $value = [string]$row.$ValueProperty

        if (-not [string]::IsNullOrWhiteSpace($value) -and -not $map[$path].Contains($value))
        {
            $map[$path].Add($value) | Out-Null
        }
    }

    return $map
}

New-Item -ItemType Directory -Force -Path $OutputDir | Out-Null

$documentationInventory = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'phase-01-documentation-inventory.csv'))
$runtimeHookMap = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'runtime-hook-map.csv'))
$serializationRegister = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'serialization-register.csv'))
$projectTruth = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'project-truth-register.csv'))
$systemCards = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'phase-04-system-card-index.csv'))

$wikiIndexPath = Join-Path $RepoRoot 'docs\wiki\INDEX.md'
$wikiBacklogPath = Join-Path $RepoRoot 'docs\wiki\wikibacklog.md'
$indexMap = Get-IndexMap -IndexPath $wikiIndexPath
$wikiBacklogRows = @(Get-BacklogMap -BacklogPath $wikiBacklogPath)
$runtimeByFile = Get-CoverageByFile -Rows $runtimeHookMap -PathProperty 'File' -ValueProperty 'HookType'
$serializationByFile = Get-CoverageByFile -Rows $serializationRegister -PathProperty 'File' -ValueProperty 'Class'
$projectByPath = @{}
foreach ($row in $projectTruth)
{
    if (-not $projectByPath.ContainsKey($row.Path))
    {
        $projectByPath[$row.Path] = $row
    }
}

$inventoryByPath = @{}
foreach ($row in $documentationInventory)
{
    $inventoryByPath[$row.DocPath] = $row
}

$docFiles = @(Get-ChildItem -LiteralPath (Join-Path $RepoRoot 'docs') -Recurse -File -Filter '*.md' | Where-Object {
    $repoPath = Convert-ToRepoPath $_.FullName
    $repoPath -notmatch '^docs/codebase-audit/' -and
    $repoPath -notmatch '^docs/codebase-audit-phases/'
} | Sort-Object FullName)

$truthRows = New-Object System.Collections.Generic.List[object]
$backlogCandidates = New-Object System.Collections.Generic.List[object]
$aliasRows = New-Object System.Collections.Generic.List[object]
$canonicalRows = New-Object System.Collections.Generic.List[object]

foreach ($file in $docFiles)
{
    $docPath = Convert-ToRepoPath $file.FullName
    $content = Get-Content -Raw -LiteralPath $file.FullName
    $title = Get-Title $content
    $slug = Normalize-Slug $docPath
    $scope = Get-DocScope $docPath
    $indexed = $indexMap.ContainsKey($docPath)
    $indexCategory = if ($indexed) { $indexMap[$docPath].Category } else { '' }
    $indexText = if ($indexed) { $indexMap[$docPath].LinkText } else { '' }
    $inventory = if ($inventoryByPath.ContainsKey($docPath)) { $inventoryByPath[$docPath] } else { $null }
    $sourceTracePresent = ($content -match '(?im)^##\s+Source Trace\b' -or ($null -ne $inventory -and $inventory.SourceTrace -eq 'True'))
    $codeVerified = ($content -match 'Code-Verified|Code Verified' -or ($null -ne $inventory -and $inventory.CodeVerified -eq 'True'))
    $needsRework = ($content -match 'Needs Rework|TODO|placeholder|stub' -or ($null -ne $inventory -and $inventory.NeedsRework -eq 'True'))
    $legacySlug = ($content -match '(?im)^#\s+Legacy Slug\b|legacy slug is preserved' -or ($null -ne $inventory -and $inventory.LegacySlug -eq 'True'))
    $links = @(Get-MarkdownLinks -BaseRepoPath $docPath -Content $content)
    $sourcePaths = @(Get-SourcePaths -BaseRepoPath $docPath -Content $content)
    $existingBacklogIds = Find-BacklogIdsForDoc -BacklogRows $wikiBacklogRows -DocPath $docPath
    $missingSourceFiles = New-Object System.Collections.Generic.List[string]
    $existingSourceFiles = New-Object System.Collections.Generic.List[string]

    foreach ($sourcePath in $sourcePaths)
    {
        $literal = Join-Path $RepoRoot ($sourcePath -replace '/', '\')

        if (Test-Path -LiteralPath $literal -PathType Leaf)
        {
            if (-not $existingSourceFiles.Contains($sourcePath))
            {
                $existingSourceFiles.Add($sourcePath) | Out-Null
            }
        }
        elseif (Test-Path -LiteralPath $literal -PathType Container)
        {
            if (-not $existingSourceFiles.Contains($sourcePath))
            {
                $existingSourceFiles.Add($sourcePath) | Out-Null
            }
        }
        else
        {
            if (-not $missingSourceFiles.Contains($sourcePath))
            {
                $missingSourceFiles.Add($sourcePath) | Out-Null
            }
        }
    }

    $runtimeTypes = New-Object System.Collections.Generic.List[string]
    $serializedClasses = New-Object System.Collections.Generic.List[string]
    $projectTruthStates = New-Object System.Collections.Generic.List[string]

    foreach ($sourcePath in $existingSourceFiles)
    {
        if ($runtimeByFile.ContainsKey($sourcePath))
        {
            foreach ($hookType in $runtimeByFile[$sourcePath].ToArray())
            {
                if (-not $runtimeTypes.Contains($hookType))
                {
                    $runtimeTypes.Add($hookType) | Out-Null
                }
            }
        }

        if ($serializationByFile.ContainsKey($sourcePath))
        {
            foreach ($className in $serializationByFile[$sourcePath].ToArray())
            {
                if (-not $serializedClasses.Contains($className))
                {
                    $serializedClasses.Add($className) | Out-Null
                }
            }
        }

        if ($projectByPath.ContainsKey($sourcePath))
        {
            $projectTruthStates.Add("$sourcePath=$($projectByPath[$sourcePath].Action)") | Out-Null
        }
    }

    $canonicalStatus = 'Support'
    $canonicalTarget = ''
    $aliasType = ''
    $independentAliasClaims = 'No'

    if ($scope -eq 'WikiPage')
    {
        if ($legacySlug)
        {
            $canonicalStatus = 'Alias'
            $aliasType = 'LegacySlug'
            $canonicalLink = @($links | Where-Object { $_.Resolved -match '^docs/wiki/' -and $_.Resolved -ne $docPath } | Select-Object -First 1)

            if ($canonicalLink.Count -gt 0)
            {
                $canonicalTarget = $canonicalLink[0].Resolved
            }

            $nonHeadingLineCount = @(($content -split "`r?`n") | Where-Object {
                $trimmed = $_.Trim()
                -not [string]::IsNullOrWhiteSpace($trimmed) -and
                $trimmed -notmatch '^#' -and
                $trimmed -notmatch '^This legacy slug is preserved'
            }).Count

            if ($nonHeadingLineCount -gt 8)
            {
                $independentAliasClaims = 'Yes'
            }
        }
        elseif ($indexed)
        {
            $canonicalStatus = 'Canonical'
        }
        else
        {
            $canonicalStatus = 'Unknown'
        }
    }
    elseif ($scope -eq 'SystemAudit')
    {
        $canonicalStatus = 'AuditRegister'
    }

    $staleClaims = New-Object System.Collections.Generic.List[string]
    $missingClaims = New-Object System.Collections.Generic.List[string]

    if ($needsRework)
    {
        $staleClaims.Add('Contains Needs Rework/TODO/stub markers that require source-backed follow-up.') | Out-Null
    }

    if ($missingSourceFiles.Count -gt 0)
    {
        $staleClaims.Add('References source paths that do not exist on disk.') | Out-Null
    }

    if ($canonicalStatus -eq 'Canonical' -and -not $sourceTracePresent)
    {
        $missingClaims.Add('Missing Source Trace section.') | Out-Null
    }

    if ($canonicalStatus -eq 'Canonical' -and $existingSourceFiles.Count -eq 0)
    {
        $missingClaims.Add('No existing source files were extracted from the page.') | Out-Null
    }

    if ($runtimeTypes.Count -gt 0 -and $content -notmatch 'Command|Runtime|Entry|Gump|Timer|Hook|Speech|Movement|Packet|Region')
    {
        $missingClaims.Add('Traced source files have runtime hooks, but the page does not clearly cover runtime entry points.') | Out-Null
    }

    if ($serializedClasses.Count -gt 0 -and $content -notmatch 'Serial|Serialize|Deserialize|Persistence|Persistent|Save|version')
    {
        $missingClaims.Add('Traced source files have serialized state, but persistence is not clearly documented.') | Out-Null
    }

    if ($canonicalStatus -eq 'Alias' -and [string]::IsNullOrWhiteSpace($canonicalTarget))
    {
        $missingClaims.Add('Alias page does not expose a resolvable canonical target.') | Out-Null
    }

    if ($canonicalStatus -eq 'Alias' -and $independentAliasClaims -eq 'Yes')
    {
        $missingClaims.Add('Alias page carries independent behavior claims instead of only pointing to the canonical page.') | Out-Null
    }

    if ($scope -eq 'WikiPage' -and -not $indexed)
    {
        $missingClaims.Add('Wiki page is not linked from docs/wiki/INDEX.md.') | Out-Null
    }

    $coverageStatus = if ($canonicalStatus -in @('Support', 'AuditRegister')) {
        'Support'
    } elseif ($staleClaims.Count -eq 0 -and $missingClaims.Count -eq 0 -and $sourceTracePresent) {
        'SourceTracePresent'
    } elseif ($existingBacklogIds.Length -gt 0) {
        'Backlogged'
    } elseif ($sourceTracePresent -and $existingSourceFiles.Count -gt 0) {
        'Partial'
    } else {
        'NeedsBacklog'
    }

    $truthRow = [pscustomobject]@{
        DocPath = $docPath
        Title = $title
        Scope = $scope
        CanonicalPage = $canonicalStatus
        CanonicalTarget = $canonicalTarget
        LegacySlug = [string]$legacySlug
        Indexed = [string]$indexed
        IndexCategory = $indexCategory
        IndexText = $indexText
        SourceTracePresent = if ($sourceTracePresent) { 'Yes' } else { 'No' }
        CodeVerified = [string]$codeVerified
        VerifiedSourceFiles = ($existingSourceFiles.ToArray() -join ';')
        MissingSourceFiles = ($missingSourceFiles.ToArray() -join ';')
        RuntimeHooksCovered = ($runtimeTypes.ToArray() | Sort-Object -Unique) -join ';'
        SerializationCovered = ($serializedClasses.ToArray() | Sort-Object -Unique) -join ';'
        ProjectTruthCoverage = ($projectTruthStates.ToArray() -join ';')
        StaleClaims = ($staleClaims.ToArray() -join '; ')
        MissingClaims = ($missingClaims.ToArray() -join '; ')
        ExistingBacklogIds = $existingBacklogIds
        GeneratedBacklogId = ''
        CoverageStatus = $coverageStatus
    }

    $truthRows.Add($truthRow) | Out-Null

    if ($canonicalStatus -eq 'Canonical')
    {
        $canonicalRows.Add([pscustomobject]@{
            DocPath = $docPath
            Title = $title
            IndexCategory = $indexCategory
            SourceTracePresent = $truthRow.SourceTracePresent
            VerifiedSourceFileCount = $existingSourceFiles.Count
            RuntimeHookTypes = $truthRow.RuntimeHooksCovered
            SerializedClassCount = $serializedClasses.Count
            CoverageStatus = $coverageStatus
            BacklogIds = ''
        }) | Out-Null
    }

    if ($scope -eq 'WikiPage' -and ($canonicalStatus -eq 'Alias' -or $legacySlug))
    {
        $aliasRows.Add([pscustomobject]@{
            AliasPath = $docPath
            AliasType = if ([string]::IsNullOrWhiteSpace($aliasType)) { 'AliasCandidate' } else { $aliasType }
            CanonicalPath = $canonicalTarget
            Indexed = [string]$indexed
            IndependentClaims = $independentAliasClaims
            ExistingBacklogIds = $existingBacklogIds
            GeneratedBacklogId = ''
            Recommendation = if ($independentAliasClaims -eq 'Yes') { 'Reduce alias to a canonical pointer or verify that independent claims are generated from the canonical page.' } else { 'Keep alias only if it preserves legacy links.' }
        }) | Out-Null
    }

    if ($coverageStatus -eq 'NeedsBacklog' -or $missingClaims.Count -gt 0 -or $staleClaims.Count -gt 0)
    {
        $issueTypes = New-Object System.Collections.Generic.List[string]

        if ($missingClaims -match 'Source Trace')
        {
            $issueTypes.Add('MissingSourceTrace') | Out-Null
        }

        if ($missingClaims -match 'not linked')
        {
            $issueTypes.Add('UnindexedWikiPage') | Out-Null
        }

        if ($missingClaims -match 'independent behavior')
        {
            $issueTypes.Add('AliasIndependentClaims') | Out-Null
        }

        if ($missingSourceFiles.Count -gt 0)
        {
            $issueTypes.Add('MissingSourcePath') | Out-Null
        }

        if ($needsRework)
        {
            $issueTypes.Add('NeedsReworkOrTodo') | Out-Null
        }

        if ($issueTypes.Count -eq 0)
        {
            $issueTypes.Add('UnverifiedDocumentation') | Out-Null
        }

        $backlogCandidates.Add([pscustomobject]@{
            DocPath = $docPath
            IssueTypes = ($issueTypes.ToArray() -join ';')
            ExistingBacklogIds = $existingBacklogIds
            GeneratedBacklogId = ''
            Priority = if ($canonicalStatus -eq 'Canonical' -and -not $sourceTracePresent) { 'High' } elseif ($legacySlug -and $independentAliasClaims -eq 'Yes') { 'Medium' } else { 'Medium' }
            Evidence = Trim-Text ((@($staleClaims.ToArray()) + @($missingClaims.ToArray())) -join '; ') 500
            Recommendation = 'Verify against source/register outputs, update or reduce the page, then record the result in docs/wiki/wikibacklog.md or Phase 13 repair backlog.'
            Status = if ($existingBacklogIds.Length -gt 0) { 'ExistingBacklog' } else { 'Open' }
        }) | Out-Null
    }
}

$generatedIndex = 1
foreach ($candidate in $backlogCandidates)
{
    if ([string]::IsNullOrWhiteSpace($candidate.ExistingBacklogIds))
    {
        $candidate.GeneratedBacklogId = 'DOC-{0:D4}' -f $generatedIndex
        $generatedIndex++
    }
}

foreach ($truthRow in $truthRows)
{
    $candidate = @($backlogCandidates | Where-Object { $_.DocPath -eq $truthRow.DocPath } | Select-Object -First 1)

    if ($candidate.Count -gt 0)
    {
        $truthRow.GeneratedBacklogId = $candidate[0].GeneratedBacklogId
    }
}

foreach ($aliasRow in $aliasRows)
{
    $candidate = @($backlogCandidates | Where-Object { $_.DocPath -eq $aliasRow.AliasPath } | Select-Object -First 1)

    if ($candidate.Count -gt 0)
    {
        $aliasRow.GeneratedBacklogId = $candidate[0].GeneratedBacklogId
    }
}

foreach ($canonicalRow in $canonicalRows)
{
    $truthRow = @($truthRows | Where-Object { $_.DocPath -eq $canonicalRow.DocPath } | Select-Object -First 1)

    if ($truthRow.Count -gt 0)
    {
        $ids = @($truthRow[0].ExistingBacklogIds, $truthRow[0].GeneratedBacklogId) | Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
        $canonicalRow.BacklogIds = ($ids -join ';')
    }
}

$coverageRows = New-Object System.Collections.Generic.List[object]
$categoryGroups = @($truthRows | Where-Object { $_.Scope -eq 'WikiPage' } | Group-Object IndexCategory | Sort-Object Name)

foreach ($group in $categoryGroups)
{
    $rows = @($group.Group)
    $coverageRows.Add([pscustomobject]@{
        IndexCategory = if ([string]::IsNullOrWhiteSpace($group.Name)) { 'Unindexed' } else { $group.Name }
        TotalPages = $rows.Count
        CanonicalPages = @($rows | Where-Object { $_.CanonicalPage -eq 'Canonical' }).Count
        AliasPages = @($rows | Where-Object { $_.CanonicalPage -eq 'Alias' }).Count
        SourceTracePresent = @($rows | Where-Object { $_.SourceTracePresent -eq 'Yes' }).Count
        SourceTraceMissing = @($rows | Where-Object { $_.CanonicalPage -eq 'Canonical' -and $_.SourceTracePresent -ne 'Yes' }).Count
        CodeVerified = @($rows | Where-Object { $_.CodeVerified -eq 'True' }).Count
        BackloggedOrGenerated = @($rows | Where-Object { -not [string]::IsNullOrWhiteSpace($_.ExistingBacklogIds) -or -not [string]::IsNullOrWhiteSpace($_.GeneratedBacklogId) }).Count
        RuntimeCoveredPages = @($rows | Where-Object { -not [string]::IsNullOrWhiteSpace($_.RuntimeHooksCovered) }).Count
        SerializationCoveredPages = @($rows | Where-Object { -not [string]::IsNullOrWhiteSpace($_.SerializationCovered) }).Count
    }) | Out-Null
}

$truthRowsArray = @($truthRows.ToArray())
$canonicalRowsArray = @($canonicalRows.ToArray())
$aliasRowsArray = @($aliasRows.ToArray())
$backlogRowsArray = @($backlogCandidates.ToArray())
$coverageRowsArray = @($coverageRows.ToArray())

$truthPath = Export-AuditCsv -Rows $truthRowsArray -FileName 'documentation-truth-table.csv'
$phaseTruthPath = Export-AuditCsv -Rows $truthRowsArray -FileName 'phase-07-documentation-truth-table.csv'
$canonicalPath = Export-AuditCsv -Rows $canonicalRowsArray -FileName 'phase-07-canonical-page-map.csv'
$aliasPath = Export-AuditCsv -Rows $aliasRowsArray -FileName 'phase-07-alias-legacy-slug-map.csv'
$staleBacklogPath = Export-AuditCsv -Rows $backlogRowsArray -FileName 'phase-07-stale-claim-backlog.csv'
$coveragePath = Export-AuditCsv -Rows $coverageRowsArray -FileName 'phase-07-source-trace-coverage-report.csv'

$summaryPath = Join-Path $OutputDir 'phase-07-summary.md'
$canonicalTotal = @($truthRowsArray | Where-Object { $_.CanonicalPage -eq 'Canonical' }).Count
$canonicalMissingTrace = @($truthRowsArray | Where-Object { $_.CanonicalPage -eq 'Canonical' -and $_.SourceTracePresent -ne 'Yes' }).Count
$aliasTotal = @($truthRowsArray | Where-Object { $_.CanonicalPage -eq 'Alias' }).Count
$independentAliasTotal = @($aliasRowsArray | Where-Object { $_.IndependentClaims -eq 'Yes' }).Count

$summaryLines = @(
    '# Phase 7 Documentation Truth Audit Summary',
    '',
    "Generated: $(Get-Date -Format o)",
    '',
    '## Required Inputs',
    '',
    '| Input | Status |',
    '| --- | --- |',
    "| Documentation inventory | Present: ``phase-01-documentation-inventory.csv`` with $($documentationInventory.Count) rows |",
    "| Wiki index | Present: ``docs/wiki/INDEX.md`` with $($indexMap.Count) indexed links |",
    "| Wiki backlog | Present: ``docs/wiki/wikibacklog.md`` with $($wikiBacklogRows.Count) rows |",
    "| System Cards | Present: ``phase-04-system-card-index.csv`` with $($systemCards.Count) rows |",
    "| Runtime Hook Map | Present: ``runtime-hook-map.csv`` with $($runtimeHookMap.Count) rows |",
    "| Serialization Register | Present: ``serialization-register.csv`` with $($serializationRegister.Count) rows |",
    "| Project Truth Register | Present: ``project-truth-register.csv`` with $($projectTruth.Count) rows |",
    '',
    '## Generated Outputs',
    '',
    '| Output | Rows | Purpose |',
    '| --- | ---: | --- |',
    "| ``documentation-truth-table.csv`` | $($truthRowsArray.Count) | Canonical documentation truth table. |",
    "| ``phase-07-documentation-truth-table.csv`` | $($truthRowsArray.Count) | Phase-scoped truth table. |",
    "| ``phase-07-canonical-page-map.csv`` | $($canonicalRowsArray.Count) | Canonical indexed page map with source trace and coverage status. |",
    "| ``phase-07-alias-legacy-slug-map.csv`` | $($aliasRowsArray.Count) | Alias and legacy slug map with canonical targets and independent-claim flags. |",
    "| ``phase-07-stale-claim-backlog.csv`` | $($backlogRowsArray.Count) | Generated documentation verification backlog candidates. |",
    "| ``phase-07-source-trace-coverage-report.csv`` | $($coverageRowsArray.Count) | Source trace coverage grouped by wiki index category. |",
    '',
    '## Coverage Counts',
    '',
    "| Metric | Count |",
    "| --- | ---: |",
    "| Canonical wiki pages | $canonicalTotal |",
    "| Canonical pages missing Source Trace | $canonicalMissingTrace |",
    "| Alias or legacy slug pages | $aliasTotal |",
    "| Alias pages with independent claims | $independentAliasTotal |",
    "| Generated backlog candidates | $($backlogRowsArray.Count) |",
    '',
    '## Exit Criteria',
    '',
    '- Wiki pages are classified as canonical, alias, support, audit register, or unknown.',
    '- Canonical pages missing source trace, indexed pages with missing source evidence, aliases with independent claims, stale markers, and missing source paths are backlogged instead of edited in Phase 7.',
    '- Runtime and serialization coverage are derived from traced source files and the Phase 5/6 registers.',
    '- This phase does not modify wiki pages; fixes are deferred to the wiki backlog or Phase 13 repair backlog.'
)

Set-Content -LiteralPath $summaryPath -Value $summaryLines -Encoding UTF8

[pscustomobject]@{
    DocumentationRows = $truthRowsArray.Count
    CanonicalRows = $canonicalRowsArray.Count
    AliasRows = $aliasRowsArray.Count
    BacklogRows = $backlogRowsArray.Count
    CoverageRows = $coverageRowsArray.Count
    CanonicalMissingSourceTrace = $canonicalMissingTrace
    IndependentAliasClaims = $independentAliasTotal
    DocumentationTruthTable = Convert-ToRepoPath $truthPath
    PhaseTruthTable = Convert-ToRepoPath $phaseTruthPath
    CanonicalPageMap = Convert-ToRepoPath $canonicalPath
    AliasMap = Convert-ToRepoPath $aliasPath
    StaleClaimBacklog = Convert-ToRepoPath $staleBacklogPath
    CoverageReport = Convert-ToRepoPath $coveragePath
    Summary = Convert-ToRepoPath $summaryPath
}
